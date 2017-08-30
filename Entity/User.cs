using HELP.GlobalFile.Global.Helper;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Security.Principal;

namespace HELP.BLL.Entity
{
    public class User: BaseEntity
    {

        private string _authCode;
        public string AuthCode
        {
            // 只能调用SetAuthCode()赋值
            get { return _authCode; }
        }

        public  string Name { get; set; }
        public  string Password { get; set; }

        public virtual Contact contact { get; set; }


        public int Credit { get; protected internal set; }

        public virtual IList<Credit> CreditHistory { get; set; }

        //public bool IsAuthenticated => throw new NotImplementedException();

        public virtual void SetAuthCode()
        {
            if (!string.IsNullOrEmpty(_authCode))
            {
                string message = string.Format(
                    "User(Id={0})已经有一个AuthCode({1})，不能覆盖",
                    Id, AuthCode);
                throw new Exception(message);
            }
            _authCode = RandomGenerator.GetNumbers(6);
        }

                public virtual void Register()
        {
            CreditHistory = new List<Credit>();
            Credit credit = new Credit
            {
                Count = 100,
                Description = "注册完成，系统赠送",
                UserId = this.Id,
                User=this
            };
            credit.SetBalance();
            CreditHistory.Add(credit);
        }

    }
}
