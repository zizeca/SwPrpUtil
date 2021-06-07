using SwPrpUtilWpfTest.Infrastructure.Commands.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SwPrpUtilWpfTest.Infrastructure.Commands
{
	class RelayCommand : CommandBase
	{
		private readonly Action<object> execute;
		private readonly Func<object, bool> canExecute;

		public RelayCommand(Action<object> Execute, Func<object, bool> CanExecute = null)
		{
			execute = Execute ?? throw new ArgumentNullException(nameof(Execute));
			canExecute = CanExecute;
		}

		public override bool CanExecute(object parameter)
		{
			return canExecute?.Invoke(parameter) ?? true;
		}

		public override void Execute(object parameter) => execute(parameter);
	}
}
