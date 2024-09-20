using CNCEmu.Constants;
using CNCEmu.DTOs;
using System;
using System.IO;

namespace CNCEmu.Models
{
    public class Profile
    {
        public static long IdCounter { get; set; } = 1000;

        public long Id { get; private set; }

        public string Name { get; private set; }

        public string Email { get; private set; }

        public Profile(ProfileDto dto) : this(dto.Name, dto.Email)
        {
            Id = dto.Id;
        }

        public Profile(string name, string email)
        {
            if (string.IsNullOrEmpty(name))
                throw new ArgumentNullException(nameof(name));

            if (string.IsNullOrEmpty(email))
                throw new ArgumentNullException(nameof(email));

            Id = ++IdCounter;
            Name = name;
            Email = email;
        }

        public bool IsVailable() =>
            Id > 0 && !string.IsNullOrEmpty(Name) && !string.IsNullOrEmpty(Email);

        public override string ToString() =>
            $"Id = {Id}, Name = {Name}, Mail = {Email}";

        public string GetFileName() =>
            Path.Combine(General.ProfileFoler, $"{Id:X8}.{General.ProfileFileExtension}");
    }
}
