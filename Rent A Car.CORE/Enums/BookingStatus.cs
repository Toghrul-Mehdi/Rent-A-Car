using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rent_A_Car.CORE.Enums
{
    public enum BookingStatus
    {
        Pending,        // Beklemede (henüz onaylanmadı)
        Started,      // Basladi
        Cancelled,      // İptal edildi
        Scheduled,      //Planlasdirilir
        Completed       // Tamamlandı (Araç geri teslim edildi)
    }
}
