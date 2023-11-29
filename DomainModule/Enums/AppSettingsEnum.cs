using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomainModule.Enums
{
    public enum AppSettingsEnum
    {
        [Display(Name = "Encryption Key")]
        EncryptionKey
    }
}
