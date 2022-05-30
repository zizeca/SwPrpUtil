﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace SwPrpUtil.Views.Windows
{
	/// <summary>
	/// Interaction logic for ImportDialog.xaml
	/// </summary>
	public partial class ImportDialog : Window
	{
		public ImportDialog()
		{
			InitializeComponent();
		}

		//crutch programming
		private void TreeView_SelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
		{
			SelectedItemHelper.Content = e.NewValue;
		}
	}
}
