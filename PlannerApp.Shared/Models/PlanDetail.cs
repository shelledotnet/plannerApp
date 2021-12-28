using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlannerApp.Shared.Models
{
    public class PlanDetail : PlanSummary
    {
        public IFormFile CoverFile { get; set; }
        //Todo list of items
    }
}
