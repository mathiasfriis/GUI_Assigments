using MvvmFoundation.Wpf;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace Mathias_Loenborg_Friis_201505665.Models
{
    public class TransactionList : ObservableCollection<Transaction>, INotifyPropertyChanged
    {
        public List<Transaction> Transactions = new List<Transaction>();

        public TransactionList()
        {
            if ((bool)(DesignerProperties.IsInDesignModeProperty.GetMetadata(typeof(DependencyObject)).DefaultValue))
            {
                AddTransaction();
            }
            AddTransaction();
        }

        #region Properties

        int currentIndex = -1;

        public int CurrentIndex
        {
            get { return currentIndex; }
            set
            {
                if (currentIndex != value)
                {
                    currentIndex = value;
                    NotifyPropertyChanged();
                }
            }
        }

        Transaction currentTransaction = null;

        public Transaction CurrentTransaction
        {
            get { return currentTransaction; }
            set
            {
                if (currentTransaction != value)
                {
                    currentTransaction = value;
                    NotifyPropertyChanged();
                }
            }
        }

        #endregion

        #region Commands

        #region NewTransactionCommand
        ICommand _newTransactionCommand;
        public ICommand NewTransactionCommand
        {
            get
            {
                return _newTransactionCommand ?? (_newTransactionCommand = new RelayCommand(
                    () => AddTransaction(),
                    () => true
                    ));
            }
        }

        private void AddTransaction()
        {
            MessageBox.Show("Adding new Transaction");
            Transactions.Add(new Transaction());
            NotifyPropertyChanged("Count");
            CurrentIndex = Count - 1;
        }
        #endregion

        #region DeleteTransactionCommand
        ICommand _deleteTransactionCommand;
        public ICommand DeleteTransactionCommand
        {
            get { return _deleteTransactionCommand ?? (_deleteTransactionCommand = new RelayCommand(DeleteTransaction, DeleteTransaction_CanExecute)); }
        }

        private void DeleteTransaction()
        {
            RemoveAt(CurrentIndex);
            NotifyPropertyChanged("Count");
        }

        private bool DeleteTransaction_CanExecute()
        {
            if (Count > 0 && CurrentIndex >= 0)
                return true;
            else
                return false;
        }
        #endregion

        #endregion

        #region INotifyPropertyChanged implementation

        public new event PropertyChangedEventHandler PropertyChanged;

        private void NotifyPropertyChanged([CallerMemberName] string propertyName = null)
        {
            var handler = PropertyChanged;
            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(propertyName));
            }
        }
        #endregion
    }
}
