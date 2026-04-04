using CNCEmu.Constants;
using CNCEmu.DTOs;
using CNCEmu.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;

namespace CNCEmu.Services
{
    public class ProfileService
    {
        public const string ProfileFoler = "Backend\\Profiles";
        public const string ProfileFileExtension = "profile";

        /// <summary>
        /// Singleton instance
        /// </summary>
        public static ProfileService Instance { get; } = new ProfileService();

        private readonly ObservableCollection<Account> Profiles;

        [Obsolete]
        public Account ServerProfile { get; private set; }

        private ProfileService()
        {
            LoadProfiles().ForEach(p => Profiles.Add(p));
            Profiles.CollectionChanged += Profiles_CollectionChanged;

            // Update id counter
            Account.IdCounter = Profiles.Max(p => p.Id_);

            // Get server profile
            ServerProfile = GetProfileByName(General.ServerAccountName) ?? 
                AddNew(General.ServerAccountName, General.ServerAccountEmail);
        }

        private void Profiles_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            
        }

        /// <summary>
        /// Load all profiles from ProfileFoler files
        /// </summary>
        /// <returns></returns>
        private List<Account> LoadProfiles()
        {
            // Make sure profile folder exists
            Directory.CreateDirectory(ProfileFoler);

            var profiles = new List<Account>();
            // Get all profile files
            foreach (var file in Directory.GetFiles(ProfileFoler, $"*.{ProfileFileExtension}"))
            {
                try
                {
                    var profile = JsonConvert.DeserializeObject<AccountDto>(File.ReadAllText(file));
                    profiles.Add(new Account(profile));
                }
                catch (Exception ex) 
                {
                    // todo
                }
            }

            return profiles;
        }

        /// <summary>
        /// Get all profiles
        /// </summary>
        /// <returns></returns>
        public List<Account> GetAll() =>
            new List<Account>(Profiles);

        /// <summary>
        /// Get profile by email or null
        /// </summary>
        /// <param name="email"></param>
        /// <returns></returns>
        public Account GetProfileByEmail(string email) =>
            Profiles.FirstOrDefault(p => p.Email.Equals(email, StringComparison.OrdinalIgnoreCase));

        /// <summary>
        /// Get profile by name or null
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public Account GetProfileByName(string name) =>
            Profiles.FirstOrDefault(p => p.UserName.Equals(name, StringComparison.OrdinalIgnoreCase));

        /// <summary>
        /// Add new profile
        /// </summary>
        /// <param name="name"></param>
        /// <param name="email"></param>
        /// <returns></returns>
        /// <exception cref="Exception">name or email already exists, or name or email is null</exception>
        public Account AddNew(string name, string email)
        {
            if (GetProfileByName(name) != null)
                throw new Exception("Profile name already exists");

            if (GetProfileByEmail(email) != null)
                throw new Exception("Profile email already exists");

            var profile = new Account(name, email);

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
        public void Remove(Account profile)
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
        [Obsolete]
        public void RemoveAt(int index)
        {
            if (index < 0 || index >= Profiles.Count)
                throw new ArgumentOutOfRangeException(nameof(index));

            var profile = Profiles[index];
            Remove(profile);
        }
    }
}
