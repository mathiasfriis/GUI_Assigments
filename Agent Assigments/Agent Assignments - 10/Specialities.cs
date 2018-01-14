using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.ObjectModel;
using System.ComponentModel;

namespace Agent_Assignments
{
    public class Specialities : ObservableCollection<string>
    {
        public Specialities()
        {
            Add("Assassination");
            Add("Bombs");
            Add("Martinis");
            Add("Low profile");
            Add("Spy");
        }
    }
}
