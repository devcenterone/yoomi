using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace Yoomi.Entity
{
    public class OrderForm
    {
        
        public string Name { get; set; }
        [Required(ErrorMessage = "Email-ul este obligatoriu!")]
        [DataType(DataType.EmailAddress, ErrorMessage = "Email-ul nu este valid!")]
        [EmailAddress(ErrorMessage = "Email-ul nu este valid!")]
        public string Email { get; set; }
        
        [Required(ErrorMessage = "Completati numarul de telefon.")]
        //[RegularExpression(@"^(0)[+1-9]((\d{7})|(\d{8}))$", ErrorMessage = "Nr. de telefon trebuie sa contina 9 sau 10 caractere numerice(prima cifra 0).")]     
        public string Phone { get; set; }

        [Required(ErrorMessage = "Alegeti minim un produs pt comanda.")]
        public string[] Products { get; set; }
    }
}
