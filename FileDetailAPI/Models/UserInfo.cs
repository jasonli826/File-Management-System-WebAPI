using System.Collections.Generic;

namespace FileDetailAPI.Models
{
    public class UserInfo
    {
        public string userId { get; set; }

        public List<Menu> accessList { get; set; }

        public UserInfo(string Id, List<Menu> list)
        {
            userId = Id;
            accessList = list;
        }
        public class Menu
        {
            public int MenuID { get; set; }

            public string Menu_Description { get; set; }

            public Menu(int Id, string desc)
            {
                MenuID = Id;
                Menu_Description = desc;
            
            }
        }
    }
}
