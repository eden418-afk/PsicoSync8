using PsicoSync.Model;

namespace PsicoSync.Servicios
{
    public class ServicioCita : ServicioBase
    {
        public ServicioCliente servicioCliente = new ServicioCliente();
        public ServicioTipoCita servicioTipoCita = new ServicioTipoCita();

        public ServicioCita()
        {

        }

        public async Task<List<objCita>> GetItemsAsync()
        {
            await Init();
            var citas = await Database.Table<objCita>().ToListAsync();

            var clientes = await servicioCliente.GetItemsAsync();
            var tipoCitas = await servicioTipoCita.GetItemsAsync();

            foreach (var cita in citas)
            {
                cita.Cliente = clientes.Where(c => c.ID == cita.ClienteID).FirstOrDefault();
                cita.TipoCita = tipoCitas.Where(t => t.ID == cita.TipoCitaID).FirstOrDefault();
                cita.FechaString = cita.Fecha.ToString("dd MMM yyyy HH:mm");
            }

            return citas;
        }

        public async Task<objCita> GetItemAsync(int id)
        {
            await Init();
            var cita = await Database.Table<objCita>().Where(i => i.ID == id).FirstOrDefaultAsync();

            cita.Cliente = await servicioCliente.GetItemAsync(cita.ClienteID);
            cita.TipoCita = await servicioTipoCita.GetItemAsync(cita.TipoCitaID);

            return cita;
        }

        public async Task<int> SaveItemAsync(objCita item)
        {
            await Init();
            if (item.ID != 0)
                return await Database.UpdateAsync(item);
            else
                return await Database.InsertAsync(item);
        }

        public async Task<int> DeleteItemAsync(objCita item)
        {
            await Init();
            return await Database.DeleteAsync(item);
        }
    }
}
