using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace SwPrpUtil.Infrastructure.Commands.Base
{
	public abstract class AsyncCommandBase : ICommand
	{
		private bool _isExecutting;
		private readonly Action<Exception> _onExeption;

		public bool IsExecuting 
		{	
			get => _isExecutting;
			set
			{
				_isExecutting = value;
				CanExecuteChanged?.Invoke(this, new EventArgs());
			} 
		}

		public event EventHandler CanExecuteChanged;

		public AsyncCommandBase(Action<Exception> onExeption)
		{
			_onExeption = onExeption;
		}

		public bool CanExecute(object parameter)
		{
			return !IsExecuting;
		}

		public async void Execute(object parameter)
		{
			IsExecuting = true;
			try
			{
				await ExecuteAsunc(parameter);
			}
			catch (Exception ex)
			{
				_onExeption?.Invoke(ex);
			} 

			IsExecuting = false;
		}

		public abstract Task ExecuteAsunc(object parameter);
	}
}
