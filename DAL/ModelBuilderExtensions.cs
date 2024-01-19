using Quark_Backend.Entities;
using Microsoft.EntityFrameworkCore;

namespace Quark_Backend.DAL;
public static class ModelBuilderExtensions
{
    public static void Seed(this ModelBuilder modelBuilder)
    {
        //Departments
        modelBuilder.Entity<Department>().HasData(
            new Department
            {
                Id = 1,
                Name = "IT"
            },
            new Department
            {
                Id = 2,
                Name = "HR"
            }
        );

        //JobPositions
        modelBuilder.Entity<JobPosition>().HasData(
            new JobPosition
            {
                Id = 1,
                Name = "Junior Web Developer",
                DepartmentId = 1
            },
            new JobPosition
            {
                Id = 2,
                Name = "Mid Web Developer",
                DepartmentId = 1
            },
            new JobPosition
            {
                Id = 3,
                Name = "Senior Web Developer",
                DepartmentId = 1
            },
            new JobPosition
            {
                Id = 4,
                Name = "Junior Software Developer",
                DepartmentId = 1
            },
            new JobPosition
            {
                Id = 5,
                Name = "Mid Software Developer",
                DepartmentId = 1
            },
            new JobPosition
            {
                Id = 6,
                Name = "Senior Software Developer",
                DepartmentId = 1
            },
            new JobPosition
            {
                Id = 7,
                Name = "Recruiter",
                DepartmentId = 2 //HR
            }
        );

        //Users
        modelBuilder.Entity<User>().HasData(
            new User
            {
                Id = 1,
                Email = "adam.kowalski@gmail.com",
                Password = "1234"
            },
            new User
            {
                Id = 2,
                Email = "jan.nowak@gmail.com",
                Password = "1234"
            },
            new User
            {
                Id = 3,
                Email = "weronika.kowalczyk@gmail.com",
                Password = "1234"
            },
            new User
            {
                Id = 4,
                Email = "adrianna.lewandowska@gmail.com",
                Password = "1234"
            }
        );
    }
}