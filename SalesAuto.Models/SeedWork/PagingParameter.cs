using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SalesAuto.Models.SeedWork
{
    public class PagingParameters
    {
        const int maxPageSize = 50;
        public int pageNumber { get; set; } = 1;
        public int _pageSize=50;
        public int PageSize 
        { 
            get
            {
                return _pageSize;
            }
            set
            {
                _pageSize = (value > maxPageSize ? maxPageSize : value);
            }
        }
    }
}
