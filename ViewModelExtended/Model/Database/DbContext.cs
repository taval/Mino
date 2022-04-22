using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;



/* NOTE:
 * using DbContext looks like a Builder pattern, e.g.
 *
 * // create
 * Note note = CreateNote("Hello", "World");
 * Node node = CreateNode(null, null);
 * Timestamp timestamp = CreateTimestamp();
 * CreateNoteListItem(note, node, timestamp);
 *
 * // update
 * UpdateNote(note, "GoodBye", null);
 * UpdateNode(node, node, node);
 * UpdateNoteListItem();
 */



namespace ViewModelExtended.Model
{
	public class DbContext : Microsoft.EntityFrameworkCore.DbContext, IDbContext
	{
		public void Save ()
		{
			SaveChanges();
		}



		#region	Path

		private string DbPath { get; set; }

		#endregion



		#region Tables

		public DbSet<Node> Nodes { get; set; }
		public DbSet<Timestamp> Timestamps { get; set; }
		public DbSet<Note> Notes { get; set; }
		public DbSet<Group> Groups { get; set; }
		public DbSet<NoteListItem> NoteListItems { get; set; }
		public DbSet<GroupListItem> GroupListItems { get; set; }
		public DbSet<GroupItem> GroupItems { get; set; }

		#endregion



		#region Constructor

		public DbContext ()
		{
			// set db name and location
			var folder = Environment.SpecialFolder.LocalApplicationData;
			var path = Environment.GetFolderPath(folder);
			DbPath = System.IO.Path.Join(path, Config.dbFile);

			Nodes = Set<Node>();
			Timestamps = Set<Timestamp>();
			Notes = Set<Note>();
			GroupListItems = Set<GroupListItem>();
			Groups = Set<Group>();
			NoteListItems = Set<NoteListItem>();
			GroupListItems = Set<GroupListItem>();
			GroupItems = Set<GroupItem>();
		}

		#endregion



		#region Queries

		public IQueryable<NoteListItem> GetAllNoteListItems ()
		{
			return NoteListItems;
		}

		public IQueryable<GroupListItem> GetAllGroupListItems ()
		{
			return GroupListItems;
		}

		public IQueryable<GroupItem> GetGroupItemsInGroup (Group groop)
		{
			return GroupItems.Where(row => row.GroupId == groop.Id);
		}

		#endregion



		#region Data Objects (temporary)

		public Node CreateNode (INode? previous, INode? next)
		{
			Node output = new Node() { PreviousId = previous?.Id, NextId = next?.Id };

			Nodes.Add(output);

			return output;
		}

		// TODO: when selecting a GroupListObject, this is called inadvertently sometimes
		public void UpdateNode (INode target, INode? previous, INode? next)
		{
			// always assign even if null
			target.PreviousId = previous?.Id;
			target.NextId = next?.Id;

			Nodes.Update((Node)target);

			if (previous != null) {
				previous.NextId = target.Id;
				Nodes.Update((Node)previous);
			}
			if (next != null) {
				next.PreviousId = target.Id;
				Nodes.Update((Node)next);
			}
		}

		public void DeleteNode (INode target)
		{
			Nodes.Remove((Node)target);
		}

		public Timestamp CreateTimestamp ()
		{
			Timestamp output = new Timestamp();

			Timestamps.Add(output);

			return output;
		}

		public void UpdateTimestamp (Timestamp target, int? userModified, int? userIndexed, int? autoModified)
		{
			// only assign if not null
			if (userModified == null && userIndexed == null && autoModified == null) {
				return;
			}
			if (userModified != null) {
				target.UserModified = userModified;
			}
			if (userIndexed != null) {
				target.UserIndexed = userIndexed;
			}
			if (autoModified != null) {
				target.AutoModified = autoModified;
			}
			Timestamps.Update(target);
		}

		public void DeleteTimestamp (Timestamp target)
		{
			Timestamps.Remove(target);
		}

		public Note CreateNote (string title, string text)
		{
			Note output = new Note() { Title = title, Text = text };

			Notes.Add(output);

			return output;
		}

		public void UpdateNote (Note target, string? title, string? text)
		{
			if (title != null) {
				target.Title = title;
			}
			if (text != null) {
				target.Text = text;
			}
			Notes.Update(target);
		}

		public void DeleteNote (Note target)
		{
			Notes.Remove(target);
		}

		public Group CreateGroup (string title, string color)
		{
			Group output = new Group() { Title = title, Color = color };

			Groups.Add(output);

			return output;
		}

		public void UpdateGroup (Group target, string? title, string? color)
		{
			if (title != null) {
				target.Title = title;
			}
			if (color != null) {
				target.Color = color;
			}
			Groups.Update(target);
		}

		public void DeleteGroup (Group target)
		{
			Groups.Remove(target);
		}

		#endregion



		#region Composite Objects (persistent / interface)

		public NoteListItem CreateNoteListItem (IObject root, Note data)
		{
			NoteListItem output = new NoteListItem()
			{
				ObjectId = data.Id,
				NodeId = root.Node.Id,
				TimestampId = root.Timestamp.Id
			};

			NoteListItems.Add(output);

			return output;
		}

		public void UpdateNoteListItem ()
		{
			//SaveChanges();
		}

		public void DeleteNoteListItem (NoteListItem target)
		{
			// remove any GroupItems derived from the Note data point
			foreach (GroupItem groupItem in GroupItems) {
				if (groupItem.ObjectId == target.ObjectId) {
					DeleteGroupItem(groupItem);
				}
			}
			DeleteNode(Nodes.Find(target.NodeId));
			DeleteTimestamp(Timestamps.Find(target.TimestampId));
			DeleteNote(Notes.Find(target.ObjectId));
			NoteListItems.Remove(target);
		}

