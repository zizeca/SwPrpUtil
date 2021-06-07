using SwPrpUtil.Models;
using SwPrpUtil.ViewModels.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace SwPrpUtil.ViewModels
{
	internal class MainWindowViewModel : ViewModelBase, IDisposable
	{
		#region Title

		private string _Title = "Property Utils";

		public string Title
		{
			get => _Title;
			set => Set(ref _Title, value);
		}

		#endregion Title

		public MainWindowViewModel()
		{
			Application.Current.MainWindow.Closing += (o, e) => { SwHolder.DisposeInstance(); };
		}
	}
}