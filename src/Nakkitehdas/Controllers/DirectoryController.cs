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
     
    /// <summary>
    /// returns root
    /// </summary>
    /// <returns></returns>
        [HttpGet]
        [Route("[action]")]
        public List<ItemModel> GetItems()
        {
            return AzureData.Get_Items_By_ParentId("juuri");
        }

        // GET api/values/7
        [HttpGet]
        [Route("[action]/{id}")]
        public List<ItemModel> GetItems(string id)
        {
            return AzureData.Get_Items_By_ParentId(id);
        }



    }
}
