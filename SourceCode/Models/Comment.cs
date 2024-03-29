﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Lab4_Cs.Models
{
    public class Comment
    {
        [Key]
        public int CommentId
        {
            get;
            set;
        }
        [ForeignKey("BlogPostId")]
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
        [StringLength(2048)]
        public string Content
        {
            get;
            set;
        }
        public User user
        {
            get;
            set;
        }
    }
}
