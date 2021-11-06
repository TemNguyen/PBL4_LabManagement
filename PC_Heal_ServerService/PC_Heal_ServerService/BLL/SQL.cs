using PC_Heal_ServerService.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PC_Heal_ServerService.BLL
{
    class SQL
    {
        public static SQL Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new SQL();
                }
                return _instance;
            }
            private set
            {
            }
        }
        private static SQL _instance;
        private SQL()
        {

        }

        public bool Add(CI computer)
        {
            using (CIDbContext db = new CIDbContext())
            {
                if (computer == null)
                    return false;
                else
                {
                    db.CI.Add(computer);
                    db.SaveChanges();
                    return true;
                }
            }
        }
        public bool Update(CI computer)
        {
            using (CIDbContext db = new CIDbContext())
            {
                if (computer == null)
                    return false;
                else
                {
                    var computerIndb = db.CI.Find(computer.ComputerName);

                    if (computerIndb == null)
                    {
                        return false;
                    }
                    else
                    {
                        computerIndb.CpuUsage = computer.CpuUsage;
                        computerIndb.RamUsage = computer.RamUsage;
                        computerIndb.NumThread = computer.NumThread;
                        computerIndb.NumProcess = computer.NumProcess;
                        computerIndb.DiskUsage = computer.DiskUsage;
                        computerIndb.GpuUsage = computer.GpuUsage;
                        computerIndb.ActiveTime = computer.ActiveTime;
                        computerIndb.IsOnline = computer.IsOnline;
                        db.SaveChanges();
                        return true;
                    }
                }
            }
        }

        public bool IsExist(string computerName)
        {
            using (CIDbContext db = new CIDbContext())
            {
                var computer = db.CI.Find(computerName);

                if (computer == null)
                    return false;
            }


            return true;
        }


    }
}
