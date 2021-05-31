﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace SwPrpUtil.Infrastructure.Commands.Base
{
	public interface IAsyncCommand : ICommand
	{
		Task ExecuteAsync();
		bool CanExecute();
	}

	public interface IAsyncCommand<T> : ICommand
	{
		Task ExecuteAsync(T parameter);
		bool CanExecute(T parameter);
	}
}
