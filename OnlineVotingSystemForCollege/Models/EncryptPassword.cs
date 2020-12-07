﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace OnlineVotingSystemForCollege.Models
{
    public class EncryptPassword
    {
        public static string textToEncrypt(string Password)
        {
            return Convert.ToBase64String(System.Security.Cryptography.SHA256.Create().ComputeHash(Encoding.UTF8.GetBytes(Password)));
        }
    }
}