using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.RazorPages;



namespace PwnedPassword.Models
{
    public class PwnedPasswordsModel
    {
        public string Password { get; set; }
        public string Message { get; set; }
        public long Frequency { get; set; }
        public string Name { get; set; }
        public bool IsPwned { get; set; }

        public void Copy(PwnedPasswordsModel model)
        {
            if (model == null)
                return;

            Password = model.Password;
            Message = model.Message;
            Frequency = model.Frequency;
            Name = model.Name;
            IsPwned = model.IsPwned;
        }
    }
}
