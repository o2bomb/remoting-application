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
            catch (ArgumentException e)
            {
                // Catch exception thrown and throw an HTTP exception across the network, to the client
                var response = new HttpResponseMessage(HttpStatusCode.BadRequest)
                {
                    Content = new StringContent(String.Format("{0}", e.Message)),
                    // ReasonPhrase cannot contain any newline characters for some reason, so remove them
                    ReasonPhrase = e.Message.Replace('\n',  ' ').Replace('\r', ' ')
                };
                throw new HttpResponseException(response);
            }
            catch (CommunicationException e)
            {
                // Catch exception thrown and throw an HTTP exception across the network, to the client
                // Indicate to the client that the request's contents is too large to process
                var response = new HttpResponseMessage(HttpStatusCode.RequestEntityTooLarge)
                {
                    Content = new StringContent(String.Format("{0}", e.Message)),
                    // ReasonPhrase cannot contain any newline characters for some reason, so remove them
                    ReasonPhrase = e.Message.Replace('\n', ' ').Replace('\r', ' ')
                };
                throw new HttpResponseException(response);
            }
        }
    }
}