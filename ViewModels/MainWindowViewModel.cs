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
		#region Properties_for_view

		#region Title

		private string _Title = "Property Utils";

		public string Title
		{
			get => _Title;
			set => Set(ref _Title, value);
		}

		#endregion Title

		//private string _statusText;

		#region StatusBarText

		private string _statusText = "Ready";

		public string StatusText
		{
			get => _statusText;
			set => Set(ref _statusText, value);
		}

		#endregion StatusBarText

		#region List_properties

		private List<SwProperty> _sourceProperties;

		public List<SwProperty> SourceProperties
		{ get => _sourceProperties; set => Set(ref _sourceProperties, value); }

		#endregion List_properties

		#endregion Properties_for_view

		// class for manipulated with sollidworks files
		private SwPrpEditor _editor = null;

		// ctor
		public MainWindowViewModel()
		{
			#region Closing_Event_handle

			// Closing event
			try
			{
				Application.Current.MainWindow.Closing += MainWindow_Closing;
			}
			catch (Exception e)
			{
				Debug.WriteLine(string.Format("Catch exception {0}", e.Message));
			}

			#endregion Closing_Event_handle

			#region Command_relay

			OpenImportDialog = new AsyncRelayCommand(ShowImportDialog);

			#endregion Command_relay

			_editor = new SwPrpEditor();

			_editor.PropertyChanged += (s, e) =>
			{
				if (e.PropertyName == "StatusMessage")
				{
					StatusText = _editor.StatusMessage;
				}
			};

			_editor.PropertyChanged += (s, e) =>
			{
				if (e.PropertyName == "ImportedProperties")
				{
					SourceProperties = _editor.ImportedProperties;
				}
			};
		}

		#region Command_and_command_actions

		public ICommand OpenImportDialog { get; set; }

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

		#endregion Command_and_command_actions

		private void MainWindow_Closing(object sender, CancelEventArgs e)
		{
			SwHolder.DisposeInstance();
			e.Cancel = false;
		}
	}
}