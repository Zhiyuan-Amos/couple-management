﻿using System;

namespace Couple.Client.Data.ToDo
{
    public class ToDoModel
    {
        public Guid Id { get; init; }
        public string Text { get; init; }
        public string Category { get; init; }
        public DateTime CreatedOn { get; init; }
    }
}
