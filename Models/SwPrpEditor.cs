using SolidWorks.Interop.sldworks;
using SolidWorks.Interop.swconst;
using SwPrpUtil.Infrastructure;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;

namespace SwPrpUtil.Models
{
	internal class SwPrpEditor : ObservableObject
	{
		private List<SwFileItem> _swFileItems;
		private List<SwProperty> _swSourceProperties;

		private string _statusMessage = "Ready";

		public string StatusMessage
		{
			get => _statusMessage;
			set
			{
				Set(ref _statusMessage, value);
			}
		}

		private void RaiseChange(object o)
		{
			OnPropertyChanged((string)o);
		}

		public SwPrpEditor()
		{
			_swSourceProperties = new List<SwProperty>();
			_swFileItems = new List<SwFileItem>();
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
			//read configName properties
			//add properties to _swSourceProperties

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
										configName,
										ref Error,
										ref Warning);
			}

			if (Error != 0)
				throw new FileLoadException(string.Format("Open file error code {0:X}", Error));

			if (Warning != 0)
				Debug.WriteLine(string.Format("Open file has warning, code: {0:X}", Warning));

			doc.Rebuild((int)swRebuildOptions_e.swRebuildAll);

			#endregion Open_Document

			CustomPropertyManager manager = doc.Extension.CustomPropertyManager[configName];

			object PropNames = null;
			object PropTypes = null;
			object PropValues = null;
			object Resolved = null;
			object PropLink = null;
			int PropCount = 0;

			PropCount = manager.GetAll3(ref PropNames, ref PropTypes, ref PropValues, ref Resolved, ref PropLink);

			try
			{
				StatusMessage = string.Format("Found {0} properties", PropCount);
				for (int i = 0; i < PropCount; i++)
				{
					SwProperty prp = new SwProperty
					{
						PropertyName = ((string[])PropNames)[i],
						TypePrp = (swCustomInfoType_e)((int[])PropTypes)[i],
						Expression = ((string[])PropValues)[i]
					};
					Debug.WriteLine(string.Format("Add prp: {0}|{1}|{2}", prp.PropertyName, prp.TypePrp.ToString(), prp.Expression));
					_swSourceProperties.Add(prp);
				}
			}
			catch (Exception e)
			{
				StatusMessage = string.Format("Catch exception {0}", e.Message);
			}

			doc.Quit();
			return true;
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