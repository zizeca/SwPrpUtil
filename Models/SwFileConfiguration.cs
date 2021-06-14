using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SwPrpUtil.Models
{
	internal class SwFileConfiguration
	{
		public SwFileConfiguration BaseFileConfiguration;

		public string ConfigurationName;

		public List<SwProperty> Properties;

		public SwFileConfiguration(string configurationName, List<SwProperty> properties, SwFileConfiguration baseFileConfiguration = null)
		{
			BaseFileConfiguration = baseFileConfiguration;
			ConfigurationName = configurationName;
			Properties = properties;
		}

		public SwFileConfiguration()
		{
			BaseFileConfiguration = null;
			ConfigurationName = "";
			Properties = new List<SwProperty>();
		}
	}
}