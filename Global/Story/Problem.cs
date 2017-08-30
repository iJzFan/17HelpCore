using System;
using System.Collections.Generic;
using System.Text;

namespace HELP.GlobalFile.Global.Story
{
    public class Problem
    {
        #region DK

        public static int PhoneGap_Id = 1;
        public static DateTime PhoneGap_CreateTime = new DateTime(2017, 3, 1, 9, 47, 12);
        public static string PhoneGap_Title = "PhoneGap环境搭建问题";
        public static string PhoneGap_Body = "我用的是windows系统，没有Mac机，使用了一个虚拟机，然后还是报错：……跪求大神帮忙看一下。坐等。";
        public static int PhoneGap_Reward = 30;

        public static int WeChat_Id = 2;
        public static DateTime WeChat_CreateTime = new DateTime(2017, 2, 28, 19, 4, 0);
        public static string WeChat_Title = "微信支付调用JS-API接口支付的问题";
        public static string WeChat_Body = "先从官网下载的这个JS-SDK微信支付示例ASP.NET版：pay.weixin.qq.com/wiki/doc/api/jsapi.php?chapter=11_1。在使用这个接口的时候出现问题：jsApiPay.GetOpenidAndAccessToken();";
        public static int WeChat_Reward = 30;

        public static int SSCE_Id = 3;
        public static DateTime SSCE_CreateTime = new DateTime(2017, 2, 28, 9, 7, 0);
        public static string SSCE_Title = "搭vs2015专业版安装问题 某些东西打包失败，想安装上但又一直失败";
        public static string SSCE_Body = "后来自己到解压文件夹packages\\SSCE40下安装SSCERuntime_x64-chs.exe 但又显示……";
        public static int SSCE_Reward = 40;

        public static int WebGrease_Id = 4;
        public static DateTime WebGrease_CreateTime = new DateTime(2017, 2, 27, 8, 47, 0);
        public static string WebGrease_Title = "未能加载文件或程序集“WebGrease”或它的某一个依赖项。找到的程序集清单定义与程序集引用不匹配。 (异常来自 HRESULT:0x80131040)";
        public static string WebGrease_Body = "我在看这篇博文（http://www.cnblogs.com/xqin/p/3434792.html）的时候，试着按照它说的安装，但是又报了这个错误：";
        public static int WebGrease_Reward = 10;

        #endregion

        #region yezi

        public static int Install_Id = 5;
        public static DateTime Install_CreateTime = new DateTime(2017, 3, 1, 8, 42, 22);
        public static string Install_Title = "请问数据库和开发工具先装哪个";
        public static string Install_Body = "有vs2008和sqlserver2005，应该先装哪个";
        public static int Install_Reward = 20;

        #endregion
    }
}
