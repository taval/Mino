using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;
using Mino.Model;



namespace Mino.ViewModel
{
	public class ViewModelKit : IViewModelKit
	{
		public IDbListHelper DbListHelper { get; private set; }
		public IDbQueryHelper DbQueryHelper { get; private set; }
		public IViewModelCreator ViewModelCreator { get; private set; }
		public ICommandBuilder CommandBuilder { get; private set; }

		private readonly DbContextOptions<MinoDbContext> f_Options;

		public ViewModelKit (DbContextOptions<MinoDbContext> options)
		{
			f_Options = options;

			// TODO: Testing only: truncating the table and resetting id auto-increment - remove this in production
			using (IDbContext dbContext = CreateDbContext()) {
				//dbContext.Reset();
			}

			DbListHelper = new DbListHelper();
			CommandBuilder = new CommandBuilder();
			ViewModelCreator = new ViewModelCreator(this);
			DbQueryHelper = new DbQueryHelper(ViewModelCreator);
		}

		public IDbContext CreateDbContext ()
		{
			return new MinoDbContext(f_Options);
		}
	}
}
