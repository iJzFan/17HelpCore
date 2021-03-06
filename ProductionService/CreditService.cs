﻿using HELP.BLL.Entity;
using HELP.BLL.EntityFrameworkCore;
using HELP.GlobalFile.Global.Encryption;
using HELP.GlobalFile.Global.Helper;
using HELP.Service.ServiceInterface;
using HELP.Service.ViewModel.Credit;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HELP.Service.ProductionService
{
	public class CreditService : BaseService, ICreditService
	{
		#region Constructor

		public CreditService(EFDbContext _context, IHttpContextAccessor _httpContextAccessor, IEncrypt _encrypt) : base(_context, _httpContextAccessor, _encrypt)
		{
		}

		#endregion Constructor

		/// <summary>
		/// 获取积分明细
		/// </summary>
		/// <returns>积分列表</returns>
		public async Task<IndexModel> Get()
		{
			var id = (await GetCurrentUser()).Id;
			IList<Credit> credits = await _context.Credit.Where(x => x.UserId == id).OrderByDescending(x => x.CreateTime).ToListAsync();
			var list = new List<IndexItemModel>();
			foreach (var credit in credits)
			{
				var indexItemModel = new IndexItemModel();
				indexItemModel.Balance = credit.Balance;
				indexItemModel.Count = credit.Count;
				indexItemModel.Description = credit.Description;
				indexItemModel.SetProperty("CreateTime", credit.CreateTime);
				list.Add(indexItemModel);
			}

			return new IndexModel { Items = list };
		}
	}
}