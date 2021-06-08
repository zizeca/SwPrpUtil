using SwPrpUtil.Models;
using SwPrpUtil.ViewModels.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using SwPrpUtil.Infrastructure;
using System.ComponentModel;
using System.Diagnostics;
using SwPrpUtil.Infrastructure.Commands;
using SwPrpUtil.Views.Windows;

namespace SwPrpUtil.ViewModels
{
	internal class MainWindowViewModel : ObservableObject
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
			try
			{
				Application.Current.MainWindow.Closing += MainWindow_Closing;
			}
			catch(Exception e)
			{
				Debug.WriteLine(string.Format("Catch exception {0}", e.Message));
			}

			OpenImportDialog = new RelayCommand(ShowImportDialog);

		}


		public ICommand OpenImportDialog { get; set; }

		private void MainWindow_Closing(object sender, CancelEventArgs e)
		{
			SwHolder.DisposeInstance();
			e.Cancel = false;
		}

		private void ShowImportDialog(object parameter)
		{
			ImportDialog dialog = new ImportDialog();
			dialog.ShowDialog();
		}

	}
}