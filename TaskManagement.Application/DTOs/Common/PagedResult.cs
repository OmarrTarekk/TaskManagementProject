using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskManagement.Application.DTOs.Common
{
    public class PagedResult<T>
    {
        public IReadOnlyList<T> Items { get; set; } = [];

        public int TotalCount { get; set; }

        public int Page { get; set; }

        public int Limit { get; set; }
    }
}