using HELP.BLL.Entity;
using HELP.BLL.EntityFrameworkCore;
using System;
using System.Threading.Tasks;
using Xunit;

namespace DatabaseTest
{
    public class UserTableTest
    {




        [Fact]
        public void UpdateDatabase([FromServices] EFDbContext context)
        {
           context.Users.Add(new User { CreateTime = DateTime.Now, Name = "xiaoming", Password = "12345678" });

        }

        
    }
}
