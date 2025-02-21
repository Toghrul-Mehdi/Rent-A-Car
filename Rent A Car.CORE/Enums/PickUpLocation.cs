using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rent_A_Car.CORE.Enums
{
    using System.ComponentModel.DataAnnotations;

    public enum PickupLocation
    {
        // Qırmızı Xətt (Red Line)
        [Display(Name = "İçərişəhər")]
        Icherisheher,

        [Display(Name = "Sahil")]
        Sahil,

        [Display(Name = "28 May")]
        May28,

        [Display(Name = "Gənclik")]
        Ganjlik,

        [Display(Name = "Nəriman Nərimanov")]
        Narimanov,

        [Display(Name = "Ulduz")]
        Ulduz,

        [Display(Name = "Koroğlu")]
        Koroglu,

        [Display(Name = "Neftçilər")]
        Neftchilar,

        [Display(Name = "Xalqlar Dostluğu")]
        KhalglarDostlugu,

        [Display(Name = "Əhmədli")]
        Ahmedli,

        [Display(Name = "Həzi Aslanov")]
        HaziAslanov,

        // Yaşıl Xətt (Green Line)
        [Display(Name = "Xətai")]
        Khatai,

        [Display(Name = "Şah İsmayıl Xətai")]
        ShahIsmailKhatai,

        // Mavi Xətt (Blue Line)
        [Display(Name = "Memar Əcəmi")]
        MemarAjami,

        [Display(Name = "Nəsimi")]
        Nasimi,

        [Display(Name = "Cəfər Cabbarlı")]
        JafarJabbarli,

        [Display(Name = "20 Yanvar")]
        January20,

        [Display(Name = "İnşaatçılar")]
        Inshaatchilar,

        [Display(Name = "Elmlər Akademiyası")]
        ElmlerAkademiyasi,

        [Display(Name = "Nizami")]
        Nizami,

        [Display(Name = "Bakmil")]
        Bakmil,

        // Yeni Xəttlər
        [Display(Name = "Avtovağzal")]
        Avtovagzal,

        [Display(Name = "8 Noyabr")]
        November8,

        [Display(Name = "Dərnəgül")]
        Dernegul
    }

}
