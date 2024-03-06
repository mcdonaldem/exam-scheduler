CREATE TABLE "courses" (
  "id" integer PRIMARY KEY,
  "name" varchar,
  "start_date" date,
  "end_date" date
);

CREATE TABLE "students" (
  "id" integer PRIMARY KEY,
  "name" varchar
);

CREATE TABLE "mentors" (
  "id" integer PRIMARY KEY,
  "name" varchar
);

CREATE TABLE "enrollments" (
  "id" integer PRIMARY KEY,
  "student_id" integer,
  "course_id" integer
);

CREATE TABLE "exams" (
  "id" integer PRIMARY KEY,
  "student_id" integer,
  "start" datetime,
  "end" datetime
);

CREATE TABLE "mentor_availabilities" (
  "id" integer PRIMARY KEY,
  "mentor_id" integer,
  "date" date,
  "time_slot" varchar
);

ALTER TABLE "exams" ADD FOREIGN KEY ("student_id") REFERENCES "students" ("id");

ALTER TABLE "enrollments" ADD FOREIGN KEY ("student_id") REFERENCES "students" ("id");

ALTER TABLE "enrollments" ADD FOREIGN KEY ("course_id") REFERENCES "courses" ("id");

CREATE TABLE "mentors_courses" (
  "mentors_id" integer,
  "courses_id" integer,
  PRIMARY KEY ("mentors_id", "courses_id")
);

ALTER TABLE "mentors_courses" ADD FOREIGN KEY ("mentors_id") REFERENCES "mentors" ("id");

ALTER TABLE "mentors_courses" ADD FOREIGN KEY ("courses_id") REFERENCES "courses" ("id");


CREATE TABLE "mentors_exams" (
  "mentors_id" integer,
  "exams_id" integer,
  PRIMARY KEY ("mentors_id", "exams_id")
);

ALTER TABLE "mentors_exams" ADD FOREIGN KEY ("mentors_id") REFERENCES "mentors" ("id");

ALTER TABLE "mentors_exams" ADD FOREIGN KEY ("exams_id") REFERENCES "exams" ("id");


ALTER TABLE "mentor_availabilities" ADD FOREIGN KEY ("mentor_id") REFERENCES "mentors" ("id");
