namespace SchoolProject.Data.Responses
{
    public class GetUserClaimsResponse
    {
        public int UserId { get; set; }
        public List<UserClaims> UserClaims { get; set; }
    }
    public class UserClaims
    {
        public string ClaimType { get; set; }
        public bool ClaimValue { get; set; }
    }
}