﻿using HELP.GlobalFile.Global.Helper;
using HELP.GlobalFile.Global.Story;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HELP.BLL.EntityFrameworkCore
{
	public class InitData
	{
		private EFDbContext _context;

		/// <summary>
		/// Add init data to database
		/// </summary>
		/// <param name="context">DBContext</param>
		public InitData(EFDbContext context)
		{
			_context = context;
		}

		/// <summary>
		/// Add InitData to database
		/// </summary>
		public async Task Initialize()
		{
			await _context.Database.EnsureCreatedAsync();
			await AddUser();
			await AddProblems();
			await AddComments();
			await AddCredits();
		}

		/// <summary>
		/// Add Users Data(Contain Concacts)
		/// </summary>
		public async Task AddUser()
		{
			Entity.User yezi = InitUsers.Register("yezi");
			Entity.User DK = InitUsers.Register("DK");
			Entity.User TCreditIndex = InitUsers.Register("TCreditIndex");
			yezi.SetAuthCode();
			DK.SetAuthCode();
			TCreditIndex.SetAuthCode();
			await _context.Users.AddAsync(yezi);
			await _context.Users.AddAsync(DK);
			await _context.Users.AddAsync(TCreditIndex);
			await _context.SaveChangesAsync();
		}

		/// <summary>
		/// Add Proplems Data
		/// </summary>
		public async Task AddProblems()
		{
			Entity.User yezi = await _context.Users.Where(x => x.Name == "yezi").FirstOrDefaultAsync();
			Entity.User DK = await _context.Users.Where(x => x.Name == "DK").FirstOrDefaultAsync();
			List<Entity.Problem> list = new List<Entity.Problem>()
			{
				new Entity.Problem
			{
                //CreateTime = Problem.PhoneGap_CreateTime,
                Body = Problem.PhoneGap_Body,
				Reward = Problem.PhoneGap_Reward,
				Title = Problem.PhoneGap_Title,
				UserId = yezi.Id,
			},
				new Entity.Problem
			{
                //CreateTime = Problem.SSCE_CreateTime,
                Body = Problem.SSCE_Body,
				Reward = Problem.SSCE_Reward,
				Title = Problem.SSCE_Title,
				UserId = yezi.Id
			},
				new Entity.Problem
			{
                //CreateTime = Problem.WebGrease_CreateTime,
                Body = Problem.WebGrease_Body,
				Reward = Problem.WebGrease_Reward,
				Title = Problem.WebGrease_Title,
				UserId = DK.Id
			},
				new Entity.Problem
			{
                //CreateTime = Problem.WeChat_CreateTime,
                Body = Problem.WeChat_Body,
				Reward = Problem.WeChat_Reward,
				Title = Problem.WeChat_Title,
				UserId = yezi.Id
			},
				new Entity.Problem
			{
                //CreateTime = Problem.Install_CreateTime,
                Body = Problem.Install_Body,
				Reward = Problem.Install_Reward,
				Title = Problem.Install_Title,
				UserId = DK.Id
			}
			};
			for (int i = 1; i < 20; i++)
			{
				Entity.Problem fakeproblem = new Entity.Problem
				{
					Body = "Fakeproblem" + i.ToString(),
					Title = "FakeTitle" + i.ToString()
				};
				fakeproblem.SetPrivateFieldInBase("<CreateTime>k__BackingField", DateTime.Now.AddDays(-i));
				fakeproblem.UserId = yezi.Id;
				fakeproblem.Reward = i;
				list.Add(fakeproblem);
			}

			foreach (Entity.Problem problem in list)
			{
				await _context.Problems.AddAsync(problem);
				await _context.SaveChangesAsync();
			}
		}

		/// <summary>
		/// Add Comments Data
		/// </summary>
		public async Task AddComments()
		{
			Entity.User yezi = await _context.Users.Where(x => x.Name == "yezi").FirstOrDefaultAsync();
			Entity.User DK = await _context.Users.Where(x => x.Name == "DK").FirstOrDefaultAsync();
			List<Entity.Comment> list = new List<Entity.Comment>
			{
				new Entity.Comment
				{
					Body=Comment.PhoneGap_Comment_1_Body,
                    //CreateTime=Comment.PhoneGap_Comment_1_CreateTime,
                    UserId=yezi.Id,
					ProblemId=1,
					Floor=1
				},
				new Entity.Comment
				{
					Body=Comment.PhoneGap_Reply_1_Body,
					UserId=DK.Id,
					ProblemId=1,
					Floor=2
				}
			};

			foreach (Entity.Comment comment in list)
			{
				await _context.Comments.AddAsync(comment);
				await _context.SaveChangesAsync();
			}
		}

		/// <summary>
		/// Add Credits Data
		/// </summary>
		public async Task AddCredits()
		{
			InitUsers.yezi.Register();
			InitUsers.DK.Register();
			InitUsers.TCreditIndex.Register();
			await _context.Credit.AddAsync(InitCredit.SaveCredit(InitUsers.yezi.CreditHistory.FirstOrDefault().Count,
				InitUsers.yezi.CreditHistory.FirstOrDefault().Description,
				InitUsers.yezi.CreditHistory.FirstOrDefault().CreateTime, _context));
			await _context.Credit.AddAsync(InitCredit.SaveCredit(InitUsers.DK.CreditHistory.FirstOrDefault().Count,
				InitUsers.DK.CreditHistory.FirstOrDefault().Description,
				InitUsers.DK.CreditHistory.FirstOrDefault().CreateTime, _context));
			await _context.Credit.AddAsync(InitCredit.SaveCredit(InitUsers.TCreditIndex.CreditHistory.FirstOrDefault().Count,
				InitUsers.TCreditIndex.CreditHistory.FirstOrDefault().Description,
				InitUsers.TCreditIndex.CreditHistory.FirstOrDefault().CreateTime, _context));
			await _context.Credit.AddAsync(InitCredit.SaveCredit(Credit.TCreditIndex_Count_1,
				Credit.TCreditIndex_Description_1,
				Credit.TCreditIndex_CreateTime_1, _context));
			await _context.Credit.AddAsync(InitCredit.SaveCredit(Credit.TCreditIndex_Count_2,
				Credit.TCreditIndex_Description_2,
				Credit.TCreditIndex_CreateTime_2, _context));
			await _context.Credit.AddAsync(InitCredit.SaveCredit(Credit.TCreditIndex_Count_3,
				Credit.TCreditIndex_Description_3,
				Credit.TCreditIndex_CreateTime_3, _context));
			await _context.Credit.AddAsync(InitCredit.SaveCredit(Credit.TCreditIndex_Count_4,
				Credit.TCreditIndex_Description_4,
				Credit.TCreditIndex_CreateTime_4, _context));
			await _context.Credit.AddAsync(InitCredit.SaveCredit(Credit.TCreditIndex_Count_5,
				Credit.TCreditIndex_Description_5,
				Credit.TCreditIndex_CreateTime_5, _context));
			await _context.Credit.AddAsync(InitCredit.SaveCredit(Credit.TCreditIndex_Count_6,
				Credit.TCreditIndex_Description_6,
				Credit.TCreditIndex_CreateTime_6, _context));
			await _context.Credit.AddAsync(InitCredit.SaveCredit(Credit.TCreditIndex_Count_7,
				Credit.TCreditIndex_Description_7,
				Credit.TCreditIndex_CreateTime_7, _context));
			await _context.Credit.AddAsync(InitCredit.SaveCredit(Credit.TCreditIndex_Count_8,
				Credit.TCreditIndex_Description_8,
				Credit.TCreditIndex_CreateTime_8, _context));
			await _context.Credit.AddAsync(InitCredit.SaveCredit(Credit.TCreditIndex_Count_9,
				Credit.TCreditIndex_Description_9,
				Credit.TCreditIndex_CreateTime_9, _context));
			await _context.Credit.AddAsync(InitCredit.SaveCredit(Credit.TCreditIndex_Count_10,
				Credit.TCreditIndex_Description_10,
				Credit.TCreditIndex_CreateTime_10, _context));
			await _context.Credit.AddAsync(InitCredit.SaveCredit(Credit.TCreditIndex_Count_11,
				Credit.TCreditIndex_Description_11,
				Credit.TCreditIndex_CreateTime_11, _context));
			await _context.Credit.AddAsync(InitCredit.SaveCredit(Credit.TCreditIndex_Count_12,
				Credit.TCreditIndex_Description_12,
				Credit.TCreditIndex_CreateTime_12, _context));
			await _context.Credit.AddAsync(InitCredit.SaveCredit(Credit.TCreditIndex_Count_13,
				Credit.TCreditIndex_Description_13,
				Credit.TCreditIndex_CreateTime_13, _context));
			_context.SaveChanges();
		}
	}

	public class InitUsers
	{
		internal static string password = "2491d59c3432b1a10deb43f1907b9d4e9625081d0187532bcce7820a6be830074d24270844772fb330f44fb2039a806f3683c80b68de41c35eb3a7bf64bdaaad";

		public static Entity.User Register(string name)
		{
			Entity.User user = new Entity.User
			{
				Name = name,
				Password = password,
				contact = new Entity.Contact()
			};

			return user;
		}

		public static Entity.Contact yezicontact = new Entity.Contact
		{
			QQ = Contact.yezi_QQ,
			Telephone = Contact.yezi_Telephone,
			WeChat = Contact.yezi_Wechat,
			Other = Contact.yezi_Other,
		};

		public static Entity.Contact DKcontact = new Entity.Contact
		{
			QQ = Contact.dk_QQ
		};

		public static Entity.User yezi = Register("yezi");

		public static Entity.User DK = Register("DK");

		public static Entity.User TCreditIndex = Register("TCreditIndex");
	}

	public class InitCredit
	{
		public static Entity.Credit SaveCredit(int count, string description, DateTime create, EFDbContext _context)
		{
			Entity.User index = _context.Users.Where(x => x.Name == "TCreditIndex").FirstOrDefault();
			Entity.Credit credit = new Entity.Credit
			{
				Count = count,
				Description = description,
				UserId = index.Id
			};
			credit.SetPrivateFieldInBase("<CreateTime>k__BackingField", create);

			return credit;
		}
	}
}