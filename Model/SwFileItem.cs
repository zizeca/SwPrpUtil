using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace SwPrpUtil.Model
{
	internal class SwFileItem
	{
		#region Path_properies

		private string _filePath;
		private string _extension;
		private string _filename;

		public string Extension
		{
			get => _extension;
			private set => _extension = value;
		}

		public string FileName
		{
			get => _filename;
			private set => _filename = value;
		}

		public string FilePath
		{
			get => _filePath;
			set
			{
				_filePath = value;
				Extension = Path.GetExtension(_filePath);
				FileName = Path.GetFileName(_filePath);
			}
		}

		#endregion Path_properies

		// Main custom property
		public SwProperty MainProperty;

		//Configurations with self properties
		public List<SwFileConfiguration> swFileConfigurations;
	}
}