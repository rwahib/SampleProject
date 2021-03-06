﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Doe.Ls.RAndD.CodeFirst.Bll.CodeFirstEntities;

namespace Doe.Ls.RAndD.CodeFirst.Bll.Services
{
    public class CustomerService
    {
        private AdventureCtx _dbcontext;

        public CustomerService()
        {
            
            
        }

      public  virtual async Task<ResultModel<Customer>> GetCustomersByName(string name)
      {
         Console.WriteLine($" parameter {name} is invoked");
            //await Task.Delay(5000);
            List<Customer> resultData;
            using (_dbcontext = new AdventureCtx()){
                resultData= _dbcontext.Customers.Include("Person").Where(c =>
              c.Person.FirstName.ToLower().Contains(name.ToLower())
              ||
              c.Person.LastName.ToLower().Contains(name.ToLower()
              )).Take(100).ToList();

            }
         
          return  await Task.Run(() => new ResultModel<Customer>{ModelList = resultData, Search = name});
      }

        public virtual async Task<Customer> GetCustomerById(int customerId)
        {
            DebuggerInfo.DisplayCurrentMethod(MethodBase.GetCurrentMethod());
            Customer resultData;
            using (_dbcontext = new AdventureCtx())
            {
                resultData = _dbcontext.Customers.SingleOrDefault(c => c.CustomerID == customerId);
            }
            return await Task.Run(() => resultData);
        }

        public virtual async Task<ResultModel<Customer>> SearchCustomer(Func<IQueryable<Customer>, IQueryable<Customer>> expression)
        {
            Console.WriteLine($" parameter {expression} is invoked");
            //await Task.Delay(5000);
            List<Customer> resultData;
            using (_dbcontext = new AdventureCtx())
            {
                resultData = expression(_dbcontext.Customers.Include("Person")).ToList();

            }

            return await Task.Run(() => new ResultModel<Customer> { ModelList = resultData, Search = expression.ToString() });
        }
    }
}
