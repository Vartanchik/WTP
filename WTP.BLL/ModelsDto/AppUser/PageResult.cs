using System;
using System.Collections.Generic;
using System.Text;

namespace WTP.BLL.ModelsDto.AppUser
{
    public class PageResult<T>
    {
        public int Count { get; set; }
        public int PageIndex { get; set; }
        public int PageSize { get; set; }
        public List<T> Items { get; set; }
    }
}
