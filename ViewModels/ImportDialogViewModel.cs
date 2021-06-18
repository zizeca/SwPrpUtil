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
			///*
			//test code
			SwFileItem item = new SwFileItem();
			SwFileItem item2 = new SwFileItem();

			item.FilePath = @"D:\1.Сurrent_work\vinnik\0712-01-01-001.sldprt";
			item2.FilePath = @"D:\1.Сurrent_work\vinnik\0712-01-01-002.sldprt";

			SwProperty prp = new SwProperty();
			prp.PropertyName = "property";
			prp.Expression = "expression";
			prp.TypePrp = SolidWorks.Interop.swconst.swCustomInfoType_e.swCustomInfoText;

			SwProperty prp1 = new SwProperty();
			prp1.PropertyName = "property1";
			prp1.Expression = "expression1";
			prp1.TypePrp = SolidWorks.Interop.swconst.swCustomInfoType_e.swCustomInfoText;


			item.MainProperty = new SwFileConfiguration("Main properties", new List<SwProperty>() { prp, prp1, prp });
			item2.MainProperty = new SwFileConfiguration("Main properties", new List<SwProperty>() { prp, prp1, prp });

			SwFileConfiguration cnf1 = new SwFileConfiguration("Config 1", new List<SwProperty>() { prp, prp1, prp });
			SwFileConfiguration cnf2 = new SwFileConfiguration("Config 2", new List<SwProperty>() { prp, prp1, prp });
			SwFileConfiguration cnf3 = new SwFileConfiguration("Config 3", new List<SwProperty>() { prp, prp1, prp });

			item.SwFileConfigurations = new List<SwFileConfiguration>() { cnf1, cnf2, cnf3 };
			item2.SwFileConfigurations = new List<SwFileConfiguration>() { cnf1, cnf2, cnf3 };
			_fileItems = new List<SwFileItem>() { item, item2 };
			//*/
		}

		public ICommand ImoprtPorperties { get; set; }

		public async Task OnImoprtPorpertiesExecuted()
		{
			OpenFileDialog dialog = new OpenFileDialog();
			dialog.ShowDialog();
			string path = dialog.FileName;

			_ = await _editor.ImportFileProperties(path);
			FileItems = _editor.SourceFiles;
		}
	}
}