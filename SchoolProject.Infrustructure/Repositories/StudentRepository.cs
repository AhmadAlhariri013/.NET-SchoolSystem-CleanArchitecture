﻿using Microsoft.EntityFrameworkCore;
using SchoolProject.Data.Entities;
using SchoolProject.Infrustructure.Data;
using SchoolProject.Infrustructure.InfrustructureBases;
using SchoolProject.Infrustructure.Interfaces;

namespace SchoolProject.Infrustructure.Repositories
{
    public class StudentRepository : GenericRepository<Student>, IStudentRepository
    {
        private readonly DbSet<Student> _Students;

        public StudentRepository(AppDbContext dbContext) : base(dbContext)
        {
            _Students = dbContext.Set<Student>();
        }
        #region Handles Functions
        public async Task<List<Student>> GetStudentsListAsync()
        {
            return await _Students.Include(x => x.Department).ToListAsync();
        }
        #endregion
    }
}
