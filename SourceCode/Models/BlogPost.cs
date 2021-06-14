using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Lab4_Cs.Models
{
    public class BlogPost
    {
        [Key]
        public int BlogPostId
        {
            get;
            set;
        }
        [ForeignKey("UserId")]
        public int UserId
        {
            get;
            set;
        }
        [Required]
        [StringLength(200)]
        public string Title
        {
            get;
            set;
        }
        [Required]
        [StringLength(4000)]
        public string Content
        {
            get;
            set;
        }
        [Required]
        [DataType(DataType.Date)]
        public DateTime Posted
        {
            get;
            set;
        }
        public User user{
            get;
            set;
        }
        public  List<Comment> comment {
            get;
            set;
        }
//         public List<Photo> photo
//         {
//             get;
//             set;
//         }
    }
}
