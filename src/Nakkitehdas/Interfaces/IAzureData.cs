using Nakkitehdas.ViewModels;
using System.Collections.Generic;
using System.Linq;

namespace Nakkitehdas.DataProviders
{
    public interface IAzureData
    {
        /// <summary>
        /// gets all values
        /// </summary>
        /// <returns></returns>
        List<IGrouping<string, ItemModel>> GetAzureBlob();

        /// <summary>
        /// gets items by parentid
        /// </summary>
        /// <param name="parentId"></param>
        /// <returns></returns>
        List<ItemModel> Get_Items_By_ParentId(string parentId);
    }
}