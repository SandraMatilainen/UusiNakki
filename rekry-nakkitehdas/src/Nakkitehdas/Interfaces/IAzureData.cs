using Nakkitehdas.ViewModels;
using System.Collections.Generic;
using System.Linq;

namespace Nakkitehdas.DataProviders
{
    public interface IAzureData
    {
        List<IGrouping<int, ItemModel>> GetAzureBlobs();
    }
}