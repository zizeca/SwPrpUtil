using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SwPrpUtil.Models
{
	internal class SwCustomProperty
	{
		//public SwFileConfiguration BaseFileConfiguration { get; set; }

		//public List<SwFileConfiguration> Children { get; set; }

		public string ConfigurationName { get; set; }

		public List<SwProperty> Properties { get; set; }

		public SwCustomProperty(string configurationName, List<SwProperty> properties)
		{
			ConfigurationName = configurationName;
			Properties = properties;
		}

		public SwCustomProperty()
		{
		}
	}
}