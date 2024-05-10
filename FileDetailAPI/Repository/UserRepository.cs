using FileDetailAPI.Models;
using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;
using System.Collections;
using System.Net;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion.Internal;

namespace FileDetailAPI.Repository
{
    public interface IUserRepository {

        Task<IEnumerable<User_DTO>> GetUsers();
        Task<User_tbl> GetUserByID(string userId);
        Task<User_tbl> InsertUser(User_DTO user_dto);
        Task<User_tbl> UpdateUser(User_DTO user_dto );

        Task<IEnumerable<User_DTO>> SearchUsers(User_DTO user_dto);
    }
    public class UserRepository : IUserRepository
    {
        private readonly APIDbContext _appDBContext;
        private string key = "b14ca5898a4e4133bbce2ea2315a1916";
        public UserRepository(APIDbContext context)
        {
            _appDBContext = context ?? throw new ArgumentNullException(nameof(context));
        }
        public async Task<User_tbl> GetUserByID(string userId)
        {
            User_tbl user =  await _appDBContext.User_tbl.Where(x=>x.UserId==userId).FirstOrDefaultAsync();
            return user;
        }


        public async Task<IEnumerable<User_DTO>> GetUsers()
        {
            IEnumerable<User_DTO> userList = null;
            try
            {
               

                userList = await (from user in _appDBContext.User_tbl
                                  select new User_DTO
                                  {
                                      UserId = user.UserId,
                                      UserName = user.UserName,
                                      Password = user.Password,
                                      isActive = (user.Status == "Active" ? true : false),
                                      RoleIds = _appDBContext.UserRole.Where(x => x.UserId == user.UserId).Select(u => u.RoleId).ToList(),
                                      Created_by = user.Created_by,
                                      Created_Date = user.Created_Date,
                                      Update_by = user.Updated_by,
                                      Update_Date = user.Updated_Date,
                                      Remark = user.Remarks
                                     

                                  }).ToListAsync();

                foreach (var user in userList)
                {
                    user.Password = DecryptString(key,user.Password);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            
            }
            return userList;
        }
        public async Task<IEnumerable<User_DTO>> SearchUsers(User_DTO user_dto)
        {
            IEnumerable<User_DTO> userList = null;
            string userId = user_dto.UserId;
            string userName = user_dto.UserName;
            string remark = user_dto.Remark;
            List<int> roleIds = user_dto.RoleIds;
            try
            {
                if (roleIds.Count==0)
                {

                    userList = await (from user in _appDBContext.User_tbl
                                      join role in _appDBContext.UserRole on user.UserId equals role.UserId
                                      where ((user.UserId.ToUpper() ?? "").Contains(userId.ToUpper()) &&(user.UserName.ToUpper() ?? "").Contains(userName.ToUpper()) && (user.Remarks.ToUpper() ?? "").Contains(remark.ToUpper()))
                                      select new User_DTO
                                      {
                                          UserId = user.UserId,
                                          UserName = user.UserName,
                                          Password = user.Password,
                                          isActive = (user.Status == "Active" ? true : false),
                                          RoleIds = _appDBContext.UserRole.Where(x => x.UserId == user.UserId).Select(u => u.RoleId).ToList(),
                                          Created_by = user.Created_by,
                                          Created_Date = user.Created_Date,
                                          Update_by = user.Updated_by,
                                          Update_Date = user.Updated_Date,
                                          Remark = user.Remarks
                                       

                                      }).ToListAsync();


                }
                else
                {
                    userList = await (from user in _appDBContext.User_tbl
                                      join role in _appDBContext.UserRole on user.UserId equals role.UserId
                                      where ((user.UserId.ToUpper() ?? "").Contains(userId.ToUpper()) && (user.UserName.ToUpper() ?? "").Contains(userName.ToUpper()) && (user.Remarks.ToUpper() ?? "").Contains(remark.ToUpper())) && (roleIds).Contains(role.RoleId)
                                      select new User_DTO
                                      {
                                          UserId = user.UserId,
                                          UserName = user.UserName,
                                          Password = user.Password,
                                          isActive = (user.Status == "Active" ? true : false),
                                          RoleIds = _appDBContext.UserRole.Where(x => x.UserId == user.UserId).Select(u => u.RoleId).ToList(),
                                          Created_by = user.Created_by,
                                          Created_Date = user.Created_Date,
                                          Update_by = user.Updated_by,
                                          Update_Date = user.Updated_Date,
                                          Remark = user.Remarks,
                                      

                                      }).ToListAsync();

                
                }

                foreach (var user in userList)
                {
                    user.Password = DecryptString(key, user.Password);
                }
            }
            catch (Exception ex)
            {
                throw ex;

            }
            return userList;
        }
        public async Task<User_tbl> InsertUser(User_DTO user_dto)
        {
            User_tbl user = new User_tbl();
            try
            {
             
                string encrptionStr = EncryptString(key, user_dto.Password);
                if (_appDBContext.User_tbl.Where(x => x.UserId == user_dto.UserId).Count() > 0)
                {
                    user.UserId = "0";
                    return user;
                }
                user.UserId = user_dto.UserId;
                user.UserName = user_dto.UserName;
                user.Password = encrptionStr;
                user.Status = (user_dto.isActive == true ? "Active" : "Inactive");
            
                user.Remarks = user_dto.Remark;
                user.Created_by = user_dto.Created_by;
                user.Created_Date = DateTime.Now;
                user.Updated_by = user_dto.Created_by;
                user.Updated_Date = DateTime.Now;
                user.Invalidlogin = 0;
                user.Logout_Date = DateTime.Now;
                user.Last_Login_Date = DateTime.Now;
                user.Acc_Lock = 0;
                await _appDBContext.User_tbl.AddAsync(user);
                await _appDBContext.SaveChangesAsync();
                List<UserRole> userRoleList = new List<UserRole>();

                foreach (int roleId in user_dto.RoleIds)
                {
                    UserRole userRole = new UserRole();
                    userRole.RoleId = roleId;
                    userRole.UserId = user_dto.UserId;
                    await _appDBContext.UserRole.AddAsync(userRole);
                    await _appDBContext.SaveChangesAsync();
                }
              
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return user;
        }

        public async Task<User_tbl> UpdateUser(User_DTO user_dto)
        {
            User_tbl user = null;
            string encrptionStr = string.Empty;
            try
            {
                user = await _appDBContext.User_tbl.Where(x => x.UserId == user_dto.UserId).FirstOrDefaultAsync();
                user.UserName = user_dto.UserName;
                if (!string.IsNullOrEmpty(user_dto.Password))
                {
                    encrptionStr = EncryptString(key, user_dto.Password);

                    user.Password = encrptionStr;
                }
                user.Status = (user_dto.isActive == true ? "Active" : "Inactive");
                user.Remarks = user_dto.Remark;
                user.Updated_by = user_dto.Update_by;
                user.Updated_Date = DateTime.Now;
                _appDBContext.Entry(user).State = Microsoft.EntityFrameworkCore.EntityState.Modified;

                await _appDBContext.SaveChangesAsync();
                var role = await _appDBContext.UserRole.Where(x => x.UserId == user_dto.UserId).ToListAsync();

                _appDBContext.UserRole.RemoveRange(role);
                foreach (int roleId in user_dto.RoleIds)
                {
                    UserRole userRole = new UserRole();
                    userRole.RoleId = roleId;
                    userRole.UserId = user_dto.UserId;
                    await _appDBContext.UserRole.AddAsync(userRole);
                    await _appDBContext.SaveChangesAsync();
                }
            }
            catch (Exception ex)
            {
                return null;
            }
            return user;


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

        private string DecryptString(string key, string cipherText)
        {
            byte[] iv = new byte[16];
            byte[] buffer = Convert.FromBase64String(cipherText);

            using (Aes aes = Aes.Create())
            {
                aes.Key = Encoding.UTF8.GetBytes(key);
                aes.IV = iv;
                ICryptoTransform decryptor = aes.CreateDecryptor(aes.Key, aes.IV);

                using (MemoryStream memoryStream = new MemoryStream(buffer))
                {
                    using (CryptoStream cryptoStream = new CryptoStream((Stream)memoryStream, decryptor, CryptoStreamMode.Read))
                    {
                        using (StreamReader streamReader = new StreamReader((Stream)cryptoStream))
                        {
                            return streamReader.ReadToEnd();
                        }
                    }
                }
            }
        }
    }
}
