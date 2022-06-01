using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ViewModelExtended.Model;



namespace ViewModelExtended.ViewModel
{
	public interface IDbQueryHelper
	{
		/// <summary>
		/// load fully formed NoteListObjects from database
		/// </summary>
		/// <param name="dbContext"></param>
		/// <returns></returns>
		public IQueryable<NoteListObjectViewModel> GetAllNoteListObjects (IDbContext dbContext);

		/// <summary>
		/// load fully formed GroupListObjects from database
		/// </summary>
		/// <param name="dbContext"></param>
		/// <returns></returns>
		public IQueryable<GroupListObjectViewModel> GetAllGroupListObjects (IDbContext dbContext);

		/// <summary>
		/// load dependency-free GroupObject components from database
		/// (to later associate each with a NoteListObject client-side)
		/// the end group is known client-side so not passing along to create the full GroupObject should be ok
		/// </summary>
		/// <param name="dbContext"></param>
		/// <param name="groop"></param>
		/// <returns></returns>
		public IQueryable<Tuple<GroupItem, ObjectRoot>> GetGroupItemsInGroup (IDbContext dbContext, Group groop);

		/// <summary>
		/// get a collection of GroupObjects associated with a particular known Note model
		/// </summary>
		/// <param name="dbContext"></param>
		/// <param name="note"></param>
		/// <returns></returns>
		public IQueryable<GroupObjectViewModel> GetGroupObjectsByNote (IDbContext dbContext, Note note);
	}
}
