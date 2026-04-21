using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReferenceData.Application.Common
{
    public class PagedResponse<T>
    {
        public List<T> Data { get; set; } = [];
        public int Page { get; set; }
        public int PageSize { get; set; }
        public int Total { get; set; }
    }
}
