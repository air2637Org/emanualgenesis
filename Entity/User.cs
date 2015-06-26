using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MaintenanceContractFormsMgmt.Entity
{
    public class User
    {
        public const string DB_TABLE_NAME = "user_account";
        public const string DB_ID = "id";
        public const string User_NAME = "username";
        public const string PASSWORD = "pwd";
        public const string SALT = "salt";
        public const string ROLE_FLG = "role_flag";
        public const string SESSION_ID = "session_id";

        public int id;
        public string userName;

        public static User Parse(Dictionary<string, string> databaseSelectDictionary)
        {
            var user = new User();
            if (databaseSelectDictionary.ContainsKey(DB_ID))
            {
                user.id = Int32.Parse(databaseSelectDictionary[DB_ID]);
            }
            if (databaseSelectDictionary.ContainsKey(User_NAME))
            {
                user.userName = databaseSelectDictionary[User_NAME];
            }
            return user;
        }
    }
}
