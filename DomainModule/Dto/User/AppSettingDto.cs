﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomainModule.Dto
{
    public class AppSettingCreateDto 
    {
        public string Key { get; set; }
        public string? Value { get; set; }
    }
    public class AppSettingDto :AppSettingCreateDto
    {
        public string? UserId { get; set; }
        public string? DisplayName { get; set; }
    }
    public class AppSettingModel
    {
        public List<AppSettingDto> AppSettings { get; set; } = new List<AppSettingDto>(); 
    }
}
