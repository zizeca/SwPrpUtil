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

        private readonly SwPrpEditor _editor;

		//ctor
		public ImportDialogViewModel()
		{
			_editor = new SwPrpEditor();
			OpenFiles = new AsyncRelayCommand(OnOpenFilesExecuted);
			_editor.PropertyChanged += (s, e) =>
			{
				switch (e.PropertyName)
				{
					case nameof(_editor.StatusMessage):
						StatusText = _editor.StatusMessage;
						break;

					case nameof(_editor.TargetFiles):
						OnPropertyChanged(nameof(FileItems));
						break;
				}
			};

            #region Test_code
            ///*
            // tempolary test code
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

			SwCustomProperty property = new SwCustomProperty("Main properties", new List<SwProperty>() { prp, prp1, prp });

			item.FileProperties = new SwFileProperty();
			item2.FileProperties = new SwFileProperty();

			SwCustomProperty cnf1 = new SwCustomProperty("Config 1", new List<SwProperty>() { prp, prp1, prp });
			SwCustomProperty cnf2 = new SwCustomProperty("Config 2", new List<SwProperty>() { prp, prp1, prp });
			SwCustomProperty cnf3 = new SwCustomProperty("Config 3", new List<SwProperty>() { prp, prp1, prp });

			item.FileProperties.SwFileConfigurations = new List<SwCustomProperty>() { cnf1, cnf2, cnf3 };
			item.FileProperties.MainProperty = property;
			item2.FileProperties.SwFileConfigurations = new List<SwCustomProperty>() { cnf1, cnf2, cnf3 };
			item2.FileProperties.MainProperty = property;

			_fileItems = new List<SwFileItem>() { item, item2 };
			//*/

			#endregion Test_code

		}

		public ICommand OpenFiles { get; set; }

		public async Task OnOpenFilesExecuted()
		{
			OpenFileDialog dialog = new OpenFileDialog();
			dialog.ShowDialog();
			string path = dialog.FileName;

			_ = await _editor.ReadFileProperties(path);
			FileItems = _editor.SourceFiles;
		}

		//tree view seletcted item, try cast to SwProperty
		public object SelectedObject
		{
			get { return _selected_object; }
			set
			{
				Set(ref _selected_object, value);
				Console.WriteLine(SelectedObject);
			}
		}
		object _selected_object;

        private RelayCommand importPorperties;

        public ICommand ImportPorperties
        {
            get
            {
                if (importPorperties == null)
                {
                    importPorperties = new RelayCommand(OnImportPorpertiesExecute, CanImportProperiesExecute);
                }

                return importPorperties;
            }
        }

        private void OnImportPorpertiesExecute(object commandParameter)
        {
			
        }

		private bool CanImportProperiesExecute(object commandParameter)
        {
			if(_selected_object is SwCustomProperty)
				return true;
			return false;
        }


	}
}