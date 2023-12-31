﻿namespace TodoWebService.Models.DTOs.Todo
{
    public class TodoItemDto
    {
        public int Id { get; set; }
        public string Text { get; set; } = string.Empty;
        public bool IsComleted { get; set; }
        public bool IsNotified { get; set; }
        public DateTimeOffset CreatedAt { get; set; }
        public DateTime EndTime { get; set; }
        public bool Notify { get; set; } = default;
    }
}
