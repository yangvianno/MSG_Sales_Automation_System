﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SalesAuto.Wasm.Features
{
    public class PagingLink
    {

        public string Text { set; get; }
        public int Page { get; set; }
        public bool Enabled { get; set; }
        public bool Active { get; set; }
        public PagingLink(int page, bool enable, string text)
        {
            Text = text;
            Page = page;
            Enabled = enable;            
        }


    }
}
