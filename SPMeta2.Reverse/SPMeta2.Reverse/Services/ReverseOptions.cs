﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SPMeta2.Reverse.Services
{
    public class ReverseOptions
    {
        #region constructors

        public ReverseOptions()
        {
            Options = new List<ReverseOption>();
        }

        #endregion

        #region static

        public static ReverseOptions Default
        {
            get { return new ReverseOptions(); }
        }

        #endregion

        #region propeties

        public List<ReverseOption> Options { get; set; }

        #endregion

        #region methods

        #endregion
    }
}
