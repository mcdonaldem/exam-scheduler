﻿using Microsoft.EntityFrameworkCore.Design.Internal;

namespace ExamScheduler.Models
{
    public class AlgoLanguage
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public List<Mentor> Mentors { get; set; }
    }
}
