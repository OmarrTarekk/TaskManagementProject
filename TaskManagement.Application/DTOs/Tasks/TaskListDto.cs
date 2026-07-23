using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskManagement.Domain.Entities.Enums;

namespace TaskManagement.Application.DTOs.Tasks
{
    public class TaskListDto
    {
        public int Id { get; set; }

        public string Title { get; set; } = string.Empty;

        public string ProjectName { get; set; } = string.Empty;
        public TaskItemStatus Status { get; set; }

        public TaskPriority Priority { get; set; }

        public DateTime? DueDate { get; set; }
    }
}