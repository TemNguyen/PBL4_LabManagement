using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PC_Heal_ServerService
{
    [Table("Account")]
    public class Account
    {
        [Key]
        public string Username { get; set; }
        public string Password { get; set; }
    }
}
