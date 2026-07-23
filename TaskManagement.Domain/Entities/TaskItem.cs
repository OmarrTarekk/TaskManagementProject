using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskManagement.Domain.Entities.Enums;
using TaskManagement.Domain.Entities.Common;

namespace TaskManagement.Domain.Entities
{
    public class TaskItem : BaseEntity<int>
    {
        public int ProjectId { get; set; }

        public string Title { get; set; } = null!;

        public string? Description { get; set; }

        public TaskItemStatus Status { get; set; } = TaskItemStatus.Todo;
        public TaskPriority Priority { get; set; } = TaskPriority.Medium;

        public DateTime? DueDate { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

      
        public Project Project { get; set; } = null!;
    }
}
