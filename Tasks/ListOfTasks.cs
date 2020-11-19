using System;
using System.Collections.Generic;
using System.Text;

namespace ZanzibarBot.Tasks
{
    public static class ListOfTasks
    {
        private readonly static Task task1 = new Task(1, "Задача 1: 1 + 1 = ?", "2");
        private readonly static Task task2 = new Task(2, "Задача 2: 2 + 2 = ?", "4");
        private readonly static Task task3 = new Task(3, "Задача 3: 3 + 3 = ?", "6");
        private readonly static Task task4 = new Task(4, "Задача 4: 4 + 4 = ?", "8");
        private readonly static Task task5 = new Task(5, "Задача 5: 5 + 5 = ?", "10");
        private readonly static Task task6 = new Task(6, "Задача 6: 6 + 6 = ?", "12");
        private readonly static Task task7 = new Task(7, "Задача 7: 7 + 7 = ?", "14");
        private readonly static Task task8 = new Task(8, "Задача 8: 8 + 8 = ?", "16");
        private readonly static Task task9 = new Task(9, "Задача 9: 9 + 9 = ?", "18");
        private readonly static Task task10 = new Task(10, "Задача 10: 10 + 10 = ?", "20");
        private readonly static Task task11 = new Task(11, "Задача 11: 11 + 11 = ?", "22");
        private readonly static Task task12 = new Task(12, "Задача 12: 12 + 12 = ?", "24");
        private readonly static Task task13 = new Task(13, "Задача 13: 13 + 13 = ?", "26");
        private readonly static Task task14 = new Task(14, "Задача 14: 14 + 14 = ?", "28");
        private readonly static Task task15 = new Task(15, "Задача 15: 15 + 15 = ?", "30");
        private readonly static Task task16 = new Task(16, "Задача 16: 16 + 16 = ?", "32");
        private readonly static Task task17 = new Task(17, "Задача 17: 17 + 17 = ?", "34");
        private readonly static Task task18 = new Task(18, "Задача 18: 18 + 18 = ?", "36");
        private readonly static Task task19 = new Task(19, "Задача 19: 19 + 19 = ?", "38");
        private readonly static Task task20 = new Task(20, "Задача 20: 20 + 20 = ?", "40");
        
        public static List<Task> Tasks = new List<Task> 
        { 
            task1, task2, task3, task4, task5,
            task6, task7, task8, task9, task10,
            task11, task12, task13, task14, task15,
            task16, task17, task18, task19, task20
        };

        public static Task GetTask(int Number)
        {
            foreach (Task task in Tasks)
            {
                if (task.Number == Number)
                {
                    return task;
                }
            }
            throw new Exception("No task with such id.");
        }

    }
}
