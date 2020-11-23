using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;

namespace webapi_sample.Entity
{
    [Table("Users")]
    public class Person
    {
        [Column("UserId")]
        public int Id {get;set;}

        [Column("Name")]
        public string Name {get;set;}
        
        [Column("Surname")]
        public string Surname {get; set;}


    }
}