		public GroupListItem CreateGroupListItem (IObject root, Group data)
		{
			GroupListItem output = new GroupListItem()
			{
				ObjectId = data.Id,
				NodeId = root.Node.Id,
				TimestampId = root.Timestamp.Id
			};

			GroupListItems.Add(output);

			return output;
		}

		public void UpdateGroupListItem ()
		{
			//SaveChanges();
		}

		public void DeleteGroupListItem (GroupListItem target)
		{
			IQueryable<GroupItem> groupItems = GetGroupItemsInGroup(Groups.Find(target.ObjectId));

			foreach (GroupItem item in groupItems) {
				DeleteGroupItem(item);
			}
			DeleteNode(Nodes.Find(target.NodeId));
			DeleteTimestamp(Timestamps.Find(target.TimestampId));
			DeleteGroup(Groups.Find(target.ObjectId));
			GroupListItems.Remove(target);
		}

		public GroupItem CreateGroupItem (IObject root, Group groop, Note data)
		{
			GroupItem output = new GroupItem()
			{
				GroupId = groop.Id,
				ObjectId = data.Id,
				NodeId = root.Node.Id,
				TimestampId = root.Timestamp.Id
			};

			GroupItems.Add(output);

			return output;
		}

		public void UpdateGroupItem ()
		{
			//SaveChanges();
		}

		public void DeleteGroupItem (GroupItem target)
		{
			DeleteNode(Nodes.Find(target.NodeId));
			DeleteTimestamp(Timestamps.Find(target.TimestampId));
			GroupItems.Remove(target);
		}

		#endregion



		#region Instance Objects

		public ObjectRoot CreateObjectRoot (INode node, Timestamp timestamp)
		{
			return new ObjectRoot(node, timestamp);
		}

		public NoteListObject CreateNoteListObject (NoteListItem item, IObject root, Note data)
		{
			return new NoteListObject(item, root, data);
		}

		public GroupListObject CreateGroupListObject (GroupListItem item, IObject root, Group data)
		{
			return new GroupListObject(item, root, data);
		}

		public GroupObject CreateGroupObject (GroupItem item, IObject root, Group groop, Note data)
		{
			return new GroupObject(item, root, groop, data);
		}

		#endregion



		#region Destructors

		public void DeleteNoteListObject (NoteListObject target)
		{
			DeleteNoteListItem(target.Item);
		}

		public void DeleteGroupListObject (GroupListObject target)
		{
			DeleteGroupListItem(target.Item);
		}

		public void DeleteGroupObject (GroupObject target)
		{
			DeleteGroupItem(target.Item);
		}

		// NOTE: of limited use if the list items delete the nodes and timestamps already
		public void DeleteObjectRoot (IObject target)
		{
			DeleteNode(target.Node);
			DeleteTimestamp(target.Timestamp);
		}

		#endregion



		#region Configuration

		/// <summary>
		/// The following configures EF to create a Sqlite database file in the special "local" folder for your platform.
		/// </summary>
		/// <param name="options"></param>
		protected override void OnConfiguring (DbContextOptionsBuilder options)
			=> options.UseSqlite($"Data Source={ DbPath }");

		/// <summary>
		/// configure model with custom columns and data
		/// </summary>
		/// <param name="modelBuilder"></param>
		protected override void OnModelCreating (ModelBuilder modelBuilder)
		{
			//modelBuilder.Entity<Group>()
			//	.Property(b => b.Url)
			//	.IsRequired();

			//modelBuilder.Entity<Note>()
			//	.Property(b => b.Url)
			//	.IsRequired();

			//modelBuilder.Entity<Node>()
			//	.Property(b => b.Url)
			//	.IsRequired();

			// see https://docs.microsoft.com/en-us/ef/ef6/modeling/code-first/fluent/types-and-properties
			// see also https://khalidabuhakmeh.com/how-to-add-a-view-to-an-entity-framework-core-dbcontext
			//modelBuilder.Entity<NoteSorted>(ens => {
			//	ens.HasNoKey();
			//	ens.ToView("v_NotesSorted");
			//});
		}

		// TODO: for testing only, remove access in production
		private delegate void SqlSequence (string tableName);
		public void Reset ()
		{
			SqlSequence sqlSequence = delegate (string tableName)
			{
				string seq =
					$"DELETE FROM { tableName }; UPDATE sqlite_sequence SET seq = 0 WHERE name = '{ tableName }';";

				Database.ExecuteSqlRaw(seq);
			};

			sqlSequence("Notes");
			sqlSequence("GroupItems");
			sqlSequence("GroupListItems");
			sqlSequence("Groups");
			sqlSequence("Nodes");
			sqlSequence("NoteListItems");
			sqlSequence("States");
			sqlSequence("Timestamps");

			//GroupItems.RemoveRange(GroupItems);
			//GroupListItems.RemoveRange(GroupListItems);
			//Groups.RemoveRange(Groups);
			//Nodes.RemoveRange(Nodes);
			//NoteListItems.RemoveRange(NoteListItems);
			//States.RemoveRange(States);
			//Timestamps.RemoveRange(Timestamps);
		}

		#endregion
	}
}



// NOTE: how to on executing stored procedures from EF: https://entityframework.net/stored-procedure (not used/allowed in this app but relates to raw queries)

// NOTE - target can get scrambled if accidentally installing a full .NET package (v5, 6, or 7) instead of .NET Core (3.1) - see .csproj for ViewModelExtended

// NOTE - in general, attributes should be attached to the input interface, e.g. View and its cooresponding ViewModel. Model validation should remain in the model.

// TODO: DbSets (table representations) don't need to be publicly accessible, however the IQueryable interface is output from the methods which will be publicly accessible.
// Furthermore those returns could be further refined to only return exactly the data needed by the viewmodel and could be facaded away

