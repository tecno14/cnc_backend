using CNCEmu.Constants;
using CNCEmu.DTOs;
using System;
using System.IO;

namespace CNCEmu.Models
{
    public class Account
    {
        public static long IdCounter { get; set; } = 1000;

        [Obsolete]
        public long Id_ { get; private set; }

        public Guid Id { get; set; } = Guid.NewGuid();

        public string DisplayName { get; private set; }
        
        public string UserName { get; private set; }

        public string Email { get; private set; }

        public string PasswordHash { get; set; }

        public Account(AccountDto dto) : this(dto.Name, dto.Email)
        {
            Id_ = dto.Id;
        }

        public Account(string name, string email)
        {
            if (string.IsNullOrEmpty(name))
                throw new ArgumentNullException(nameof(name));

            if (string.IsNullOrEmpty(email))
                throw new ArgumentNullException(nameof(email));

            Id_ = ++IdCounter;
            UserName = name;
            Email = email;
        }

        public bool IsVailable() =>
            Id_ > 0 && !string.IsNullOrEmpty(UserName) && !string.IsNullOrEmpty(Email);

        public override string ToString() =>
            $"Id = {Id_}, Name = {UserName}, Mail = {Email}";

        public string GetFileName() =>
            Path.Combine(General.ProfileFoler, $"{Id_:X8}.{General.ProfileFileExtension}");
    }
}
