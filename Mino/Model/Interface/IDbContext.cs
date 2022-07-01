using System;
using System.Linq;
using Microsoft.EntityFrameworkCore.ChangeTracking;



namespace Mino.Model
{
	/// <summary>

	/// interface for Mino database context. performs all necessary data abstraction layer operations.

	/// allows temporary (managed by GC) and permanent (managed by DbContext)
	/// GC managed entities should not use the disposal methods here - these are intended only for Db entities

	/// Use caution when manually managing simple objects -
	/// composite objects manage existing simple objects and so care must be taken to not leave strays behind when using temps,
	/// and to call the composite delete method to cleanup any associated simple objects

	/// Model conventions
	/// - Id is private set - assuming that only EF/db is setting the model's id.
	/// - parameterless constructor is called by and most object instantiation done via initializers ({} syntax).
	/// - using custom object hierarchy for association (lacking many ef core conventions)
	///   e.g. a Note may be associated with a Group, but could it also be independently associated with a Category, SubGroup, etc?
	/// - Item: stores Id of each entity that composes a composite model
	/// - Object: stores instance of each entity that composes a composite model plus the Item
	/// </summary>

	/// TODO: associations may be formalized within the fluent configuration

	public interface IDbContext : IDisposable
	{
		#region Queries

		public IQueryable<Node> GetAllNodes ();
		public IQueryable<Timestamp> GetAllTimestamps ();
		public IQueryable<Note> GetAllNotes ();
		public IQueryable<Group> GetAllGroups ();
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

		public Note CreateNote (string title, string text, int priority, bool temporary = false);
		public void UpdateNote (Note target, string? title, string? text, int? priority, bool temporary = false);
		public void DeleteNote (Note target);

		public Group CreateGroup (string title, string color, bool temporary = false);
		public void UpdateGroup (Group target, string? title, string? color, bool temporary = false);
		public void DeleteGroup (Group target);

        #endregion

        #region Composite Table Objects

        public NoteListItem CreateNoteListItem (IObject root, Note data, bool temporary = false);
		public void DeleteNoteListItem (NoteListItem target);

		public GroupListItem CreateGroupListItem (IObject root, Group data, bool temporary = false);
		public void DeleteGroupListItem (GroupListItem target);

		public GroupItem CreateGroupItem (IObject root, Group groop, Note data, bool temporary = false);
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
		/// perform migrations, create database if does not exist
		/// </summary>
		public void Migrate ();

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
