namespace SchoolProject.Data.AppMetaData
{
    public static class Router
    {
        public const string SingleRoute = "/{id}";

        public const string root = "";
        public const string vesrsion = "";
        public const string Rule = root + "/" + vesrsion + "/";

        public static class StudentRouting
        {
            public const string Prefix = Rule + "Student";
            public const string List = Prefix + "/List";
            public const string GetById = Prefix + SingleRoute;
            public const string Create = Prefix + "/Create";
            public const string Edit = Prefix + "/Edit";
            public const string Delete = Prefix + "/Delete" + SingleRoute;
            public const string Paginated = Prefix + "/Paginated";
        }

        public static class DepartmentRouting
        {
            public const string Prefix = Rule + "Departmetn";
            public const string GetById = Prefix + "/Id";
        }

        public static class ApplicationUserRouting
        {
            public const string Prefix = Rule + "User";
            public const string Create = Prefix + "/Create";
            public const string List = Prefix + "/List";
            public const string GetById = Prefix + SingleRoute;
            public const string Edit = Prefix + "/Edit";
            public const string Delete = Prefix + "/Delete" + SingleRoute;
            public const string ChangePassword = Prefix + "/Change-Password";
        }

        public static class AuthenticationRouting
        {
            public const string Prefix = Rule + "Authentication";
            public const string SignIn = Prefix + "/SignIn";
            public const string RefreshToken = Prefix + "/Refresh-Token";
            public const string ValidateToken = Prefix + "/Validate-Token";
        }
    }
}
