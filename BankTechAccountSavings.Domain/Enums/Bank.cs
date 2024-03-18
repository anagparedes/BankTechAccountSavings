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
        [Display(Name = "BanReservas")]
        BanReservas = 1,
        [Display(Name = "Banco Popular Dominicano")]
        BancoPopularDominicano,
        [Display(Name = "Banco BHD Leon")]
        BancoBHDLeon,
        [Display(Name = "Banco Santa Cruz")]
        BancoSantaCruz,
        [Display(Name = "Banco Caribe")]
        BancoCaribe,
        [Display(Name = "Banco BDI")]
        BancoBDI,
        [Display(Name = "Banco Vimenca")]
        BancoVimenca,
        [Display(Name = "Banco Lopez de Haro")]
        BancoLopezdeHaro,
        [Display(Name = "Bancamérica")]
        Bancamérica,
        [Display(Name = "Banesco")]
        Banesco,
        [Display(Name = "Scotiabank")]
        Scotiabank,
        [Display(Name = "Banco Promerica")]
        BancoPromerica,
        [Display(Name = "Asociación Popular de Ahorros y Préstamos")]
        AsociaciónPopulardeAhorrosyPréstamos,
        [Display(Name = "Asociación Cibao")]
        AsociaciónCibao,
        [Display(Name = "Asociación La Nacional")]
        AsociaciónLaNacional,

    }
}
