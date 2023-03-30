using Microsoft.AspNetCore.Builder;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using SkillMatrix.Domain.Models;
using SkillMatrix.service.Data;
using SkillMatrix.service.Dto;
using System;
using System.Collections.Generic;
using System.Data;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace SkillMatrix.service.Services
{
    public class UserService
    {
        private readonly ApplicationDbContext _db;
        private readonly IConfiguration _configuration;
        

        public UserService(ApplicationDbContext db,
                           IConfiguration configuration)
        {
            _db = db;
            _configuration = configuration;
        }

        public async Task<Array> GetSkillsList()
        {
            var arr = _db.SkillsMaster.Where(m => m.SkillStatus == 1).Select(m => new SkillsMaster()
            {
                Id= m.Id,
                SkillName= m.SkillName,
                SkillDescription= m.SkillDescription,
                SkillStatus= m.SkillStatus,
            }).ToArray();
            return arr;
        }

        public async Task<Array> GetSkillsListForApprove()
        {
            var arr = _db.SkillsMaster.Where(m => m.SkillStatus == 0).Select(m => new SkillsMaster()
            {
                Id = m.Id,
                SkillName = m.SkillName,
                SkillDescription = m.SkillDescription,
                SkillStatus = m.SkillStatus,
            }).ToArray();
            return arr;
        }

        public async Task<String> Login(LoginDto dto)
        {
            var token = "Not Found";
            var p = Convert.ToBase64String(Encoding.UTF8.GetBytes(dto.Password));
            Console.WriteLine("------------------" + p);
            var user = _db.Users.Where(m => m.Email == dto.Email &&
            m.Password == p).FirstOrDefault();
            if(user != null && user.Name != "Admin")
            {
                token = GenerateToken(user, "Employee");
            }
            else if(user != null && user.Name == "Admin")
            {
                token = GenerateToken(user, "Admin");
            }
            return token;
        }

        public async Task<bool> Register(RegisterDto dto)
        {
            try
            {
                var item = _db.Users.Where(m => m.Email == dto.Email).FirstOrDefault();
                if(item != null)
                {
                    return false;
                }
                var p = Convert.ToBase64String(Encoding.UTF8.GetBytes(dto.Password));
                Console.WriteLine("------------" + p);
                Console.WriteLine(Encoding.UTF8.GetString(Convert.FromBase64String(p)));
                _db.Users.Add(new Users()
                {
                    Name = dto.Name,
                    Email = dto.Email,
                    Password = p.ToString(),
                    PhoneNumber = dto.Phone
                });
                _db.SaveChanges();
                return true;
            }catch(Exception ex)
            {
                return false;
            }
        }

        public string GenerateToken(Users user, string role)
        {
            string key = _configuration["Jwt:Key"];
            string issuer = _configuration["Jwt:Issuer"];
            string audience = _configuration["Jwt:Audience"];

            var signingKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key));
            var credentials = new SigningCredentials(signingKey, "HS256");
            List<int> master = null;

            var claims = new Claim[]
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                //new Claim(ClaimTypes.NameIdentifier, user.Id),
                new Claim(ClaimTypes.Name, $"{user.Name}"),
                new Claim(ClaimTypes.Role, role),
                new Claim("Role", role),
                new Claim("UserId", user.Id.ToString()),
            };

            var token = new JwtSecurityToken(
                issuer: issuer,
                claims: claims,
                audience: audience,
                expires: DateTime.UtcNow + TimeSpan.FromDays(1),
                signingCredentials: credentials
                );
            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        public async Task<bool> DeleteAdditionalSkill(int id)
        {
            try
            {
                _db.EmployeSkills.Remove(_db.EmployeSkills.FirstOrDefault(m => m.id == id));
                _db.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {

                return false;
            }
        }

        public async Task<bool> UpdateSkillByAdmin(int id,AddSkillsDto dto)
        {
            try
            {
                var items = _db.SkillsMaster.FirstOrDefault(m => m.SkillName == dto.SkillName.ToUpper());
                if (items != null)
                {
                    return false;
                }
                //Data Source=RASHIDKOTTAKUND\\SQLEXPRESS;Initial Catalog=SkillMatrix;Integrated Security=SSPI
                //string ConnectionString = @"Server=RASHIDKOTTAKUND\\SQLEXPRESS;Database=SkillMatrix;Integrated Security=true;TrustServerCertificate = true";
                string ConnectionString = @"Data Source=RASHIDKOTTAKUND\SQLEXPRESS;Initial Catalog=SkillMatrix;Integrated Security=true;TrustServerCertificate = true";

                using (SqlConnection connection = new SqlConnection(ConnectionString))
                {
                    //Create the SqlCommand object
                    SqlCommand cmd = new SqlCommand()
                    {
                        CommandText = "spUpdateSkillByAdmin", //Specify the Stored procedure name
                        Connection = connection, //Specify the connection object where the stored procedure is going to execute
                        CommandType = CommandType.StoredProcedure, //Specify the command type as Stored Procedure
                   
                    };
                    //Create an instance of SqlParameter
                    SqlParameter param1 = new SqlParameter
                    {
                        ParameterName = "@Id", //Parameter name defined in stored procedure
                        SqlDbType = SqlDbType.Int, //Data Type of Parameter
                        Value = id, //Value passes to the paramtere
                        Direction = ParameterDirection.Input //Specify the parameter as input
                    };
                    SqlParameter param2 = new SqlParameter
                    {
                        ParameterName = "@SkillName", //Parameter name defined in stored procedure
                        SqlDbType = SqlDbType.VarChar, //Data Type of Parameter
                        Value = dto.SkillName.ToUpper(), //Value passes to the paramtere
                        Direction = ParameterDirection.Input //Specify the parameter as input
                    };
                    SqlParameter param3 = new SqlParameter
                    {
                        ParameterName = "@SkillDescription", //Parameter name defined in stored procedure
                        SqlDbType = SqlDbType.VarChar, //Data Type of Parameter
                        Value = dto.SkillDescription, //Value passes to the paramtere
                        Direction = ParameterDirection.Input //Specify the parameter as input
                    };
                    //Add the parameter to the Parameters property of SqlCommand object
                    cmd.Parameters.Add(param1);
                    cmd.Parameters.Add(param2);
                    cmd.Parameters.Add(param3);
                    //Open the Connection
                    connection.Open();
                    //Execute the command i.e. Executing the Stored Procedure using ExecuteReader method
                    //SqlDataReader requires an active and open connection
                    var res = cmd.ExecuteNonQuery();
                    //var res = _db.SkillsMaster.FirstOrDefault(m => m.Id== id);
                    //res.SkillName = dto.SkillName.ToUpper();
                    //res.SkillDescription = dto.SkillDescription;
                    //_db.SaveChanges();
                    //FormattableString sqlQuery = $"exec spUpdateSkillByAdmin {id} {dto.SkillName} {dto.SkillDescription}";
                    //var res = _db.Database.ExecuteSql(sqlQuery);
                    ////var res = _db.Set<SkillsMaster>().FromSql(sqlQuery);
                    //var v = _db.SkillsMaster.FromSql<SkillsMaster>(sqlQuery);
                    //Console.WriteLine("--------------------"+v);
                    Console.WriteLine("---------"+res);
                    return true;
                }
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public async Task<SkillsMaster> GetSkillById(int id)
        {
            try
            {
                var item = _db.SkillsMaster.FirstOrDefault(m => m.Id == id);
                return item;
            }
            catch (Exception ex)
            {
                var item = new SkillsMaster();
                return item;
            }
        }

    }
}
