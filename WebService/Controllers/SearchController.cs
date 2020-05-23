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
    public class SearchController : ApiController
    {
        private DataModel db = new DataModel();
        // POST api/<controller>/
        [HttpPost]
        public DataIntermed Post(SearchData searchQuery)
        {
            DataIntermed data = db.GetValuesFromLName(searchQuery.searchStr);

            return data;
        }
    }
}