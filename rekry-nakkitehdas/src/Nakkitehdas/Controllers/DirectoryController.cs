using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNet.Mvc;
using Nakkitehdas.ViewModels;
using Nakkitehdas.DataProviders;

// For more information on enabling Web API for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace Nakkitehdas.Controllers
{
    [Route("api/[controller]")]
    public class DirectoryController : Controller
    {
        [FromServices]
        public IAzureData AzureData { get; set; }

        // GET: api/values
        [HttpGet]
        [Route("[action]")]
        public List<IGrouping<int, ItemModel>> GetItems()
        {         
            return AzureData.GetAzureBlobs();
        }
    }
}
