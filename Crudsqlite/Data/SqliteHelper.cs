using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Crudsqlite.Models;
using SQLite;

namespace Crudsqlite.Data
{
    public class SqliteHelper
    {
        SQLiteAsyncConnection db;

        public SqliteHelper(String dbpath)
        {
            db = new SQLiteAsyncConnection(dbpath);
            db.CreateTableAsync<Alumno>().Wait();
        }

        public Task <int> SaveAlumnoAsync(Alumno alumno)
        {
            if (alumno.IdAlumno != 0)
            {
                return  db.UpdateAsync(alumno);
            }
            else
            {
                return db.InsertAsync(alumno);
            }
        }
        public Task<int> DeleteAlumno(Alumno alumno)
        {
            return db.DeleteAsync(alumno);
        }
        public Task<List<Alumno>> GetAlumnosAsync()
        {
            return db.Table<Alumno>().ToListAsync();
        }

        public Task<Alumno> GetAlumnoById(int idAlumno)
        {
            return db.Table<Alumno>().Where(a => a.IdAlumno == idAlumno)
                .FirstOrDefaultAsync();
        }
    }
}
