using SwPrpUtil.ViewModels.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SwPrpUtil.ViewModels
{
	internal class MainWindowViewModel : ViewModelBase
	{
		#region Title

		private string _Title = "Property Utils";

		public string  Title
		{
			get => _Title;
			set => Set(ref _Title, value);
		}

		#endregion

	}
}
