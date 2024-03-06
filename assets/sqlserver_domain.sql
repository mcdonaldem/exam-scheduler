CREATE TABLE [courses] (
  [id] integer PRIMARY KEY,
  [name] nvarchar(255),
  [start_date] date,
  [end_date] date
)
GO

CREATE TABLE [students] (
  [id] integer PRIMARY KEY,
  [name] nvarchar(255)
)
GO

CREATE TABLE [mentors] (
  [id] integer PRIMARY KEY,
  [name] nvarchar(255)
)
GO

CREATE TABLE [enrollments] (
  [id] integer PRIMARY KEY,
  [student_id] integer,
  [course_id] integer
)
GO

CREATE TABLE [exams] (
  [id] integer PRIMARY KEY,
  [student_id] integer,
  [start] datetime,
  [end] datetime
)
GO

CREATE TABLE [mentor_availabilities] (
  [id] integer PRIMARY KEY,
  [mentor_id] integer,
  [date] date,
  [time_slot] nvarchar(255)
)
GO

ALTER TABLE [exams] ADD FOREIGN KEY ([student_id]) REFERENCES [students] ([id])
GO

ALTER TABLE [enrollments] ADD FOREIGN KEY ([student_id]) REFERENCES [students] ([id])
GO

ALTER TABLE [enrollments] ADD FOREIGN KEY ([course_id]) REFERENCES [courses] ([id])
GO

CREATE TABLE [mentors_courses] (
  [mentors_id] integer,
  [courses_id] integer,
  PRIMARY KEY ([mentors_id], [courses_id])
);
GO

ALTER TABLE [mentors_courses] ADD FOREIGN KEY ([mentors_id]) REFERENCES [mentors] ([id]);
GO

ALTER TABLE [mentors_courses] ADD FOREIGN KEY ([courses_id]) REFERENCES [courses] ([id]);
GO


CREATE TABLE [mentors_exams] (
  [mentors_id] integer,
  [exams_id] integer,
  PRIMARY KEY ([mentors_id], [exams_id])
);
GO

ALTER TABLE [mentors_exams] ADD FOREIGN KEY ([mentors_id]) REFERENCES [mentors] ([id]);
GO

ALTER TABLE [mentors_exams] ADD FOREIGN KEY ([exams_id]) REFERENCES [exams] ([id]);
GO


ALTER TABLE [mentor_availabilities] ADD FOREIGN KEY ([mentor_id]) REFERENCES [mentors] ([id])
GO
