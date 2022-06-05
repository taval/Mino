using System;
using System.Collections.Generic;
using System.Text;
using ViewModelExtended.Model;



namespace ViewModelExtended.ViewModel
{
	public class ViewModelKit : IViewModelKit
	{
		public IDbListHelper DbListHelper { get; private set; }
		public IDbQueryHelper DbQueryHelper { get; private set; }
		public IViewModelCreator ViewModelCreator { get; private set; }
		public ICommandBuilder CommandBuilder { get; private set; }

		public ViewModelKit ()
		{
			// TODO: Testing only: truncating the table and resetting id auto-increment - remove this in production
			using (IDbContext dbContext = CreateDbContext()) {
				dbContext.Reset();
			}

			DbListHelper = new DbListHelper();
			CommandBuilder = new CommandBuilder();
			ViewModelCreator = new ViewModelCreator(this);
			DbQueryHelper = new DbQueryHelper(ViewModelCreator);
		}

		public IDbContext CreateDbContext ()
		{
			return new DbContext();
		}
	}
}
