﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SymViewModel.PF
{
    public class AccountTypeVM
    {
        public int Id { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }

        [StringLength(450, ErrorMessage = "Remarks cannot be longer than 450 characters.")]
        public string Remarks { get; set; }
        public string Operation { get; set; }
        public string TransType { get; set; }

    }
}
