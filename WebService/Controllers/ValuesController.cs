using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using APIClasses;
using WebService.Models;

namespace WebService.Controllers
{
    public class ValuesController : ApiController
    {
        private DataModel db = new DataModel();

        [HttpGet]
        public int GetCount()
        {
            return db.GetNumEntries();
        }
    }
}