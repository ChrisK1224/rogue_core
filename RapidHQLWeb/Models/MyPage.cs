using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RapidHQLWeb.Models
{
    public class MyPage
    {
        [BindProperty]
        public string pageID { get; set; }
    }
}
