namespace SchoolProject.Data.Responses
{
    public class GetUserRoleResponse
    {
        public int UserId { get; set; }
        public List<UserRoles> UserRoles { get; set; }
    }

    public class UserRoles
    {
        public int RoleId { get; set; }
        public string RoleName { get; set; }
        public bool IsInRole { get; set; }
    }
}