using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HabbitLogger.Commons.Classes
{
    internal class Habbit
    {
        private int _Id;
        public int Id 
        { 
            get 
            { 
                return _Id; 
            } 
            set 
            { 
                _Id = value; 
            }
        }

        private string _Name;
        public string Name
        {
            get
            {
                return _Name;
            }
            set
            {
                _Name = value;
            }
        }

        private string _Description;
        public string Description
        {
            get
            {
                return _Description;
            }
            set
            {
                _Description = value;
            }
        }

        private UnitOfMeasure _UnitOfMeasure;
        public UnitOfMeasure UnitOfMeasure
        {
            get
            {
                return _UnitOfMeasure;
            }
            set
            {
                _UnitOfMeasure = value;
            }
        }

        public Habbit(int id, string name, string description, int unitOfMeasure)
        {
            Id = id;
            Name = name;
            Description = description;
        }
    }
}
