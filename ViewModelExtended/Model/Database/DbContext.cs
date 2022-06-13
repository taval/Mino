using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System;
using System.Collections.Generic;
using System.Linq;

// TODO/NOTE: EntityXItemRemove only works if the item is found again using the id; passing the object gives a duplicate tracking error which so far can't simply be resolved by detaching the temp entities from db upon creation. The problem is probably not here but some object from send/receive that is setting a temporary id somehow

namespace ViewModelExtended.Model
{
	public class DbContext : Microsoft.EntityFrameworkCore.DbContext, IDbContext
	{
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

		public IQueryable<GroupItem> GetAllGroupItems ()
		{
			return GroupItems;
		}

		public IQueryable<GroupItem> GetGroupItemsInGroup (Group groop)
		{
			return GroupItems.Where(row => row.GroupId == groop.Id);
		}

		#endregion



		#region Data Objects (temporary)

		public Node CreateNode (Node? previous, Node? next, bool temporary = false)
		{
			Node output = new Node() { PreviousId = previous?.Id, NextId = next?.Id };

			if (temporary == false) Nodes.Add(output);

			return output;
		}

		public void UpdateNode (Node target, Node? previous, Node? next, bool temporary = false)
		{
			// always assign even if null
			target.PreviousId = previous?.Id;
			target.NextId = next?.Id;

			if (temporary == false) Nodes.Update(target);

			if (previous != null) {
				previous.NextId = target.Id;
				if (temporary == false) Nodes.Update(previous);
			}
			if (next != null) {
				next.PreviousId = target.Id;
				if (temporary == false) Nodes.Update(next);
			}
		}

		public void DeleteNode (Node target)
		{
			Nodes.Remove(target);
		}

		public Timestamp CreateTimestamp (bool temporary = false)
		{
			Timestamp output = new Timestamp();

			if (temporary == false) Timestamps.Add(output);

			return output;
		}

		public void UpdateTimestamp (
			Timestamp target, long? userModified, long? userIndexed, long? autoModified, bool temporary = false)
		{
			// only assign if not null
			if (userModified == null && userIndexed == null && autoModified == null) return;
			if (userModified != null) target.UserModified = userModified;
			if (userIndexed != null) target.UserIndexed = userIndexed;
			if (autoModified != null) target.AutoModified = autoModified;
			if (temporary == false) Timestamps.Update(target);
		}

		public void DeleteTimestamp (Timestamp target)
		{
			Timestamps.Remove(target);
		}

		//public Note CreateNote (string title, string text, bool temporary = false)
		//{
		//	Note output = new Note() { Title = title, Text = text };

		//	if (temporary == false) Notes.Add(output);

		//	return output;
		//}

		//public void UpdateNote (Note target, string? title, string? text, bool temporary = false)
		//{
		//	if (title != null) {
		//		target.Title = title;
		//	}
		//	if (text != null) {
		//		target.Text = text;
		//	}
		//	if (temporary == false) Notes.Update(target);
		//}
		public Note CreateNote (string title, string text, int priority, bool temporary = false)
		{
			Note output = new Note() { Title = title, Text = text, Priority = priority };

			if (temporary == false) Notes.Add(output);

			return output;
		}

		public void UpdateNote (Note target, string? title, string? text, int? priority, bool temporary = false)
		{
			if (title != null) {
				target.Title = title;
			}
			if (text != null) {
				target.Text = text;
			}
			if (priority != null) {
				target.Priority = (int)priority;
			}
			if (temporary == false) Notes.Update(target);
		}

		public void DeleteNote (Note target)
		{
			Notes.Remove(target);
		}

		public Group CreateGroup (string title, string color, bool temporary = false)
		{
			Group output = new Group() { Title = title, Color = color };

			if (temporary == false) Groups.Add(output);

			return output;
		}

		public void UpdateGroup (Group target, string? title, string? color, bool temporary = false)
		{
			if (title != null) {
				target.Title = title;
			}
			if (color != null) {
				target.Color = color;
			}
			if (temporary == false) Groups.Update(target);
		}

		public void DeleteGroup (Group target)
		{
			Groups.Remove(target);
		}

		#endregion



		#region Composite Objects (persistent / interface)

		public NoteListItem CreateNoteListItem (IObject root, Note data, bool temporary = false)
		{
			NoteListItem output = new NoteListItem()
			{
				NodeId = root.Node.Id,
				TimestampId = root.Timestamp.Id,
				ObjectId = data.Id
			};

			if (temporary == false) NoteListItems.Add(output);

			return output;
		}

		public void UpdateNoteListItem (bool temporary = false)
		{
			//SaveChanges();
		}

