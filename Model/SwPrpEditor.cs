using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.ServiceModel.Configuration;

namespace SwPrpUtil.Model
{
	internal class SwPrpEditor
	{
		private List<SwFileItem> _swFileItems;
		private List<SwProperty> _swSourceProperties;

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

		public bool ImportPropertiesFromFile(string pathToFile, string configName = null)
		{
			//Run solidworks ( or get exist solidworks object)
			//Open file in solidworks
			//open a new window with the dialog - from where to import properties (main properties or configuration properties)
			//read selected properties
			//add properties to _swSourceProperties

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