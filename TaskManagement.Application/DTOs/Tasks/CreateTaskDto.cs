using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using TaskManagement.Domain.Entities.Enums;

namespace TaskManagement.Application.DTOs.Tasks
{
    public class CreateTaskDto
    {
        [Required]
        public int ProjectId { get; set; }

        [Required]
        [MaxLength(200)]
        public string Title { get; set; } = string.Empty;

        [MaxLength(1000)]
        public string? Description { get; set; }

        [Required]
        public TaskItemStatus Status { get; set; }

        [Required]
        public TaskPriority Priority { get; set; }

        public DateTime? DueDate { get; set; }
    }
}