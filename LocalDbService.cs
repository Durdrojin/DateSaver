﻿using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DateSaver
{
    public class LocalDbService
    {
        private const string DB_NAME = "dateSaver_local_db.db3";
        private readonly SQLiteAsyncConnection _connection;
        private DateTime currentDate = DateTime.Now;


        public LocalDbService()
        {
            _connection = new SQLiteAsyncConnection(Path.Combine(FileSystem.AppDataDirectory, DB_NAME));
            _connection.CreateTableAsync<Date>();
        }


        // CRUD Operations

        // Create
        public async Task Create(Date date)
        {
            await _connection.InsertAsync(date);

            await UpdateCountDown();
        }

        // Read
        public async Task<List<Date>> GetDates()
        {
            return await _connection.Table<Date>().ToListAsync();
        }
        public async Task<Date> GetById(int id)
        {
            return await _connection.Table<Date>().Where(x => x.Id == id).FirstOrDefaultAsync();
        }

        // Update
        public async Task Update(Date date)
        {
            await _connection.UpdateAsync(date);

            await UpdateCountDown();
        }

        // Test
        public async Task UpdateCountDown()
        {
            List <Date> resultsFromSQL = await _connection.Table<Date>().ToListAsync();

            int i = 0;
            if (resultsFromSQL.Count != 0)
            {
                while (i < resultsFromSQL.Count)
                {
                    Date test = await GetById(i+1);
                    test.CountDown = (test.DateSaved.Date - currentDate.Date).Days;

                    await _connection.UpdateAsync(test);

                    i++;
                }
            }

            /*
            await _connection.UpdateAllAsync<Date>();


            await _connection.UpdateAllAsync<Date>(date.CountDown = 1);


            await _connection.UpdateAllAsync(date.CountDown = 1);

            foreach (var item in _connection.Table<Date>)
            {

            }
            await _connection.UpdateAsync(date.CountDown);
            */
        }

        // Delete
        public async Task Delete(Date date)
        {
            await _connection.DeleteAsync(date);
        }
    }
}
