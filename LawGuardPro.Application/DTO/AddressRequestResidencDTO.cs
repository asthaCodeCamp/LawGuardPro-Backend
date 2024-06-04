using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LawGuardPro.Application.DTO
{
    public class AddressRequestResidencDTO
    {
        public string AddressLine1 { get; set; }
        public string AddressLine2 { get; set; }
        public string Town { get; set; }
        public int PostalCode { get; set; }
        public string Country { get; set; }

    }
}
