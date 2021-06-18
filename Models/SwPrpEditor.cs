using SolidWorks.Interop.sldworks;
using SolidWorks.Interop.swconst;
using SwPrpUtil.Infrastructure;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace SwPrpUtil.Models
{
	internal class SwPrpEditor : ObservableObject
	{
		private List<SwFileItem> _importedFiles;
		public List<SwFileItem> ImportedFiles { get => _importedFiles; }

		private List<SwFileItem> _sourceFiles = new List<SwFileItem>();
		public List<SwFileItem> SourceFiles { get => _sourceFiles; }

		private List<SwProperty> _importedProperties;
		public List<SwProperty> ImportedProperties { get => _importedProperties; }

		private string _statusMessage = "Ready";

		/// <summary>
		/// Status of state SwPrpEditor
		/// </summary>
		public string StatusMessage
		{
			get => _statusMessage;
			private set => Set(ref _statusMessage, value);
		}

		public SwPrpEditor()
		{
			_importedProperties = new List<SwProperty>();
			_importedFiles = new List<SwFileItem>();
		}

		public bool AddFolder(string pathToFolder)
		{
			if (!Directory.Exists(pathToFolder))
				throw new DirectoryNotFoundException(pathToFolder);

			string[] files = Directory.GetFiles(pathToFolder);
			if (files.Length == 0)
			{
				Debug.WriteLine(string.Format("Directory {0} has not files", pathToFolder));
				return false;
			}

			AddFiles(files);

			return true;
		}

		public void AddFiles(string[] pathes)
		{
			if (pathes == null || pathes.Count() == 0)
				throw new ArgumentException(nameof(pathes));

			foreach (string path in pathes)
			{
				if (string.IsNullOrEmpty(path) || path.Contains(@"~$")) continue;

				string extension = Path.GetExtension(path)?.ToLower();
				if (extension == ".sldprt" || extension == ".sldasm" || extension == ".slddrw")
				{
					SwFileItem fileItem = new SwFileItem();
					fileItem.FilePath = path;
					_importedFiles.Add(fileItem);
				}
				else
				{
					Debug.WriteLine(string.Format("Ignoring file {0}", path));
				}
			}

			OnPropertyChanged(nameof(ImportedFiles));
		}


		public async Task<bool> ImportFileProperties(string pathToFile)
		{

			if (string.IsNullOrEmpty(pathToFile) || !File.Exists(pathToFile))
				throw new ArgumentException(nameof(pathToFile));

			///Start or get solidworks process

			StatusMessage = "Run SolidWorks";
			SldWorks swApp;
			try
			{
				swApp = await SwHolder.Instance.GetSwAppAsync();
			}
			catch (Exception e)
			{
				StatusMessage = string.Format("Cautch error esception {0}", e.Message);
				return false;
			}
			StatusMessage = "Solidworks Started";

			_ = swApp.SetCurrentWorkingDirectory(Path.GetDirectoryName(pathToFile));

			#region Open_Document

			int Error = 0; // file load error code
			int Warning = 0; // file load warning code

			ModelDoc2 doc;

			doc = swApp.GetOpenDocumentByName(pathToFile); // check if file is open, return ModelDoc2 or null

			if (doc == null)
			{
				StatusMessage = string.Format("Openning... {0}", Path.GetFileName(pathToFile));

				//Open document
				doc = swApp.OpenDoc6(pathToFile,
										(int)SwHelperFunction.GetSwDocTypeIdFromExtension(pathToFile),
										(int)(swOpenDocOptions_e.swOpenDocOptions_Silent | swOpenDocOptions_e.swOpenDocOptions_ReadOnly),
										"",
										ref Error,
										ref Warning);
			}

			if (Error != 0)
				throw new FileLoadException(string.Format("Open file error code {0:X}", Error));

			if (Warning != 0)
				Debug.WriteLine(string.Format("Open file has warning, code: {0:X}", Warning));

			StatusMessage = "Rebuild document";
			doc.Rebuild((int)swRebuildOptions_e.swRebuildAll);

			#endregion Open_Document


			try
			{
				_sourceFiles.Add(new SwFileItem(doc));
			}
			catch (Exception e)
			{
				StatusMessage = string.Format("Cautch error when create fileitem. Cause: {0}", e.Message);
				return false;
			}
			finally
			{
				doc.Quit();
			}


			OnPropertyChanged(nameof(SourceFiles));

			return true;
		}


		public void RunProcess()
		{
			// write properties from _swSourceProperties to each SwFileItem in _swFileItems

			throw new NotImplementedException();
		}

		public void ClearFileList()
		{
			_importedFiles.Clear();
			OnPropertyChanged(nameof(ImportedFiles));
		}

		public void ClearPropertyList()
		{
			_importedProperties.Clear();
			OnPropertyChanged(nameof(ImportedProperties));
		}
	}
}