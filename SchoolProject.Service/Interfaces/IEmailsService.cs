﻿namespace SchoolProject.Service.Interfaces
{
    public interface IEmailsService
    {
        public Task<string> SendEmail(string email, string Message, string? reason);
    }
}
