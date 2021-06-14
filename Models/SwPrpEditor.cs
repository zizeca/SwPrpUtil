using SolidWorks.Interop.sldworks;
using SolidWorks.Interop.swconst;
using SwPrpUtil.Infrastructure;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;

namespace SwPrpUtil.Models
{
	internal class SwPrpEditor : ObservableObject
	{
		private List<SwFileItem> _importedFiles;
		public List<SwFileItem> ImportedFiles { get => _importedFiles; }

		private List<SwProperty> _importedProperties;
		public List<SwProperty> ImportedProperties { get => _importedProperties; }

		private string _statusMessage = "Ready";

		/// <summary>
		/// Status of state SwPrpEditor
		/// </summary>
		public string StatusMessage
		{
			get => _statusMessage;
			private set
			{
				Set(ref _statusMessage, value);
			}
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

			StatusMessage = "Rebuild document";
			doc.Rebuild((int)swRebuildOptions_e.swRebuildAll);

			#endregion Open_Document

			CustomPropertyManager manager = doc.Extension.CustomPropertyManager[configName];

			object PropNames = null;
			object PropTypes = null;
			object PropValues = null;
			object Resolved = null;
			object PropLink = null;
			int PropCount = 0;

			StatusMessage = "Get properties from " + (configName == "" ? "main properties" : configName);
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
					_importedProperties.Add(prp);
				}
			}
			catch (Exception e)
			{
				StatusMessage = "Catch exception" + e.Message;
			}

			doc.Quit();
			OnPropertyChanged("ImportedProperties");
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
		}

		public void ClearPropertyList()
		{
			_importedProperties.Clear();
		}
	}
}