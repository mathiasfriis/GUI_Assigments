using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MvvmFoundation.Wpf;
using System.Windows.Input;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Xml.Serialization;
using System.IO;
using System.Windows.Media;
using System.Windows.Data;
using Microsoft.Win32;
using Agent_Assignments___10;

namespace Agent_Assignments
{
   public class Agents : ObservableCollection<Agent>, INotifyPropertyChanged
    {
        const string AppTitle = "Agent Assignments 4 - Lab 3";
        string filename = "";
        string filePath = "";
        string filter;
        string Title;

        public Agents()
        {
            if ((bool)(DesignerProperties.IsInDesignModeProperty.GetMetadata(typeof(DependencyObject)).DefaultValue))
            {
                Add(new Agent("007", "James Bond", "MI-5 HQ", "Assassination", "UpperVolta"));
                Add(new Agent("003", "Trinity", "The Matrix", "Kicking butt", "BluePill"));
            }
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

        Agent currentAgent = null;

        public Agent CurrentAgent
        {
            get { return currentAgent; }
            set
            {
                if (currentAgent != value)
                {
                    currentAgent = value;
                    NotifyPropertyChanged();
                }
            }
        }

        public IReadOnlyCollection<string> FilterSpecialities
        {
            get
            {
                ObservableCollection<string> result = new ObservableCollection<string>();
                result.Add("All");
                foreach (var s in new Specialities())
                    result.Add(s);
                return result;
            }
        }

        int currentSpecialityIndex = 0;

        public int CurrentSpecialityIndex
        {
            get { return currentSpecialityIndex; }
            set
            {
                if (currentSpecialityIndex != value)
                {
                    ICollectionView cv = CollectionViewSource.GetDefaultView(this);
                    currentSpecialityIndex = value;
                    if (currentSpecialityIndex == 0)
                        cv.Filter = null; // Index 0 is no filter (show all)
                    else
                    {
                        filter = FilterSpecialities.ElementAt(currentSpecialityIndex);
                        cv.Filter = CollectionViewSource_Filter;
                    }
                    NotifyPropertyChanged();
                }
            }
        }

        private bool CollectionViewSource_Filter(object ag)
        {
            Agent agent = ag as Agent;
            return (agent.Speciality == filter);
        }

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

         #region Commands

        ICommand _nextCommand;
        public ICommand NextCommand
        {
            get
            {
                return _nextCommand ?? (_nextCommand = new RelayCommand(
                    () => CurrentIndex++,
                    () => CurrentIndex < (Count-1)
                    ));
            }
        }

        ICommand _previousCommand;
        public ICommand PreviousCommand
        {
            get
            {
                return _previousCommand ?? (_previousCommand = new RelayCommand(
                    () => CurrentIndex--,
                    () => CurrentIndex > 0
                    ));
            }
        }

        ICommand _newCommand;
        public ICommand NewCommand
        {
            get
            {
                return _newCommand ?? (_newCommand = new RelayCommand(
                    () => AddAgent(),
                    () => true
                    ));
            }
        }

        private void AddAgent()
        {
            Add(new Agent("000","New Agent","N/A","N/A","N/A"));
            NotifyPropertyChanged("Count");
            CurrentIndex = Count - 1;
        }


        ICommand _editCommand;
        public ICommand EditCommand
        {
            get { return _editCommand ?? (_editCommand = new RelayCommand(EditAgent, DeleteAgent_CanExecute)); }
        }

        private void EditAgent()
        {
            // Show Modal Dialog
            var dlg = new AgentWindow();
            dlg.Title = "Edit Agent";
            // Need a temp agent in case of cancel
            Agent tempAgent = new Agent();
            tempAgent.ID = CurrentAgent.ID;
            tempAgent.CodeName = currentAgent.CodeName;
            tempAgent.Speciality = currentAgent.Speciality;
            tempAgent.Assignment = currentAgent.Assignment;
            dlg.DataContext = tempAgent;
            if (dlg.ShowDialog() == true)
            {
                CurrentAgent.ID = tempAgent.ID;
                currentAgent.CodeName = tempAgent.CodeName;
                currentAgent.Speciality = tempAgent.Speciality;
                currentAgent.Assignment = tempAgent.Assignment;
            }
        }


        ICommand _deleteCommand;
        public ICommand DeleteCommand
        {
            get { return _deleteCommand ?? (_deleteCommand = new RelayCommand(DeleteAgent, DeleteAgent_CanExecute)); }
        }

        private void DeleteAgent()
        {
            RemoveAt(CurrentIndex);
            NotifyPropertyChanged("Count");
        }

        private bool DeleteAgent_CanExecute()
        {
            if (Count > 0 && CurrentIndex >= 0)
                return true;
            else
                return false;
        }



        ICommand _SaveAsCommand;
        public ICommand SaveAsCommand
        {
            get { return _SaveAsCommand ?? (_SaveAsCommand = new RelayCommand<string>(SaveAsCommand_Execute)); }
        }

        private void SaveAsCommand_Execute(string argFilename)
        {
            var dialog = new SaveFileDialog();

            dialog.Filter = "Agent assignment documents|*.agn|All Files|*.*";
            dialog.DefaultExt = "agn";
            if (filePath == "")
                dialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            else
                dialog.InitialDirectory = Path.GetDirectoryName(filePath);

            if (dialog.ShowDialog(App.Current.MainWindow) == true)
            {
                filePath = dialog.FileName;
                filename = Path.GetFileName(filePath);
                SaveFile();
                Title = filename + " - " + AppTitle;

            }
        }

        ICommand _SaveCommand;
        public ICommand SaveCommand
        {
            get { return _SaveCommand ?? (_SaveCommand = new RelayCommand(SaveFileCommand_Execute, SaveFileCommand_CanExecute)); }
        }

        private void SaveFileCommand_Execute()
        {
            // Create an instance of the XmlSerializer class and specify the type of object to serialize.
            XmlSerializer serializer = new XmlSerializer(typeof(Agents));
            TextWriter writer = new StreamWriter(filename);
            // Serialize all the agents.
            serializer.Serialize(writer, this);
            writer.Close();
        }

        private void SaveFile()
        {
            List<Agent> tempAgents;

            try
            {
                // Copy the agents to a List. 
                tempAgents = this.ToList<Agent>();
                Repository.SaveFile(filePath, tempAgents);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Unable to save file", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private bool SaveFileCommand_CanExecute()
        {
            return (filename != "") && (Count > 0);
        }

        ICommand _NewFileCommand;
        public ICommand NewFileCommand
        {
            get { return _NewFileCommand ?? (_NewFileCommand = new RelayCommand(NewFileCommand_Execute)); }
        }

        private void NewFileCommand_Execute()
        {
            MessageBoxResult res = MessageBox.Show("Any unsaved data will be lost. Are you sure you want to initiate a new file?", "Warning",
                MessageBoxButton.YesNo, MessageBoxImage.Question, MessageBoxResult.No);
            if (res == MessageBoxResult.Yes)
            {
                Clear();
                filename = "";
            }
        }


        ICommand _OpenFileCommand;
        public ICommand OpenFileCommand
        {
            get { return _OpenFileCommand ?? (_OpenFileCommand = new RelayCommand(OpenFileCommand_Execute)); }
        }

        private void OpenFileCommand_Execute()
        {
            List<Agent> tempAgents;
            var dialog = new OpenFileDialog();

            dialog.Filter = "Agent assignment documents|*.agn|All Files|*.*";
            dialog.DefaultExt = "agn";
            if (filePath == "")
                dialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            else
                dialog.InitialDirectory = Path.GetDirectoryName(filePath);

            if (dialog.ShowDialog(App.Current.MainWindow) == true)
            {
                filePath = dialog.FileName;
                filename = Path.GetFileName(filePath);
                try
                {
                    Repository.ReadFile(filePath, out tempAgents);

                    // We have to insert the agents in the existing collection. 
                    Clear();
                    foreach (var agent in tempAgents)
                        Add(agent);
                    Title = filename + " - " + AppTitle;
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Unable to open file", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        ICommand _CloseAppCommand;
        public ICommand CloseAppCommand
        {
            get { return _CloseAppCommand ?? (_CloseAppCommand = new RelayCommand(CloseCommand_Execute)); }
        }

        private void CloseCommand_Execute()
        {
            Application.Current.MainWindow.Close();

            // Could use the line below instead
            // Application.Current.Shutdown();
        }


        ICommand _ColorCommand;


        public ICommand ColorCommand
        {
            get { return _ColorCommand ?? (_ColorCommand = new RelayCommand<String>(ColorCommand_Execute)); }
        }

        private void ColorCommand_Execute(String colorStr)
        {
            SolidColorBrush newBrush = SystemColors.WindowBrush; // Default color

            try
            {
                if (colorStr != null)
                {
                    if (colorStr != "Default")
                        newBrush = new SolidColorBrush((Color)ColorConverter.ConvertFromString(colorStr));
                }
            }
            catch (Exception)
            {
                MessageBox.Show("Unknown color name, default color is used", "Program error!");
            }

            Application.Current.MainWindow.Resources["BackgroundBrush"] = newBrush;
        }

        #endregion


    };

   [Serializable]
   public class Agent
   {
      string id;
      string codeName;
      string speciality;
      string assignment;

      public Agent()
      {
      }

      public Agent(string aId, string aName, string aAddress, string aSpeciality, string aAssignment)
      {
         id = aId;
         codeName = aName;
         speciality = aSpeciality;
         assignment = aAssignment;
      }

      public string ID
      {
         get
         {
            return id;
         }
         set
         {
            id = value;
         }
      }

      public string CodeName
      {
         get
         {
            return codeName;
         }
         set
         {
            codeName = value;
         }
      }

      public string Speciality
      {
         get
         {
            return speciality;
         }
         set
         {
            speciality = value;
         }
      }

      public string Assignment
      {
         get
         {
            return assignment;
         }
         set
         {
            assignment = value;
         }
      }
   }
}
