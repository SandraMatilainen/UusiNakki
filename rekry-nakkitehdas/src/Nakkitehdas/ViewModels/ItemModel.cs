
using System.Collections.Generic;

namespace Nakkitehdas.ViewModels
{
    public class ItemModel
    {
        public string Name { get; set; }

        public bool IsFolder { get; set; }

        public bool IsFile { get; set; }

        public int ParentId { get; set; }

    }
}
