using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
namespace Lab4_Cs.Models
{
    public class User
    {
        [Key]
        public int UserId
        {
            get;
            set;
        }
        [ForeignKey("RoleId")]
        public int RoleId
        {
            get;
            set;
        }
        [Required]
        [StringLength(50)]
        public string FirstName
        {
            get;
            set;
        }
        [Required]
        [StringLength(50)]
        public string LastName
        {
            get;
            set;
        }
        [Required]
        [StringLength(50)]
        public string EmailAddress
        {
            get;
            set;
        }
        [Required]
        [StringLength(50,MinimumLength =8)]
        public string Password
        {
            get;
            set;
        }
    }
}
