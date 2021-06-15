using System.Collections.Generic;
using System.IO;

namespace SwPrpUtil.Models
{
	internal class SwFileItem
	{
		#region Path_properies

		private string _filePath;

		public string Extension { get; private set; }

		public string FileName { get; private set; }

		public string FileNameWithoutExtension { get; private set; }

		public string FilePath
		{
			get => _filePath;
			set
			{
				_filePath = value;
				Extension = Path.GetExtension(_filePath);
				FileName = Path.GetFileName(_filePath);
				FileNameWithoutExtension = Path.GetFileNameWithoutExtension(_filePath);
			}
		}

		#endregion Path_properies

		public SwFileSummaryInfo SummaryInfo { get; set; }

		// Main custom property
		public List<SwProperty> MainProperty { get; set; }

		//Configurations with self properties
		public List<SwFileConfiguration> swFileConfigurations { get; set; }

		public SwFileItem()
		{
			FilePath = string.Empty;
			SummaryInfo = new SwFileSummaryInfo();
			MainProperty = new List<SwProperty>();
			swFileConfigurations = new List<SwFileConfiguration>();
		}
	}
}