using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace SwPrpUtil.Models
{
	internal class SwFileItem
	{
		#region Path_properies

		private string _filePath;

		public string Extension { get; private set; }

		public string FileName { get; private set; }

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

		public SwFileSummaryInfo SummaryInfo;

		// Main custom property
		public SwProperty MainProperty;

		//Configurations with self properties
		public List<SwFileConfiguration> swFileConfigurations;
	}
}