using System;

namespace ShopM4_Models.ViewModels
{
    public class QueryViewModel
    {
        public QueryHeader QueryHeader { get; set; }

        public IEnumerable<QueryDetail> QueryDetail { get; set; }
    }
}

