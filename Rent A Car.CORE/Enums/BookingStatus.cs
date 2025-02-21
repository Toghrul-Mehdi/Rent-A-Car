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
        Confirmed,      // Onaylandı
        Cancelled,      // İptal edildi
        Completed       // Tamamlandı (Araç geri teslim edildi)
    }
}
