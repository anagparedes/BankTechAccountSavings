using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankTechAccountSavings.Domain.Enums
{
    public enum Bank
    {
        [Display(Name = "Banco De Reservas De La R.D.")]
        BanReservas = 1,
        [Display(Name = "Banco Popular Dominicano Banco Multiple")]
        PopularDominicano,
        [Display(Name = "Banco Multiple BHD")]
        BHDLeon,
        [Display(Name = "Banco Multiple Santa Cruz")]
        SantaCruz,
        [Display(Name = "Banco Multiple Caribe Internacional")]
        Caribe,
        [Display(Name = "Banco Multiple BDI")]
        BDI,
        [Display(Name = "Banco Multiple Vimenca")]
        Vimenca,
        [Display(Name = "Banco Multiple Lopez de Haro")]
        LopezdeHaro,
        [Display(Name = "Banco Multiple Ademi")]
        Ademi,
        [Display(Name = "Banesco Banco Multiple")]
        Banesco,
        [Display(Name = "The Bank of Nova Scotia")]
        Scotiabank,
        [Display(Name = "Banco Multiple Promerica De La R.D.")]
        Promerica,
        [Display(Name = "Asociación Popular de Ahorros y Préstamos")]
        AsociaciónPopulardeAhorrosyPréstamos,
        [Display(Name = "Asociación La Nacional de Ahorros y Préstamos")]
        AsociaciónLaNacional,

    }
}
