using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace Yoomi.Entity
{
    public class OrderForm
    {
        [Required(ErrorMessage = "Completati Numele.")]
        [StringLength(30, MinimumLength = 3, ErrorMessage = "Numele trebuie sa contina intre 3 - 30 caractere")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Completati Adresa.")]
        [StringLength(200, MinimumLength = 10, ErrorMessage = "Adresa trebuie sa contina intre 10 - 200 caractere")]
        public string Address { get; set; }

        [Required(ErrorMessage = "Completati Email-ul.")]
        [DataType(DataType.EmailAddress, ErrorMessage = "Email-ul nu este valid!")]
        [EmailAddress(ErrorMessage = "Email-ul nu este valid!")]
        public string Email { get; set; }
        
        [Required(ErrorMessage = "Completati numarul de telefon.")]
        [RegularExpression(@"^(0)[+1-9]((\d{7})|(\d{8}))$", ErrorMessage = "Nr. de telefon trebuie sa contina 9 sau 10 caractere numerice(prima cifra 0).")]     
        public string Phone { get; set; }

        [Required(ErrorMessage = "Alegeti minim un produs pt comanda.")]
        public int[] Products { get; set; }
        public string[] ProductsNm { get; set; }
    }
}
