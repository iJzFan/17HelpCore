﻿using HELP.Service.ServiceInterface;
using System;
using System.Collections.Generic;
using System.Text;
using HELP.Service.ViewModel.Contact;
using System.Threading.Tasks;
using HELP.BLL.EntityFrameworkCore;
using HELP.GlobalFile.Global.Encryption;
using Microsoft.AspNetCore.Http;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using HELP.BLL.Entity;
using Microsoft.AspNetCore.Identity;

namespace HELP.Service.ProductionService
{
    public class ContactService : BaseService, IContactService
    {
        #region Constructor
        public ContactService(EFDbContext _context, IHttpContextAccessor _httpContextAccessor, IEncrypt _encrypt,UserManager<User> _userManager, SignInManager<User> _signInManager) : base(_context, _httpContextAccessor, _encrypt, _userManager, _signInManager)
        {
        }
        #endregion


        /// <summary>
        /// 获取当前用户联系方式
        /// </summary>
        /// <returns>RecordModel</returns>
        public async Task<RecordModel> Get()
        {
            return await Get(Convert.ToInt32((await GetCurrentUser()).Id));
        }


        /// <summary>
        /// 通过ID获取用户联系方式
        /// </summary>
        /// <param name="userId">UserId</param>
        /// <returns>RecordModel</returns>
        public async Task<RecordModel> Get(int userId)
        {
            var user = await _context.Users.Include(x=>x.contact).AsNoTracking().SingleOrDefaultAsync(x => x.Id == userId.ToString());
            return new RecordModel
            {
                QQ = user.contact.QQ,
                WeChat = user.contact.WeChat,
                Telephone = user.contact.Telephone,
                Other = user.contact.Other
            };
        }


        /// <summary>
        /// 设置用户联系方式
        /// </summary>
        /// <param name="model">RecordModel</param>
        /// <returns></returns>
        public async Task Save(RecordModel model)
        {
            var current = new Contact
            {
                QQ = model.QQ,
                WeChat = model.WeChat,
                Telephone = model.Telephone,
                Other = model.Other,
                UserId = (await GetCurrentUser()).Id
            };
            _context.Contacts.Update(current);
            await _context.SaveChangesAsync();

        }
    }
}
