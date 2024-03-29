﻿namespace ExamScheduler.Entities
{
    public class AlgoLanguage
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public List<Mentor> Mentors { get; set; }

        public AlgoLanguage()
        {
        }

        public AlgoLanguage(string name)
        {
            Name = name;
        }
    }
}
