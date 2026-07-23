using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskManagement.Application.DTOs.Projects;
using TaskManagement.Application.DTOs.Tasks;
using TaskManagement.Domain.Entities;


namespace TaskManagement.Application.Mappings
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Project, ProjectDto>();

            CreateMap<Project, ProjectListDto>();

            CreateMap<CreateProjectDto, Project>();

            CreateMap<UpdateProjectDto, Project>();
            
            CreateMap<CreateTaskDto, TaskItem>();

            CreateMap<UpdateTaskDto, TaskItem>();

            CreateMap<TaskItem, TaskDto>();

            CreateMap<TaskItem, TaskListDto>().ForMember(dest => dest.ProjectName,opt => opt.MapFrom(src => src.Project.Name));
        }
    }
}