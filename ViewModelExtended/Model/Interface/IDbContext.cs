using System;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;



namespace ViewModelExtended.Model
{
	/// <summary>
	/// interface for Mino database context. performs all necessary data abstraction layer operations.
	/// NOTE: it would be wise to use caution when manually managing simple objects -
	/// composite objects manage existing simple objects and so care must be taken to not leave strays behind when using temps,
	/// and to call the composite delete method to cleanup any associated simple objects
	/// </summary>
	public interface IDbContext : IDisposable
	{
		// expose identity tracking
		public EntityEntry<TEntity> Entry<TEntity> (TEntity entity) where TEntity : class;

		// save changes
		public void Save ();

		// flush entire db
		public void Reset ();

		#region Tables

		public DbSet<Node> Nodes { get; set; }
		public DbSet<Timestamp> Timestamps { get; set; }
		public DbSet<Note> Notes { get; set; }
		public DbSet<Group> Groups { get; set; }
		public DbSet<NoteListItem> NoteListItems { get; set; }
		public DbSet<GroupListItem> GroupListItems { get; set; }
		public DbSet<GroupItem> GroupItems { get; set; }

		#endregion

		#region Queries

		public IQueryable<NoteListItem> GetAllNoteListItems ();
		public IQueryable<GroupListItem> GetAllGroupListItems ();
		public IQueryable<GroupItem> GetAllGroupItems ();
		public IQueryable<GroupItem> GetGroupItemsInGroup (Group groop);

		#endregion

		#region Simple Table Objects

		public Node CreateNode (Node? previous, Node? next);
		public void UpdateNode (Node target, Node? previous, Node? next);
		public void DeleteNode (Node target);

		public Timestamp CreateTimestamp ();
		public void UpdateTimestamp (Timestamp target, long? userModified, long? userIndexed, long? autoModified);
		public void DeleteTimestamp (Timestamp target);

		public Note CreateNote (string title, string text);
		public void UpdateNote (Note target, string? title, string? text);
		public void DeleteNote (Note target);

		public Group CreateGroup (string title, string color);
		public void UpdateGroup (Group target, string? title, string? color);
		public void DeleteGroup (Group target);

        #endregion

        #region Composite Table Objects

        public NoteListItem CreateNoteListItem (IObject root, Note data);
		public void UpdateNoteListItem ();
		public void DeleteNoteListItem (NoteListItem target);

		public GroupListItem CreateGroupListItem (IObject root, Group data);
		public void UpdateGroupListItem ();
		public void DeleteGroupListItem (GroupListItem target);

		public GroupItem CreateGroupItem (IObject root, Group groop, Note data);
		public void UpdateGroupItem ();
		public void DeleteGroupItem (GroupItem target);

		#endregion

		#region Instantiated Objects

		public NoteListObject CreateNoteListObject (NoteListItem item, IObject root, Note data);
		public void UpdateNoteListObject (NoteListObject target);
		public void DeleteNoteListObject (NoteListObject target);

		public GroupListObject CreateGroupListObject (GroupListItem item, IObject root, Group data);
		public void UpdateGroupListObject (GroupListObject target);
		public void DeleteGroupListObject (GroupListObject target);

		public GroupObject CreateGroupObject (GroupItem item, IObject root, Group groop, Note data);
		public void UpdateGroupObject (GroupObject target);
		public void DeleteGroupObject (GroupObject target);

		public ObjectRoot CreateObjectRoot (Node node, Timestamp timestamp);
		public void UpdateObjectRoot (IObject target);
		public void DeleteObjectRoot (IObject target);

		#endregion
	}
}
