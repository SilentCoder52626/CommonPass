using DomainModule.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomainModule.ServiceInterface
{
    public interface AppSettingsServiceInterface
    {
        void UpdateSetting(AppSettingCreateDto dto, string userId);
        void BulkUpdateSetting(List<AppSettingCreateDto> dto,string userId);
    }
}
