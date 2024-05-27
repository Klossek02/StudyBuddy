﻿namespace StudyBuddy.Models
{
    public class AuthResponse
    {
        public string AccessToken { get; set; }
        public string RefreshToken { get; set; }
        public DateTime ExpiresIn { get; set; }
        public string Role { get; set; }
        public int UserId { get; set; }
    }
}