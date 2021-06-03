using SwPrpUtilWpfTest.Infrastructure;
using SwPrpUtilWpfTest.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SwPrpUtilWpfTest.ViewModels
{
	internal class MainWindowViewModel : ObservableObject
	{
		#region Title

		private string _Title = "Property Utils";

		public string  Title
		{
			get => _Title;
			set => Set(ref _Title, value);
		}

		#endregion

		private TestModel _testModel;



	}
}
