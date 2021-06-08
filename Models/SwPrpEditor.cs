using SolidWorks.Interop.sldworks;
using SolidWorks.Interop.swconst;
using SwPrpUtil.Infrastructure;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.ServiceModel.Configuration;
using System.Threading.Tasks;

namespace SwPrpUtil.Models
{
	internal class SwPrpEditor : ObservableObject
	{
		private List<SwFileItem> _swFileItems;
		private List<SwProperty> _swSourceProperties;

		private SldWorks _swApp;

		private string _statusMessage;
		public string StatusMessage { get => _statusMessage; set => Set(ref _statusMessage, value); }

		public SwPrpEditor()
		{
		}

		private async Task runSldWorks()
		{
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

			foreach (string fileName in files)
			{
				if (!AddFile(fileName))
					Debug.WriteLine(string.Format("Ignoring file {0}", fileName));
			}

			return true;
		}

		public bool AddFile(string pathToFile)
		{
			//Create SwFileItem and add to _swFileItems/
			// get the file attributes for file or directory
			if (!File.Exists(pathToFile))
				throw new FileNotFoundException(pathToFile);

			string extension = Path.GetExtension(pathToFile)?.ToLower();
			if (extension == ".sldprt" || extension == ".sldasm" || extension == ".slddrw")
			{
				SwFileItem fileItem = new SwFileItem();
				fileItem.FilePath = pathToFile;
				_swFileItems.Add(fileItem);
				return true;
			}
			else
			{
				return false;
			}
		}

		public async Task<bool> ImportPropertiesFromFile(string pathToFile, string configName = "")
		{
			//Run solidworks ( or get exist solidworks object)
			//Open file in solidworks
			//open a new window with the dialog - from where to import properties (main properties or configuration properties)
			//read selected properties
			//add properties to _swSourceProperties

			if (string.IsNullOrEmpty(pathToFile) || !File.Exists(pathToFile))
				throw new ArgumentException(nameof(pathToFile));

			StatusMessage = "Run SolidWorks";
			try
			{
				_swApp = await SwHolder.Instance.GetSwAppAsync();
			}
			catch (Exception e)
			{
				StatusMessage = string.Format("Cautch error esception {0}", e.Message);
				throw e;
			}
			StatusMessage = "Solidworks Started";

			_swApp.SetCurrentWorkingDirectory(Path.GetDirectoryName(pathToFile));

			#region Open_Document

			int Error = 0;
			int Warning = 0;

			ModelDoc2 doc = _swApp.OpenDoc6(pathToFile,
											(int)SwHelperFunction.GetTypeIdFromExtension(pathToFile),
											(int)(swOpenDocOptions_e.swOpenDocOptions_Silent | swOpenDocOptions_e.swOpenDocOptions_ReadOnly),
											configName,
											ref Error,
											ref Warning);

			#endregion Open_Document

			throw new NotImplementedException();
		}

		public void RunProcess()
		{
			// write properties from _swSourceProperties to each SwFileItem in _swFileItems

			throw new NotImplementedException();
		}

		public void ClearFileList()
		{
			_swFileItems.Clear();
		}

		public void ClearPropertyList()
		{
			_swSourceProperties.Clear();
		}
	}
}