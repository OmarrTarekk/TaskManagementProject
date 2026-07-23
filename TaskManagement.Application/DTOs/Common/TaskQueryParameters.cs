using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using TaskManagement.Domain.Entities.Enums;

namespace TaskManagement.Application.DTOs.Common
{
    public class TaskQueryParameters
    {
        public int Page { get; set; } = 1;

        public int Limit { get; set; } = 10;

        public TaskItemStatus? Status { get; set; }

        public TaskPriority? Priority { get; set; }

        public DateTime? DueDateFrom { get; set; }

        public DateTime? DueDateTo { get; set; }

        public string? SortBy { get; set; }

        public string? SortOrder { get; set; }

        public string? Q { get; set; }
    }
}