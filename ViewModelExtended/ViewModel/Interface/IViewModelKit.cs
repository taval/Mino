using System;
using System.Collections.Generic;
using System.Text;
using ViewModelExtended.Model;



namespace ViewModelExtended.ViewModel
{
	/// <summary>
	/// contains resources for building / managing / controlling ViewModels
	/// </summary>
	public interface IViewModelKit
	{
		public IDbListHelper DbListHelper { get; }
		public IDbQueryHelper DbQueryHelper { get; }
		public IViewModelCreator ViewModelCreator { get; }
		public ICommandBuilder CommandBuilder { get; }

		public IDbContext CreateDbContext ();
	}
}
