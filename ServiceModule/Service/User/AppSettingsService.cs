﻿using DomainModule.Dto;
using DomainModule.Dto.User;
using DomainModule.Entity;
using DomainModule.Enums;
using DomainModule.Exceptions;
using DomainModule.RepositoryInterface;
using DomainModule.ServiceInterface;
using DomainModule.ServiceInterface.Pass;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceModule.Service
{
    public class AppSettingsService : AppSettingsServiceInterface
    {
        private readonly AppSettingsRepositoryInterface _settingRepo;
        private readonly IAccountDetailsService _accountDetailsService;
        private readonly IUnitOfWork _unitOfWork;

        public AppSettingsService(AppSettingsRepositoryInterface settingRepo, IUnitOfWork unitOfWork, IAccountDetailsService accountDetailsService)
        {
            _settingRepo = settingRepo;
            _unitOfWork = unitOfWork;
            _accountDetailsService = accountDetailsService;
        }

        public void BulkUpdateSetting(List<AppSettingDto> dto)
        {
            try
            {
                using (var tx = _unitOfWork.BeginTransaction())
                {
                    foreach (var data in dto)
                    {
                        UpdateSettingModel(data);
                    }
                    _unitOfWork.Complete();
                    tx.Commit();
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public void UpdateSetting(AppSettingDto dto)
        {
            try
            {
                using (var tx = _unitOfWork.BeginTransaction())
                {
                    UpdateSettingModel(dto);
                    _unitOfWork.Complete();
                    tx.Commit();
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        private void UpdateSettingModel(AppSettingDto dto)
        {
            var Entity = _settingRepo.GetByKey(dto.Key, dto.UserId);
            if (Entity == null)
            {
                Entity = new AppSettings();
                Entity.Key = dto.Key;
                Entity.Value = dto.Value;
                Entity.UserId = dto.UserId;
                _settingRepo.Insert(Entity);
            }
            else
            {
                if (Entity.Key == AppSettingsEnum.EncryptionKey.ToString())
                {
                    _accountDetailsService.ReEncryptPasswordWithoutCommit(dto.UserId,dto.Value);
                }

                Entity.Value = dto.Value;
                Entity.UserId = dto.UserId;

                
                _settingRepo.Update(Entity);
            }

        }
    }
}
