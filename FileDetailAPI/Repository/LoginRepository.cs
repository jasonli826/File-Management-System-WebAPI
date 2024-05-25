using FileDetailAPI.Models;
using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace FileDetailAPI.Repository
{
    public interface ILoginRepository
    {
        Task<UserInfo> Login(string userId,string password);
    }
    public class LoginRepository : ILoginRepository
    {
        private readonly APIDbContext _appDBContext;
        private string key = "b14ca5898a4e4133bbce2ea2315a1916";
        public LoginRepository(APIDbContext context)
        {
            _appDBContext = context ?? throw new ArgumentNullException(nameof(context));
        }
        public async Task<UserInfo> Login(string userId, string password)
        {
            UserInfo userInfo = null;
            string encryption = string.Empty;
            try
            {
                encryption = EncryptString(key, password);

                if (_appDBContext.User_tbl.Where(x => x.UserId == userId && x.Password == encryption ).Count() > 0)
                {
                    var singleuser = await _appDBContext.User_tbl.Where(x => x.UserId.ToLower() == userId.ToLower() && x.Password == encryption).FirstOrDefaultAsync();
                    if (singleuser != null)
                    {
                        if (singleuser.Status.ToUpper() == "INACTIVE")
                        {
                            userInfo = new UserInfo("Inactive User", new List<UserInfo.Menu>());
                            return userInfo;
                        }


                        singleuser.Last_Login_Date = DateTime.Now;

                        await _appDBContext.SaveChangesAsync();

                    }
                    var tempList = await (from user in _appDBContext.User_tbl
                                          join role in _appDBContext.UserRole on user.UserId equals role.UserId
                                          join roleControl in _appDBContext.RoleControl on role.RoleId equals roleControl.RoleId
                                          join menu in _appDBContext.MenuItems on roleControl.MenuId equals menu.MenuID
                                          where user.Status.ToUpper() == "ACTIVE" && user.UserId.ToUpper() == userId.ToUpper() && user.Password == encryption
                                          select new
                                          {
                                              userId = user.UserId,
                                              menuId = menu.MenuID,
                                              menudesc = menu.Menu_Description

                                          }).ToListAsync();
                    if (tempList.Count() > 0)
                    {
                        userInfo = new UserInfo(userId, new List<UserInfo.Menu>());
                        foreach (var item in tempList)
                        {

                            userInfo.accessList.Add(new UserInfo.Menu(item.menuId, item.menudesc));

                        }

                    }
                    else
                    {
                        userInfo = new UserInfo(userId, new List<UserInfo.Menu>());
                    }
                }
                else
                {
                    userInfo = new UserInfo("0", null);


                }
                
            }
            catch (Exception ex)
            {
                throw ex;

            }
            return userInfo;

            


        }
        private string EncryptString(string key, string plainText)
        {
            byte[] iv = new byte[16];
            byte[] array = null;
            try
            {
                using (Aes aes = Aes.Create())
                {
                    aes.Key = Encoding.UTF8.GetBytes(key);
                    aes.IV = iv;

                    ICryptoTransform encryptor = aes.CreateEncryptor(aes.Key, aes.IV);

                    using (MemoryStream memoryStream = new MemoryStream())
                    {
                        using (CryptoStream cryptoStream = new CryptoStream((Stream)memoryStream, encryptor, CryptoStreamMode.Write))
                        {
                            using (StreamWriter streamWriter = new StreamWriter((Stream)cryptoStream))
                            {
                                streamWriter.Write(plainText);
                            }

                            array = memoryStream.ToArray();
                        }
                    }
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }

            return Convert.ToBase64String(array);
        }
    }
}
