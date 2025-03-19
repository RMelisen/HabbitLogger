using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HabbitLogger.Commons.Classes
{
    internal class HabbitOccurence
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

        private Habbit _Habbit;
        public Habbit Habbit
        {
            get
            {
                return _Habbit;
            }
            set
            {
                _Habbit = value;
            }
        }

        private int _UnitAmount;
        public int UnitAmount
        {
            get
            {
                return _UnitAmount;
            }
            set
            {
                _UnitAmount = value;
            }
        }
        private DateTime _Datetime;

        public DateTime Datetime
        {
            get
            {
                return _Datetime;
            }
            set
            {
                _Datetime = value;
            }
        }

        public HabbitOccurence(int id, int habbitId, int unitAmount, string datetime)
        {
            Id = id;
            Habbit = DAL.HabbitloggerDAL.GetHabbitByID(id);
            UnitAmount = unitAmount;
            Datetime = DateTime.Parse(datetime);            
        }
        public HabbitOccurence(int id, int habbitId, int unitAmount, DateTime datetime)
        {
            Id = id;
            Habbit = DAL.HabbitloggerDAL.GetHabbitByID(id);
            UnitAmount = unitAmount;
            Datetime = datetime;
        }
    }
}
