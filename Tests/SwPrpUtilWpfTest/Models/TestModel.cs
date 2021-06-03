using SwPrpUtilWpfTest.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SwPrpUtilWpfTest.Models
{
	class TestModel : ObservableObject
	{
		private string _name;
		public string Name
		{
			get => _name; 
			set => Set(ref _name, value); 
		}



	}
}
