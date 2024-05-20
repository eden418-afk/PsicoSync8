using PsicoSync.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PsicoSync.Servicios
{
    public class ServicioCliente : ServicioBase
    {
        public ServicioCliente()
        {

        }   
        public async Task<List<objCliente>> GetItemsAsync()
        {
            await Init();
            var clientes = await Database.Table<objCliente>().ToListAsync();
            foreach (var cliente in clientes)
            {
                cliente.FechaNacimientoString = cliente.FechaNacimiento.ToString("dd MMM yyyy");
            }
            return clientes;
        }

        public async Task<objCliente> GetItemAsync(int id)
        {
            await Init();
            var cliente = await Database.Table<objCliente>().Where(i => i.ID == id).FirstOrDefaultAsync();
            cliente.FechaNacimientoString = cliente.FechaNacimiento.ToString("dd MMM yyyy");
            return cliente;
        }

        public async Task<int> SaveItemAsync(objCliente item)
        {
            await Init();
            if (item.ID != 0)
                return await Database.UpdateAsync(item);
            else
                return await Database.InsertAsync(item);
        }

        public async Task<int> DeleteItemAsync(objCliente item)
        {
            await Init();
            return await Database.DeleteAsync(item);
        }
    }
}
