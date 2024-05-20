using PsicoSync.Model;

namespace PsicoSync.Servicios
{
    public class ServicioTipoCita : ServicioBase
    {
        public ServicioTipoCita()
        {

        }

        public async Task<List<objTipoCita>> GetItemsAsync()
        {
            await Init();
            return await Database.Table<objTipoCita>().ToListAsync();
        }

        public async Task<objTipoCita> GetItemAsync(int id)
        {
            await Init();
            return await Database.Table<objTipoCita>().Where(i => i.ID == id).FirstOrDefaultAsync();
        }

        public async Task<int> SaveItemAsync(objTipoCita item)
        {
            await Init();
            if (item.ID != 0)
                return await Database.UpdateAsync(item);
            else
                return await Database.InsertAsync(item);
        }

        public async Task<int> DeleteItemAsync(objTipoCita item)
        {
            await Init();
            return await Database.DeleteAsync(item);
        }
    }
}
