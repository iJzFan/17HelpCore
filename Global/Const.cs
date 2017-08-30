namespace HELP.GlobalFile.Global
{
    public class Length
    {
        public const int Max = 80000;

        public const int ImageMaxLength = 1024*1024;
    }

    public class CookieName
    {
        public const string USER_ID = "USER_ID";
        public const string AUTH_CODE = "AuthCode";
        public const string ROLE = "Role";

        public static readonly string CheckCookieEnable = "ASP.NET_SessionId";
    }

    public class ClassMember
    {
        //TODO: 加文档以及规范命名
        public const string P_Id = "Id";
        public const string CREATE_TIME = "CreateTime";

        public const string FIELD_BALANCE = "_balance";
    }
}