		public void DeleteNoteListItem (NoteListItem target)
		{
			//// remove any GroupItems derived from the Note data point
			//foreach (GroupItem groupItem in GroupItems) {
			//	if (groupItem.ObjectId == target.ObjectId) {
			//		DeleteGroupItem(groupItem);
			//	}
			//}
			DeleteNode(Nodes.Find(target.NodeId));
			DeleteTimestamp(Timestamps.Find(target.TimestampId));
			DeleteNote(Notes.Find(target.ObjectId));
			//NoteListItems.Remove(target);
			NoteListItems.Remove(NoteListItems.Find(target.Id));
		}

		public GroupListItem CreateGroupListItem (IObject root, Group data, bool temporary = false)
		{
			GroupListItem output = new GroupListItem()
			{
				ObjectId = data.Id,
				NodeId = root.Node.Id,
				TimestampId = root.Timestamp.Id
			};

			if (temporary == false) GroupListItems.Add(output);

			return output;
		}

		public void UpdateGroupListItem (bool temporary = false)
		{
			//SaveChanges();
		}

		public void DeleteGroupListItem (GroupListItem target)
		{
			DeleteNode(Nodes.Find(target.NodeId));
			DeleteTimestamp(Timestamps.Find(target.TimestampId));
			DeleteGroup(Groups.Find(target.ObjectId));
			//GroupListItems.Remove(target);
			GroupListItems.Remove(GroupListItems.Find(target.Id));
		}

		public GroupItem CreateGroupItem (IObject root, Group groop, Note data, bool temporary = false)
		{
			GroupItem output = new GroupItem()
			{
				GroupId = groop.Id,
				ObjectId = data.Id,
				NodeId = root.Node.Id,
				TimestampId = root.Timestamp.Id
			};

			if (temporary == false) GroupItems.Add(output);

			return output;
		}

		public void UpdateGroupItem (bool temporary = false)
		{
			//SaveChanges();
		}

		public void DeleteGroupItem (GroupItem target)
		{
			DeleteNode(Nodes.Find(target.NodeId));
			DeleteTimestamp(Timestamps.Find(target.TimestampId));
			//GroupItems.Remove(target);
			GroupItems.Remove(GroupItems.Find(target.Id));
		}

		#endregion



		#region Instance Objects: Create

		public ObjectRoot CreateObjectRoot (Node node, Timestamp timestamp)
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



		#region Instance Objects: Update

		public void UpdateNoteListObject (NoteListObject target, bool temporary = false)
		{
			UpdateObjectRoot(target);
			if (temporary == false) Notes.Update(target.Data);
		}

		public void UpdateGroupListObject (GroupListObject target, bool temporary = false)
		{
			UpdateObjectRoot(target);
			if (temporary == false) Groups.Update(target.Data);
		}

		public void UpdateGroupObject (GroupObject target, bool temporary = false)
		{
			UpdateObjectRoot(target);
		}

		public void UpdateObjectRoot (IObject target, bool temporary = false)
		{
			Nodes.Update(target.Node);
			if (temporary == false) Timestamps.Update(target.Timestamp);
		}

		#endregion



		#region Instance Objects: Destructors

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



		#region Save

		public void Save ()
		{
			SaveChanges();
		}

		#endregion



		#region Configuration

		/// <summary>
		/// The following configures EF to create a Sqlite database file in the special "local" folder for your platform.
		/// </summary>
		/// <param name="options"></param>
		protected override void OnConfiguring (DbContextOptionsBuilder options)
		{
			options.UseSqlite($"Data Source={ DbPath }");
			//options.EnableSensitiveDataLogging(true); // TODO: remove logging in production
		}

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
			sqlSequence("Timestamps");

			//GroupItems.RemoveRange(GroupItems);
			//GroupListItems.RemoveRange(GroupListItems);
			//Groups.RemoveRange(Groups);
			//Nodes.RemoveRange(Nodes);
			//NoteListItems.RemoveRange(NoteListItems);
			//Timestamps.RemoveRange(Timestamps);
		}

		#endregion
	}
}



// NOTE: how to on executing stored procedures from EF: https://entityframework.net/stored-procedure (not used/allowed in this app but relates to raw queries)

// NOTE - target can get scrambled if accidentally installing a full .NET package (v5, 6, or 7) instead of .NET Core (3.1) - see .csproj for ViewModelExtended

// NOTE - in general, attributes should be attached to the input interface, e.g. View and its cooresponding ViewModel. Model validation should remain in the model.

// TODO: DbSets (table representations) don't need to be publicly accessible, however the IQueryable interface is output from the methods which will be publicly accessible.
// Furthermore those returns could be further refined to only return exactly the data needed by the viewmodel and could be facaded away. To this end, create a class to encapsulate DbContext, DbQueryHelper, and ViewModelCreator. ViewModelResource could act as not just a data struct but as the frontend for these objects. 

