﻿using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Doe.Ls.RAndD.CodeFirst.Runer.IdentityModel;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace Doe.Ls.RAndD.CodeFirst.Runer
{
    public class AspIdentityTest:TestBase
    {
        protected override void RunCore()
        {

          var result= CreateUserIdentityForSpecificDbConnection().Result;
          result=CreateUsersWithClaims().Result;
          result = AddingPasswords().Result;

        }

        private static async Task<bool> AddingPasswords()
        {
            var userManager = ApplicationUserManager.Create();
            foreach (var user in userManager.Users.ToList())
            {

                var result = await userManager.AddPasswordAsync(user.Id, "helloWorld@123");

                Console.WriteLine(result.Succeeded);
                Console.WriteLine(user.PasswordHash);
            }
            return true;
        }

        private static async Task<bool>  CreateUsersWithClaims()
        {
            var userManager = ApplicationUserManager.Create();

            for (var i = 0; i < 100; i++)
            {
                var newUser = new ApplicationUser()
                {
                    Email = $"First_{i}.Last@Email.com",
                    UserName = $"MyUser{i}.Name{1}@gmaEmail.com",
                    PhoneNumber = $"042{i}17{i}197",
                    SchoolCode = 1001,
                    Position = "Manager",
                    FirstName = $"First_{i}",
                    LastName = "Last",
                    TitleId = -1,
                    CreatedDate = DateTime.Now,
                    LastModifiedDate = DateTime.Now,
                    LastModifiedBy = Environment.UserName,
                    CreatedBy = Environment.UserName,
                };

                var result = userManager.Create(newUser);

                if (result.Succeeded)
                {
                    var user = await userManager.FindByEmailAsync(newUser.Email);
                    userManager.AddClaim(user.Id,
                        new Claim(ClaimTypes.DateOfBirth, DateTime.Now.AddYears(-20 - (i / 4)).ToString()));
                    userManager.AddClaim(user.Id, new Claim(ClaimTypes.UserData, Guid.NewGuid().ToString()));
                    if (i % 3 == 0)
                    {
                        userManager.AddClaim(user.Id, new Claim(ClaimTypes.Role, "Manager"));
                    }
                }
                else
                {
                    foreach (var error in result.Errors)
                    {
                        Console.WriteLine(error);
                    }
                }
            }
            return true;
        }

        private static async Task<bool> CreateUserIdentityForSpecificDbConnection()
        {
            var userManager = ApplicationUserManager.Create();

            var result = await userManager.CreateAsync(new ApplicationUser
            {
                Email = "refkyw@gmail.com",
                UserName = "refky.wahib@gmail.com",
                PhoneNumber = "0423177197",
                SchoolCode = 1001,
                Position = "Manager",
                FirstName = "Refky",
                LastName = "Wahib",
                TitleId = -1,
                CreatedDate = DateTime.Now,
                LastModifiedDate = DateTime.Now,
                LastModifiedBy = Environment.UserName,
                CreatedBy =  Environment.UserName,
            });

            Console.WriteLine(result.Succeeded);
            return true;
        }
    }
}
