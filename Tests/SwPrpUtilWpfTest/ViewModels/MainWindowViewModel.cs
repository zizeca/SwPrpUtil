using SwPrpUtilWpfTest.Infrastructure;
using SwPrpUtilWpfTest.Infrastructure.Commands;
using SwPrpUtilWpfTest.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace SwPrpUtilWpfTest.ViewModels
{
	internal class MainWindowViewModel : ObservableObject
	{
		#region Title

		private string _Title = "Property Utils";

		public MainWindowViewModel()
		{
			ShowCommand = new RelayCommand( ShowMethod );
		}

		public string  Title
		{
			get => _Title;
			set => Set(ref _Title, value);
		}

		#endregion

		private bool _IsEnabled = true;
		public bool IsEnable {
			get => _IsEnabled; 
			set 
			{
				Set(ref _IsEnabled, value);
			} 
		}


		public ICommand ShowCommand { get; private set; }

		void ShowMethod(object param)
		{
			PopUpWindow w = new PopUpWindow();
			w.ShowDialog();
		}


	}
}
