using PsicoSync.Interfaces;
using PsicoSync.Model;
using SQLite;

namespace PsicoSync.Servicios
{
    public  class DatabaseService
    {
        public const string DatabaseFilename = "PsicoSync.db3";

        public const SQLite.SQLiteOpenFlags Flags =
            // open the database in read/write mode
            SQLite.SQLiteOpenFlags.ReadWrite |
            // create the database if it doesn't exist
            SQLite.SQLiteOpenFlags.Create |
            // enable multi-threaded database access
            SQLite.SQLiteOpenFlags.SharedCache;

        public static string DatabasePath =>
            Path.Combine(FileSystem.AppDataDirectory, DatabaseFilename);

        private SQLiteAsyncConnection Database;

        public DatabaseService(string dbPath)
        {
        }

        async Task Init()
        {
            if (Database is not null)
                return;

            Database = new SQLiteAsyncConnection(DatabasePath, Flags);
            var result = await Database.CreateTableAsync<objCliente>();
            var result2 = await Database.CreateTableAsync<objTipoCita>();
        }

        public async Task<int> InsertItemAsync<T>(IObjetoDatabase item)
        {
            await Init();
            return await Database.InsertAsync(item);
        }

        public async Task<int> UpdateItemAsync<T>(IObjetoDatabase item)
        {
            await Init();
            return await Database.UpdateAsync(item);
        }

        //public async Task<IObjetoDatabase> GetItemAsync<IObjetoDatabase>(int id) where IObjetoDatabase : new()
        //{
        //    await Init();
        //    return await Database.Table<IObjetoDatabase>().Where(i => i.ID == id).FirstOrDefaultAsync();
        //}

        //public int Delete<T>(T objeto)
        //{
        //    return Database.Delete(objeto);
        //}

        //public List<T> GetAll<T>() where T : new()
        //{
        //    return Database.Table<T>().ToList();
        //}
    }
}
