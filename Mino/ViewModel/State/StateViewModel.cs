using System.Collections.Generic;
using System.Linq;
using Mino.Model;

// This has no necessary relationship with the view - rename to StateService or something like that to reflect that it is only used in accessing db

namespace Mino.ViewModel
{
	public class StateViewModel : ViewModelBase
	{
		/// <summary>
		/// configuration value for the checkbox denoting the possibility of creating ad-hoc groups in the form
		/// </summary>
		public bool IsNewGroupAllowed {
			get { return f_IsNewGroupAllowed; }
			set { Set(ref f_IsNewGroupAllowed, value); }
		}

		private bool f_IsNewGroupAllowed;

		/// <summary>
		/// the id of the selected NoteListItem (Note)
		/// </summary>
		public int? SelectedNoteListItemId {
			get { return f_SelectedNoteListItemId; }
			set { Set(ref f_SelectedNoteListItemId, value); }
		}

		private int? f_SelectedNoteListItemId;

		/// <summary>
		/// the id of the selected GroupListItem (Group)
		/// </summary>
		public int? SelectedGroupListItemId {
			get { return f_SelectedGroupListItemId; }
			set { Set(ref f_SelectedGroupListItemId, value); }
		}

		private int? f_SelectedGroupListItemId;

		/// <summary>
		/// the id of the selected GroupItem (GroupNote)
		/// </summary>
		public int? SelectedGroupItemId {
			get { return f_SelectedGroupItemId; }
			set { Set(ref f_SelectedGroupItemId, value); }
		}

		private int? f_SelectedGroupItemId;


		private readonly IViewModelKit f_Kit;


		public StateViewModel (IViewModelKit kit)
		{
			f_Kit = kit;
		}

		public void Load ()
		{
			using (IDbContext dbContext = f_Kit.CreateDbContext()) {
				// IsNewGroupAllowed
				IQueryable<State> isNewGroupAllowedMatch = dbContext.GetState("IsNewGroupAllowed");
				if (isNewGroupAllowedMatch.Any()) {
					f_IsNewGroupAllowed = ((isNewGroupAllowedMatch.Single().Value == 1) ? true : false);
				}
				else {
					f_IsNewGroupAllowed = false;
				}

				// SelectedNoteListItemId
				IQueryable<State> selectedNoteListItemMatch = dbContext.GetState("SelectedNoteListItemId");
				if (selectedNoteListItemMatch.Any()) {
					f_SelectedNoteListItemId = selectedNoteListItemMatch.Single().Value;
				}
				else {
					f_SelectedNoteListItemId = null;
				}

				// SelectedGroupListItemId
				IQueryable<State> selectedGroupListItemMatch = dbContext.GetState("SelectedGroupListItemId");
				if (selectedGroupListItemMatch.Any()) {
					f_SelectedGroupListItemId = selectedGroupListItemMatch.Single().Value;
				}
				else {
					f_SelectedGroupListItemId = null;
				}

				// SelectedGroupItemId
				IQueryable<State> selectedGroupItemMatch = dbContext.GetState("SelectedGroupItemId");
				if (selectedGroupItemMatch.Any()) {
					f_SelectedGroupItemId = selectedGroupItemMatch.Single().Value;
				}
				else {
					f_SelectedGroupItemId = null;
				}
			}
		}

		public void Shutdown ()
		{
			// persist last user state
			using (IDbContext dbContext = f_Kit.CreateDbContext()) {
				IDictionary<string, int?> values = new Dictionary<string, int?>();

				values["IsNewGroupAllowed"] = (f_IsNewGroupAllowed) ? 1 : 0;
				values["SelectedNoteListItemId"] = f_SelectedNoteListItemId;
				values["SelectedGroupListItemId"] = f_SelectedGroupListItemId;
				values["SelectedGroupItemId"] = f_SelectedGroupItemId;

				foreach (KeyValuePair<string, int?> kv in values) {
					IQueryable<State> match = dbContext.GetState(kv.Key);

					if (kv.Value != null) {
						int val = (int)kv.Value;
						if (match.Any()) {
							dbContext.UpdateState(match.First(), val);
						}
						else {
							dbContext.CreateState(kv.Key, val);
						}
					}
					else {
						if (match.Any()) {
							dbContext.DeleteState(match.First());
						}
					}
				}

				dbContext.Save();
			}
		}
	}
}
