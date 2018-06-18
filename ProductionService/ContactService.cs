using HELP.BLL.Entity;
using HELP.BLL.EntityFrameworkCore;
using HELP.GlobalFile.Global.Encryption;
using HELP.Service.ServiceInterface;
using HELP.Service.ViewModel.Contact;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace HELP.Service.ProductionService
{
	public class ContactService : BaseService, IContactService
	{
		#region Constructor

		public ContactService(EFDbContext _context, IHttpContextAccessor _httpContextAccessor, IEncrypt _encrypt) : base(_context, _httpContextAccessor, _encrypt)
		{
		}

		#endregion Constructor

		/// <summary>
		/// 获取当前用户联系方式
		/// </summary>
		/// <returns>RecordModel</returns>
		public async Task<RecordModel> Get()
		{
			return await Get((await GetCurrentUser()).Id);
		}

		/// <summary>
		/// 通过ID获取用户联系方式
		/// </summary>
		/// <param name="userId">UserId</param>
		/// <returns>RecordModel</returns>
		public async Task<RecordModel> Get(string userId)
		{
			var user = await _context.Users.AsNoTracking().Include(x => x.contact).SingleOrDefaultAsync(x => x.Id == userId);
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