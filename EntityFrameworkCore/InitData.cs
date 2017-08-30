using HELP.GlobalFile.Global.Helper;
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
            var yezi = InitUsers.Register("yezi");
            var DK = InitUsers.Register("DK");
            var TCreditIndex = InitUsers.Register("TCreditIndex");
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
            var list = new List<Entity.Problem>()
            {
                new Entity.Problem
            {
                //CreateTime = Problem.PhoneGap_CreateTime,
                Body = Problem.PhoneGap_Body,
                Reward = Problem.PhoneGap_Reward,
                Title = Problem.PhoneGap_Title,
                UserId = 2
            },
                new Entity.Problem
            {
                //CreateTime = Problem.SSCE_CreateTime,
                Body = Problem.SSCE_Body,
                Reward = Problem.SSCE_Reward,
                Title = Problem.SSCE_Title,
                UserId = 2
            },
                new Entity.Problem
            {
                //CreateTime = Problem.WebGrease_CreateTime,
                Body = Problem.WebGrease_Body,
                Reward = Problem.WebGrease_Reward,
                Title = Problem.WebGrease_Title,
                UserId = 2
            },
                new Entity.Problem
            {
                //CreateTime = Problem.WeChat_CreateTime,
                Body = Problem.WeChat_Body,
                Reward = Problem.WeChat_Reward,
                Title = Problem.WeChat_Title,
                UserId = 2
            },
                new Entity.Problem
            {
                //CreateTime = Problem.Install_CreateTime,
                Body = Problem.Install_Body,
                Reward = Problem.Install_Reward,
                Title = Problem.Install_Title,
                UserId = 1
            }
            };
            for (int i = 1; i < 20; i++)
            {
                var fakeproblem = new Entity.Problem();
                fakeproblem.Body = "Fakeproblem" + i.ToString();
                fakeproblem.Title = "FakeTitle" + i.ToString();
                fakeproblem.SetPrivateFieldInBase("_CreateTime", DateTime.Now.AddDays(-i));
                fakeproblem.UserId = 1;
                fakeproblem.Reward = i;
                list.Add(fakeproblem);
            }

            foreach (var problem in list)
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
            var list = new List<Entity.Comment>
            {
                new Entity.Comment
                {
                    Body=Comment.PhoneGap_Comment_1_Body,
                    //CreateTime=Comment.PhoneGap_Comment_1_CreateTime,
                    UserId=1,
                    ProblemId=1

                },
                new Entity.Comment
                {
                    Body=Comment.PhoneGap_Reply_1_Body,
                    UserId=2,
                    ProblemId=1
                }
            };

            foreach (var comment in list)
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
                InitUsers.yezi.CreditHistory.FirstOrDefault().CreateTime));
            await _context.Credit.AddAsync(InitCredit.SaveCredit(InitUsers.DK.CreditHistory.FirstOrDefault().Count,
                InitUsers.DK.CreditHistory.FirstOrDefault().Description,
                InitUsers.DK.CreditHistory.FirstOrDefault().CreateTime));
            await _context.Credit.AddAsync(InitCredit.SaveCredit(InitUsers.TCreditIndex.CreditHistory.FirstOrDefault().Count,
                InitUsers.TCreditIndex.CreditHistory.FirstOrDefault().Description,
                InitUsers.TCreditIndex.CreditHistory.FirstOrDefault().CreateTime));
            await _context.Credit.AddAsync(InitCredit.SaveCredit(Credit.TCreditIndex_Count_1,
                Credit.TCreditIndex_Description_1,
                Credit.TCreditIndex_CreateTime_1));
            await _context.Credit.AddAsync(InitCredit.SaveCredit(Credit.TCreditIndex_Count_2,
                Credit.TCreditIndex_Description_2,
                Credit.TCreditIndex_CreateTime_2));
            await _context.Credit.AddAsync(InitCredit.SaveCredit(Credit.TCreditIndex_Count_3,
                Credit.TCreditIndex_Description_3,
                Credit.TCreditIndex_CreateTime_3));
            await _context.Credit.AddAsync(InitCredit.SaveCredit(Credit.TCreditIndex_Count_4,
                Credit.TCreditIndex_Description_4,
                Credit.TCreditIndex_CreateTime_4));
            await _context.Credit.AddAsync(InitCredit.SaveCredit(Credit.TCreditIndex_Count_5,
                Credit.TCreditIndex_Description_5,
                Credit.TCreditIndex_CreateTime_5));
            await _context.Credit.AddAsync(InitCredit.SaveCredit(Credit.TCreditIndex_Count_6,
                Credit.TCreditIndex_Description_6,
                Credit.TCreditIndex_CreateTime_6));
            await _context.Credit.AddAsync(InitCredit.SaveCredit(Credit.TCreditIndex_Count_7,
                Credit.TCreditIndex_Description_7,
                Credit.TCreditIndex_CreateTime_7));
            await _context.Credit.AddAsync(InitCredit.SaveCredit(Credit.TCreditIndex_Count_8,
                Credit.TCreditIndex_Description_8,
                Credit.TCreditIndex_CreateTime_8));
            await _context.Credit.AddAsync(InitCredit.SaveCredit(Credit.TCreditIndex_Count_9,
                Credit.TCreditIndex_Description_9,
                Credit.TCreditIndex_CreateTime_9));
            await _context.Credit.AddAsync(InitCredit.SaveCredit(Credit.TCreditIndex_Count_10,
                Credit.TCreditIndex_Description_10,
                Credit.TCreditIndex_CreateTime_10));
            await _context.Credit.AddAsync(InitCredit.SaveCredit(Credit.TCreditIndex_Count_11,
                Credit.TCreditIndex_Description_11,
                Credit.TCreditIndex_CreateTime_11));
            await _context.Credit.AddAsync(InitCredit.SaveCredit(Credit.TCreditIndex_Count_12,
                Credit.TCreditIndex_Description_12,
                Credit.TCreditIndex_CreateTime_12));
            await _context.Credit.AddAsync(InitCredit.SaveCredit(Credit.TCreditIndex_Count_13,
                Credit.TCreditIndex_Description_13,
                Credit.TCreditIndex_CreateTime_13));
            _context.SaveChanges();
        }
    }



    public class InitUsers
    {
        internal static string password = "2491d59c3432b1a10deb43f1907b9d4e9625081d0187532bcce7820a6be830074d24270844772fb330f44fb2039a806f3683c80b68de41c35eb3a7bf64bdaaad";
        public static Entity.User Register(string name)
        {
            var user = new Entity.User();
            user.Name = name;
            user.Password = password;
            user.contact = new Entity.Contact();

            return user;
        }

        public static Entity.Contact yezicontact = new Entity.Contact
        {
            UserId = 1,
            QQ = Contact.yezi_QQ,
            Telephone = Contact.yezi_Telephone,
            WeChat = Contact.yezi_Wechat,
            Other = Contact.yezi_Other,
        };

        public static Entity.Contact DKcontact = new Entity.Contact
        {
            UserId = 2,
            QQ = Contact.dk_QQ
        };

        public static Entity.User yezi = Register("yezi");

        public static Entity.User DK = Register("DK");

        public static Entity.User TCreditIndex = Register("TCreditIndex");

    }

    public class InitCredit
    {
        public static Entity.Credit SaveCredit(int count, string description, DateTime create)
        {
            var credit = new Entity.Credit
            {
                Count = count,
                Description = description,
                UserId = 3
            };
            credit.SetPrivateFieldInBase("_CreateTime", create);

            return credit;
        }
    }
}
