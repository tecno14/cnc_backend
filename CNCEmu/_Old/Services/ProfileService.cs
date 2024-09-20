using CNCEmu.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Newtonsoft.Json;
using CNCEmu.Constants;

namespace CNCEmu.Services
{
    public class ProfileService
    {
        /// <summary>
        /// Singleton instance
        /// </summary>
        public static ProfileService Instance { get; private set; } = new ProfileService();

        private readonly List<Profile> Profiles;

        public Profile ServerProfile { get; private set; }

        private ProfileService()
        {
            Profiles = LoadProfiles();

            // Update id counter
            Profile.IdCounter = Profiles.Max(p => p.Id);

            // Get server profile
            ServerProfile = GetProfileByName(General.ServerAccountName) ?? 
                Add(General.ServerAccountName, General.ServerAccountEmail);
        }

        /// <summary>
        /// Load all profiles from ProfileFoler files
        /// </summary>
        /// <returns></returns>
        private List<Profile> LoadProfiles()
        {
            // Make sure profile folder exists
            Directory.CreateDirectory(General.ProfileFoler);

            // Get all profile files
            var files = Directory.GetFiles(General.ProfileFoler, $"*.{General.ProfileFileExtension}");

            var profiles = new List<Profile>();
            foreach (var file in files)
            {
                var profile = JsonConvert.DeserializeObject<ProfileDto>(File.ReadAllText(file));
                if (profile != null)
                    profiles.Add(new Profile(profile));
            }

            return profiles;
        }

        /// <summary>
        /// Get all profiles
        /// </summary>
        /// <returns></returns>
        public List<Profile> GetAll() =>
            new List<Profile>(Profiles);

        /// <summary>
        /// Get profile by email or null
        /// </summary>
        /// <param name="email"></param>
        /// <returns></returns>
        public Profile GetProfileByEmail(string email) =>
            Profiles.FirstOrDefault(p => p.Email.Equals(email, StringComparison.OrdinalIgnoreCase));

        /// <summary>
        /// Get profile by name or null
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public Profile GetProfileByName(string name) =>
            Profiles.FirstOrDefault(p => p.Name.Equals(name, StringComparison.OrdinalIgnoreCase));

        /// <summary>
        /// Add new profile
        /// </summary>
        /// <param name="name"></param>
        /// <param name="email"></param>
        /// <returns></returns>
        /// <exception cref="Exception">name or email already exists, or name or email is null</exception>
        public Profile Add(string name, string email)
        {
            if (GetProfileByName(name) != null)
                throw new Exception("Profile name already exists");

            if (GetProfileByEmail(email) != null)
                throw new Exception("Profile email already exists");

            var profile = new Profile(name, email);

            // Save profile file
            File.WriteAllText(profile.GetFileName(), JsonConvert.SerializeObject(profile));

            // Add to list
            Profiles.Add(profile);

            return profile;
        }

        /// <summary>
        /// Remove profile
        /// </summary>
        /// <param name="profile"></param>
        /// <exception cref="Exception"></exception>
        public void Remove(Profile profile)
        {
            if (!Profiles.Contains(profile))
                throw new Exception("Profile not found");

            // Delete profile file
            File.Delete(profile.GetFileName());

            // Remove from list
            Profiles.Remove(profile);
        }

        /// <summary>
        /// Remove profile by index
        /// </summary>
        /// <param name="index"></param>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        public void RemoveAt(int index)
        {
            if (index < 0 || index >= Profiles.Count)
                throw new ArgumentOutOfRangeException(nameof(index));

            var profile = Profiles[index];
            Remove(profile);
        }
    }
}
