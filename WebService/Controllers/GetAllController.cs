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
    public class GetAllController : ApiController
    {
        private DataModel db = new DataModel();

        // GET api/<controller>
        [HttpGet]
        public DataIntermed GetEntry(int id)
        {
            DataIntermed data = db.GetValuesFromIndex(id);

            return data;
        }
    }
}