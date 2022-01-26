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
using System.Threading;
using System.Windows.Forms;

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

		#region Files_for_export_panel

		private List<SwFileItem> _swFileItems;

		public List<SwFileItem> SwFileItems
		{
			get => _swFileItems;
			set => Set(ref _swFileItems, value);
		}

		#endregion Files_for_export_panel

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
				System.Windows.Application.Current.MainWindow.Closing += MainWindow_Closing;
			}
			catch (Exception e)
			{
				Debug.WriteLine(string.Format("Catch exception {0}", e.Message));
			}

			#endregion Closing_Event_handle

			#region Command_relay

			//OpenImportDialog = new AsyncRelayCommand(ShowImportDialog);
			OpenImportDialog = new RelayCommand(OnOpenImportDialogExecuted);

			AddFolder = new RelayCommand(OnAddFolderExecuted);

			#endregion Command_relay

			_sourceProperties = new List<SwProperty>();
			_swFileItems = new List<SwFileItem>();

			_editor = new SwPrpEditor();

			_editor.PropertyChanged += (s, e) =>
			{
				switch (e.PropertyName)
				{
					case nameof(_editor.StatusMessage):
						StatusText = _editor.StatusMessage;
						break;

					case nameof(_editor.TargetProperties):
						SourceProperties = _editor.TargetProperties;
						break;

					case nameof(_editor.TargetFiles):
						SwFileItems = _editor.TargetFiles;
						break;
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

			_ = await _editor.ImportFileProperties(path);
			OnPropertyChanged(nameof(StatusText));
		}

		public void OnOpenImportDialogExecuted(object param)
		{
			ImportDialog w = new ImportDialog();
			w.ShowDialog();
		}

		public ICommand AddFolder { get; set; }

		private void OnAddFolderExecuted(object param)
		{
			FolderBrowserDialog dialog = new FolderBrowserDialog
			{
				ShowNewFolderButton = false
			};

			if (dialog.ShowDialog() == DialogResult.OK)
			{
				_ = _editor.AddFolder(dialog.SelectedPath);
			}
		}

		#endregion Command_and_command_actions

		private void MainWindow_Closing(object sender, CancelEventArgs e)
		{
			SwHolder.DisposeInstance();
			e.Cancel = false;
		}
	}
}