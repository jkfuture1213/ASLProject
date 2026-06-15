using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Windows.Forms;

namespace SmartAccountBook
{
    internal static class UsersStore
    {
        [DataContract]
        private class UserRecord
        {
            [DataMember]
            public string Username { get; set; }
            [DataMember]
            public string Password { get; set; }
        }

        private static string FilePath => Path.Combine(Application.StartupPath, "users", "users.json");

        private static List<UserRecord> LoadAll()
        {
            try
            {
                var dir = Path.GetDirectoryName(FilePath);
                if (!Directory.Exists(dir)) Directory.CreateDirectory(dir);
                if (!File.Exists(FilePath)) return new List<UserRecord>();
                using (var fs = File.OpenRead(FilePath))
                {
                    var ser = new DataContractJsonSerializer(typeof(List<UserRecord>));
                    return ser.ReadObject(fs) as List<UserRecord> ?? new List<UserRecord>();
                }
            }
            catch { return new List<UserRecord>(); }
        }

        private static void SaveAll(List<UserRecord> list)
        {
            try
            {
                var dir = Path.GetDirectoryName(FilePath);
                if (!Directory.Exists(dir)) Directory.CreateDirectory(dir);
                using (var fs = File.Create(FilePath))
                {
                    var ser = new DataContractJsonSerializer(typeof(List<UserRecord>));
                    ser.WriteObject(fs, list);
                }
            }
            catch { }
        }

        public static bool AddUser(string username, string password)
        {
            username = (username ?? "").Trim();
            password = password ?? "";
            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password)) return false;

            var list = LoadAll();
            if (list.Exists(u => u.Username.Equals(username, StringComparison.OrdinalIgnoreCase))) return false;
            list.Add(new UserRecord { Username = username, Password = password });
            SaveAll(list);
            return true;
        }

        public static bool Validate(string username, string password)
        {
            username = (username ?? "").Trim();
            password = password ?? "";
            var list = LoadAll();
            return list.Exists(u => u.Username.Equals(username, StringComparison.OrdinalIgnoreCase) && (u.Password ?? "") == password);
        }

        public static string GetPassword(string username)
        {
            username = (username ?? "").Trim();
            if (string.IsNullOrEmpty(username)) return null;
            var list = LoadAll();
            var user = list.Find(u => u.Username.Equals(username, StringComparison.OrdinalIgnoreCase));
            return user != null ? user.Password : null;
        }

        public static bool UpdatePassword(string username, string newPassword)
        {
            username = (username ?? "").Trim();
            newPassword = newPassword ?? "";
            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(newPassword)) return false;

            var list = LoadAll();
            var user = list.Find(u => u.Username.Equals(username, StringComparison.OrdinalIgnoreCase));
            if (user == null) return false;
            user.Password = newPassword;
            SaveAll(list);
            return true;
        }
    }
}
