﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SymViewModel.Common
{
    public class BlockEmployerVM
    {
        public int Id { get; set; }
        public int JobSeekerId { get; set; }
        public int EmployerId { get; set; }
        public string FullName { get; set; }
        public string LogoName { get; set; }
        public string PresentDistrictId { get; set; }
        public int JobCategoryId { get; set; }
        public string ContactPersonName { get; set; }
        public string ContactPersonDesignation { get; set; }
        public string ContactPersonMobile { get; set; }
        public string ContactPersonEmail { get; set; }
        public string Website  { get; set; }

        public string BlockStatus { get; set; }
    }
}
