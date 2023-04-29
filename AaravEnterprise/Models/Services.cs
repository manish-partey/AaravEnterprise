﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AaravWebSolution.Models
{
    public class Services
    {
        [Key]
        public int Id { get; set; }
        public string ServiceTitle { get; set; }
        public string ServiceDescription { get; set; }
      
    }
}
