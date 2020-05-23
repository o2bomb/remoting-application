using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.ServiceModel;
using APIClasses;
using WebService.Models;

namespace WebService.Controllers
{
    public class EditController : ApiController
    {
        private DataModel db = new DataModel();
        // POST api/<controller>/5
        [HttpPost]
        public void Post(int id, [FromBody]DataIntermed value)
        {
            try
            {
                db.EditValuesFromIndex(id, value.acct, value.pin, value.bal, value.fName, value.lName, value.profileImg);
            }
            catch (FaultException<SimpleDLL.DatabaseFault> e)
            {
                Console.WriteLine("stopped at controller");
                // Catch any database exceptions thrown and throw an HTTP exception across the network, to the client
                var response = new HttpResponseMessage(HttpStatusCode.BadRequest)
                {
                    Content = new StringContent(String.Format("Failed to perform {0} operation on database", e.Detail.Operation)),
                    ReasonPhrase = e.Detail.ProblemType
                };
                throw new HttpResponseException(response);
            }
        }
    }
}