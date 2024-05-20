using PsicoSync.Model;
using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PsicoSync.Servicios
{
    public class ServicioBase
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

        internal static SQLiteAsyncConnection Database;

        internal async Task Init()
        {
            if (Database is not null)
                return;

            Database = new SQLiteAsyncConnection(DatabasePath, Flags);
            var resultCliente = await Database.CreateTableAsync<objCliente>();
            var resultTipoCita = await Database.CreateTableAsync<objTipoCita>();
            var resultCita = await Database.CreateTableAsync<objCita>();
        }
    }
}
