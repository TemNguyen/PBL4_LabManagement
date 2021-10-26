using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson.Serialization.IdGenerators;
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
        [BsonId(IdGenerator = typeof(StringObjectIdGenerator))]
        public string Username { get; set; }
        [BsonElement]
        public string Password { get; set; }
    }
}
