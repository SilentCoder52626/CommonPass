﻿using DomainModule.Dto;
using DomainModule.RepositoryInterface;
using DomainModule.ServiceInterface;
using Microsoft.AspNetCore.Mvc;
using WebApp.Extensions;
using WebApp.Helper;
using WebApp.Models;

namespace WebApp.Areas.API.Controllers
{
    [Route("api/Setting")]
    [ApiController]
    public class SettingApiController : Controller
    {
        private readonly AppSettingsRepositoryInterface _appSettingRepo;
        private readonly AppSettingsServiceInterface _appSettingService;

        public SettingApiController(AppSettingsServiceInterface appSettingService, AppSettingsRepositoryInterface appSettingRepo)
        {
            _appSettingService = appSettingService;
            _appSettingRepo = appSettingRepo;
        }
        [HttpGet("{key}")]
        public IActionResult GetByKey(string key)
        {
            try
            {
                var userId = GetCurrentUserExtension.GetCurrentUserId(this);
                var setting = _appSettingRepo.GetByKey(key,userId);
                return Ok(new ApiResponseModel()
                {
                    StatusCode = StatusCodes.Status200OK,
                    Data = new
                    {
                        Id = setting.Id,
                        Key = setting.Key,
                        Value = setting.Value
                    }
                });

            }
            catch (Exception ex)
            {
                CommonLogger.LogError(ex.Message, ex);
                return BadRequest(ex);
            }
        }
        [HttpGet("")]
        public IActionResult GetSetting()
        {
            try
            {
                var userId = GetCurrentUserExtension.GetCurrentUserId(this);
                var setting = _appSettingRepo.GetAppSettingModel(userId);
                return Ok(new ApiResponseModel()
                {
                    StatusCode = StatusCodes.Status200OK,
                    Data = setting
                });

            }
            catch (Exception ex)
            {
                CommonLogger.LogError(ex.Message, ex);
                return BadRequest(ex);
            }
        }
        [HttpPost("Update")]
        public IActionResult Update([FromBody]List<AppSettingCreateDto> model)
        {
            try
            {
                var userId = GetCurrentUserExtension.GetCurrentUserId(this);
                _appSettingService.BulkUpdateSetting(model,userId);
                return Ok(new ApiResponseModel()
                {
                    StatusCode = StatusCodes.Status200OK,
                    Message = "Setting Updated.",
                });

            }
            catch(Exception ex)
            {
                CommonLogger.LogError(ex.Message, ex);
                return BadRequest(ex);
            }
        }
    }
}
