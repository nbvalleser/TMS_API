﻿namespace API_TMS.DTO
{
    public class TaskDto
    {
        public int Id { get; set; }        
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string Status { get; set; } = string.Empty; 
        public DateTime CreatedAt { get; set; }
    }
}
