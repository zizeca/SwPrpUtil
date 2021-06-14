using SwPrpUtil.Infrastructure;
using SwPrpUtil.Infrastructure.Commands;
using SwPrpUtil.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Input;

namespace SwPrpUtil.ViewModels
{
	internal class ImportDialogViewModel : ObservableObject
	{
		#region StatusBarText

		private string _statusText = "Ready";

		public string StatusText
		{
			get => _statusText;
			set => Set(ref _statusText, value);
		}

		#endregion StatusBarText

		private List<SwFileItem> _fileItems;

		public List<SwFileItem> FileItems { get => _fileItems; set => Set(ref _fileItems, value); }

		private SwPrpEditor _editor;

		public ImportDialogViewModel()
		{
			_editor = new SwPrpEditor();
			ImoprtPorperties = new AsyncRelayCommand(OnImoprtPorpertiesExecuted);
			_editor.PropertyChanged += (s, e) =>
			{
				switch (e.PropertyName)
				{
					case nameof(_editor.StatusMessage):
						StatusText = _editor.StatusMessage;
						break;

					case nameof(_editor.ImportedFiles):
						OnPropertyChanged(nameof(FileItems));
						break;
				}
			};
		}

		public ICommand ImoprtPorperties { get; set; }

		public async Task OnImoprtPorpertiesExecuted()
		{
			OpenFileDialog dialog = new OpenFileDialog();
			dialog.ShowDialog();
			string path = dialog.FileName;

			_ = await _editor.ImportSourceFile(path);
			FileItems = _editor.SourceFiles;
		}
	}
}