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
            public const string ConfirmEmail = "/Api/Authentication/ConfirmEmail";
        }

        public static class AuthorizationRouting
        {
            public const string Prefix = Rule + "Authorization";
            public const string Roles = Prefix + "/Roles";
            public const string Claims = Prefix + "/Claims";

            public const string Create = Roles + "/Create";
            public const string Edit = Roles + "/Edit";
            public const string Delete = Roles + "/Delete" + SingleRoute;
            public const string RolesList = Roles + "/Roles-List";
            public const string RoleById = Roles + "/Role-By-Id" + SingleRoute;
            public const string GetUserRoles = Roles + "/Get-User-Roles" + "/{userId}";
            public const string UpdateUserRoles = Roles + "/Update-User-Roles";

            public const string GetUserClaims = Claims + "/Get-User-Claims" + "/{userId}";
            public const string UpdateUserClaims = Claims + "/Update-User-Claims";

        }

        public static class EmailsRouting
        {
            public const string Prefix = Rule + "EmailsRoute";
            public const string SendEmail = Prefix + "/SendEmail";
        }
    }

}
