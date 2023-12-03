using Microsoft.EntityFrameworkCore.Storage.ValueConversion.Internal;
using MimeKit.Cryptography;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomainModule.Dto.Pass
{
    public class AccountDetailModel
    {
        public List<AccountDetailsDto> Details { get; set; } = new List<AccountDetailsDto>();
    }
    public class AccountDetailsDto : AccountDetailsCreateDto
    {
        public int Id { get; set; }
        
    }
    public class AccountDetailsCreateDto
    {
        public string Account { get; set; }
        public string UserName { get; set; }
        public string Pass { get; set; }
        public string UserId { get; set; }
    }
    public class AccountExportDTO
    {
        public string Company { get; set; } // Software Name
        public string Name { get; set; } // Customer Name
        public string? Email { get; set; } // Customer Email
        public string? Phone { get; set; } // Customer Phone
        public string InfoText { get; set; } // have a text which says, Password in DataTable are decrypted, please make sure to protect this excel. 
        public DataTable DataTable { get; set; }
    }
}
