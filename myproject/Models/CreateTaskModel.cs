using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace myproject.Models
{
    public class CreateTaskModel
    {
        public int Task_Id { get; set; }

        
        public string Department { get; set; }
        public string Employee_name { get; set; }

        public string Project_title { get; set; }
        public string Task_name { get; set; }
        public string Task_Description { get; set; }
        public string task_priority { get; set; }
        public string Assigned_date { get; set; }
        public string Due_date { get; set; }
        public override string ToString()
        {
            return $"Task ID: {Task_Id}, Department: {Department}, Employee Name: {Employee_name}, Project Title: {Project_title}, Task Name: {Task_name}, Task Description: {Task_Description}, Task Priority: {task_priority}, Assigned Date: {Assigned_date}, Due Date: {Due_date}";
        }
    }
}
