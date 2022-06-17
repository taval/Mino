using System;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;



namespace ViewModelExtended.Model
{
	/// <summary>
	
	/// interface for Mino database context. performs all necessary data abstraction layer operations.
	
	/// allows temporary (managed by GC) and permanent (managed by DbContext)
	/// GC managed entities should not use the disposal methods here - these are intented only for Db entities
	
	/// Use caution when manually managing simple objects -
	/// composite objects manage existing simple objects and so care must be taken to not leave strays behind when using temps,
	/// and to call the composite delete method to cleanup any associated simple objects
	
	/// </summary>
	
	public interface IDbContext : IDisposable
	{
		#region Tables

		public DbSet<Node> Nodes { get; set; }
		public DbSet<Timestamp> Timestamps { get; set; }
		public DbSet<Note> Notes { get; set; }
		public DbSet<Group> Groups { get; set; }
		public DbSet<NoteListItem> NoteListItems { get; set; }
		public DbSet<GroupListItem> GroupListItems { get; set; }
		public DbSet<GroupItem> GroupItems { get; set; }
		public DbSet<State> States { get; set; }


		#endregion

		#region Queries

		public IQueryable<NoteListItem> GetAllNoteListItems ();
		public IQueryable<GroupListItem> GetAllGroupListItems ();
		public IQueryable<GroupItem> GetAllGroupItems ();
		public IQueryable<GroupItem> GetGroupItemsInGroup (Group groop);
		public IQueryable<State> GetState (string key);

		#endregion

		#region Simple Table Objects

		public State CreateState (string key, int value);
		public void UpdateState (State state, int value);
		public void DeleteState (State state);

		public Node CreateNode (Node? previous, Node? next, bool temporary = false);
		public void UpdateNode (Node target, Node? previous, Node? next, bool temporary = false);
		public void DeleteNode (Node target);

		public Timestamp CreateTimestamp (bool temporary = false);
		public void UpdateTimestamp (
			Timestamp target, long? userModified, long? userIndexed, long? autoModified, bool temporary = false);
		public void DeleteTimestamp (Timestamp target);

		//public Note CreateNote (string title, string text, bool temporary = false);
		//public void UpdateNote (Note target, string? title, string? text, bool temporary = false);
		public Note CreateNote (string title, string text, int priority, bool temporary = false);
		public void UpdateNote (Note target, string? title, string? text, int? priority, bool temporary = false);
		public void DeleteNote (Note target);

		public Group CreateGroup (string title, string color, bool temporary = false);
		public void UpdateGroup (Group target, string? title, string? color, bool temporary = false);
		public void DeleteGroup (Group target);

        #endregion

        #region Composite Table Objects

        public NoteListItem CreateNoteListItem (IObject root, Note data, bool temporary = false);
		public void UpdateNoteListItem (bool temporary = false);
		public void DeleteNoteListItem (NoteListItem target);

		public GroupListItem CreateGroupListItem (IObject root, Group data, bool temporary = false);
		public void UpdateGroupListItem (bool temporary = false);
		public void DeleteGroupListItem (GroupListItem target);

		public GroupItem CreateGroupItem (IObject root, Group groop, Note data, bool temporary = false);
		public void UpdateGroupItem (bool temporary = false);
		public void DeleteGroupItem (GroupItem target);

		#endregion

		#region Instantiated Objects

		public NoteListObject CreateNoteListObject (NoteListItem item, IObject root, Note data);
		public void UpdateNoteListObject (NoteListObject target, bool temporary = false);
		public void DeleteNoteListObject (NoteListObject target);

		public GroupListObject CreateGroupListObject (GroupListItem item, IObject root, Group data);
		public void UpdateGroupListObject (GroupListObject target, bool temporary = false);
		public void DeleteGroupListObject (GroupListObject target);

		public GroupObject CreateGroupObject (GroupItem item, IObject root, Group groop, Note data);
		public void UpdateGroupObject (GroupObject target, bool temporary = false);
		public void DeleteGroupObject (GroupObject target);

		public ObjectRoot CreateObjectRoot (Node node, Timestamp timestamp);
		public void UpdateObjectRoot (IObject target, bool temporary = false);
		public void DeleteObjectRoot (IObject target);

		#endregion

		#region Methods

		/// <summary>
		/// identity tracking for singular entities
		/// </summary>
		/// <typeparam name="TEntity"></typeparam>
		/// <param name="entity"></param>
		/// <returns></returns>
		public EntityEntry<TEntity> Entry<TEntity> (TEntity entity) where TEntity : class;

		/// <summary>
		/// save changes
		/// </summary>
		public void Save ();

		/// <summary>
		/// flush entire db
		/// </summary>
		public void Reset ();

		#endregion
	}
}
