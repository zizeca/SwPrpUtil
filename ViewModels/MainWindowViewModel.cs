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
using Microsoft.Win32;
using System.Threading;

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

		private SwPrpEditor _editor = null;

		//private string _statusText;

		private string _statusText = "Ready";

		public string StatusText
		{
			get => _statusText;
			set => Set(ref _statusText, value);
		}

		public MainWindowViewModel()
		{
			//*
			try
			{
				Application.Current.MainWindow.Closing += MainWindow_Closing;
			}
			catch (Exception e)
			{
				Debug.WriteLine(string.Format("Catch exception {0}", e.Message));
			}
			//*/
			OpenImportDialog = new AsyncRelayCommand(ShowImportDialog);

			_editor = new SwPrpEditor();

			_editor.PropertyChanged += (s, e) =>
			{
				if (e.PropertyName == "StatusMessage")
				{
					StatusText = _editor.StatusMessage;
				}
			};
		}

		private void prpchg(object sender, PropertyChangedEventArgs e)
		{
			Debug.WriteLine(string.Format("invoke {0}", e.PropertyName));
			if (e.PropertyName == "StatusMessage")
			{
				this.OnPropertyChanged(nameof(StatusText));
			}
		}

		public ICommand OpenImportDialog { get; set; }

		private void MainWindow_Closing(object sender, CancelEventArgs e)
		{
			SwHolder.DisposeInstance();
			e.Cancel = false;
		}

		private async Task ShowImportDialog()
		{
			/*
			ImportDialog dialog = new ImportDialog();
			dialog.ShowDialog();
			*/
			Title = "Import";

			OpenFileDialog dialog = new OpenFileDialog();
			dialog.ShowDialog();
			string path = dialog.FileName;

			_ = await _editor.ImportPropertiesFromFile(path);
			OnPropertyChanged(nameof(StatusText));
		}
	}
}