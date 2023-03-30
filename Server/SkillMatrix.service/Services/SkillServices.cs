using Microsoft.EntityFrameworkCore;
using SkillMatrix.Domain.Models;
using SkillMatrix.service.Data;
using SkillMatrix.service.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace SkillMatrix.service.Services
{
    public class SkillServices
    {
        public enum SkillType
        {
            Primary,
            Secondary,
            Additional
        };

        private readonly ApplicationDbContext _db;

        public SkillServices(ApplicationDbContext db)
        {
            _db = db;
        }

        public async Task<SkillUpdateViewDto> GetUserSkills(int id)
        {
            try
            {
                var res1 = _db.EmployeSkills.Where(m => m.UsersId == id && m.SkillType == Convert.ToInt32(SkillType.Primary)).FirstOrDefault();
                var res2 = _db.EmployeSkills.Where(m => m.UsersId == id && m.SkillType == Convert.ToInt32(SkillType.Secondary)).FirstOrDefault();
                var additional = _db.EmployeSkills.Where(m => m.UsersId == id && m.SkillType == Convert.ToInt32(SkillType.Additional)).Select(m => new AdditionalSkillsViewDto()
                {
                    SelectSkillId = m.id,
                    SelectSkill = m.SkillsMasterId,
                    SelectRating = m.SkillRating,
                    SkillName = m.SkillsMaster.SkillName
                }).ToList();

                var response = new SkillUpdateViewDto()
                {
                    PrimarySkillId = res1.id,
                    PrimarySkill = res1.SkillsMasterId,
                    PrimaryRating = res1.SkillRating,
                    SecondarySkillId = res2.id,
                    SecondarySkill = res2.SkillsMasterId,
                    SecondaryRating = res2.SkillRating,
                    AdditionalSkills= additional
                };

                return response;
            } catch(Exception ex)
            {
                var ress = new SkillUpdateViewDto();
                return ress;
            }
        }

        public async Task<bool> UpdateSkills(int userId,SkillUpdateDto dto)
        {
            try
            {
                if(dto.PrimarySkillId == 0 && dto.SecondarySkillId == 0)
                {
                    var primaryskill = new EmployeSkills()
                    {
                        SkillsMasterId= dto.PrimarySkill,
                        SkillRating= dto.PrimaryRating,
                        UsersId= userId,
                        SkillType = Convert.ToInt32(SkillType.Primary),
                    };

                    var secondaryskill = new EmployeSkills()
                    {
                        SkillsMasterId = dto.SecondarySkill,
                        SkillRating = dto.SecondaryRating,
                        UsersId = userId,
                        SkillType =  Convert.ToInt32(SkillType.Secondary),
                    };
                    await _db.EmployeSkills.AddAsync(primaryskill);
                    await _db.EmployeSkills.AddAsync(secondaryskill);
                }
                else if(dto.PrimarySkillId != 0 && dto.SecondarySkillId != 0)
                {
                    var primaryskill = _db.EmployeSkills.Where(m => m.id == dto.PrimarySkillId).FirstOrDefault();
                    primaryskill.SkillsMasterId = dto.PrimarySkill;
                    primaryskill.SkillRating = dto.PrimaryRating;

                    var secondaryskill = _db.EmployeSkills.Where(m => m.id == dto.SecondarySkillId).FirstOrDefault();
                    secondaryskill.SkillsMasterId= dto.SecondarySkill;
                    secondaryskill.SkillRating = dto.SecondaryRating;
                }
                else
                {
                    return false;
                }

                foreach (var item in dto.AdditionalSkills)
                {
                    if (item.SelectSkillId == 0)
                    {
                        _db.EmployeSkills.Add(new EmployeSkills()
                        {
                            SkillsMasterId = item.SelectSkill,
                            SkillRating = item.SelectRating,
                            UsersId = userId,
                            SkillType = Convert.ToInt32(SkillType.Additional)
                        });
                    }
                   else
                    {
                        var result = _db.EmployeSkills.Where(m => m.id == item.SelectSkillId).FirstOrDefault();
                        result.SkillsMasterId = item.SelectSkill;
                        result.SkillRating = item.SelectRating;
                    }
                }
                _db.SaveChanges();
                return true;
            }
            catch (Exception ex) 
            {
                Console.WriteLine(ex);
                return false;
            }
        }

        public async Task<bool> AddSkill(AddSkillsDto dto)
        {
            try
            {
                var item = _db.SkillsMaster.Where(m => m.SkillName == dto.SkillName.ToUpper()).FirstOrDefault();
                if (item != null)
                {
                    return false;
                }
                _db.SkillsMaster.Add(new SkillsMaster()
                {
                    SkillName = dto.SkillName.ToUpper(),
                    SkillDescription= dto.SkillDescription,
                    SkillStatus = 0
                });
                _db.SaveChanges();
                return true;
            }catch(Exception ex)
            {
                return false;
            }
        }

        public async Task<bool> AddSkillByAdmin(AddSkillsDto dto)
        {
            try
            {
                var item = _db.SkillsMaster.Where(m => m.SkillName == dto.SkillName.ToUpper()).FirstOrDefault();
                if (item != null)
                {
                    return false;
                }
                _db.SkillsMaster.Add(new SkillsMaster()
                {
                    SkillName = dto.SkillName.ToUpper(),
                    SkillDescription = dto.SkillDescription,
                    SkillStatus = 1
                });
                _db.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public async Task<bool> DeleteSkill(int id)
        {
            try
            {
                var item = _db.SkillsMaster.Where(m => m.Id== id).FirstOrDefault();
                if (item == null)
                {
                    return false; 
                }
                _db.SkillsMaster.Remove(item);
                _db.SaveChanges();
                return true;
            }catch(Exception ex)
            {
                return false;
            }
        }

        public async Task<bool> DeleteApproveSkill(int id)
        {
            try
            {
                var item = _db.EmployeSkills.Where(m => m.SkillsMasterId == id).FirstOrDefault();
                if (item != null)
                {
                    return false;
                }
                var item2 = _db.SkillsMaster.FirstOrDefault(m=> m.Id == id);
                _db.SkillsMaster.Remove(item2);
                _db.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public async Task<bool> ApproveSkill(int id)
        {
            try
            {
                var item = _db.SkillsMaster.Where(m => m.Id == id).FirstOrDefault();
                if (item == null)
                {
                    return false;
                }
                item.SkillStatus = 1;
                _db.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
    }
}
