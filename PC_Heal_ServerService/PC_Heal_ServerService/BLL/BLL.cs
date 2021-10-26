using PC_Heal_ServerService.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PC_Heal_ServerService.BLL
{
    class BLL
    {
        public static BLL Instance
        {
            get
            {
                if (_Instance == null)
                {
                    _Instance = new BLL();
                }
                return _Instance;
            }
            private set
            {
            }
        }
        private static BLL _Instance;
        private BLL()
        {

        }

        public bool Add(CI computer)
        {
            using(CIDbContext db = new CIDbContext())
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
            using(CIDbContext db = new CIDbContext())
            {
                if (computer == null)
                    return false;
                else
                {
                    var computerIndb = db.CI.Find(computer.ComputerName);

                    if(computerIndb == null)
                    {
                        return false;
                    }
                    else
                    {
                        computerIndb.CPU_Usage = computer.CPU_Usage;
                        computerIndb.RAM_Usage = computer.RAM_Usage;
                        computerIndb.Max_Clock_Speed = computer.Max_Clock_Speed;
                        computerIndb.Num_Thread = computer.Num_Thread;
                        computerIndb.Num_Process = computer.Num_Process;
                        computerIndb.Disk_Usage = computer.Disk_Usage;
                        computerIndb.GPU_Usage = computer.GPU_Usage;
                        computerIndb.ActiveTime = computerIndb.ActiveTime++;
                        db.SaveChanges();
                        return true;
                    }
                }
            }
        }

        public bool IsExist(string computerName)
        {
            using(CIDbContext db = new CIDbContext())
            {
                var computer = db.CI.Find(computerName);

                if (computer == null)
                    return false;
            }


            return true;
        }


    }
}
