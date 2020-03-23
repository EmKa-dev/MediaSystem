﻿using System;
using System.Windows.Input;

namespace MediaSystem.DesktopClientWPF.Commands
{
    public class RelayCommand : ICommand
    {
        private Action methodToExecuteNoParam;
        private Action<object> methodToExecuteWithParam;
        private Func<object, bool> canExecute;

        public event EventHandler CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }

        public RelayCommand(Action<object> execute, Func<object, bool> canExecute = null)
        {
            this.methodToExecuteWithParam = execute;
            this.canExecute = canExecute;
        }

        /// <summary>
        /// To allow for anonymous methods without arguments
        /// </summary>
        /// <param name="method"></param>
        public RelayCommand(Action method, Func<object, bool> canExecute = null)
        {
            methodToExecuteNoParam = method;
            this.canExecute = canExecute;
        }

        public bool CanExecute(object parameter)
        {
            if (canExecute == null)
            {
                return true;
            }

            return this.canExecute(parameter);
        }

        public void Execute(object parameter)
        {

            if (methodToExecuteNoParam == null)
            {
                this.methodToExecuteWithParam(parameter);
            }
            else
            {
                this.methodToExecuteNoParam();
            }
        }
    }
}
