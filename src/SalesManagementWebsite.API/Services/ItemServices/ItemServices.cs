﻿using AutoMapper;
using SalesManagementWebsite.Contracts.Dtos.Item;
using SalesManagementWebsite.Contracts.Dtos.Item;
using SalesManagementWebsite.Contracts.Dtos.Response;
using SalesManagementWebsite.Domain.Entities;
using SalesManagementWebsite.Domain.UnitOfWork;
using System.Net;
using System.Text.Json;

namespace SalesManagementWebsite.API.Services.ItemServices
{
    public class ItemServices : IItemServices
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ILogger _logger;
        private readonly IConfiguration _configuration;

        public ItemServices(IUnitOfWork unitOfWork, IMapper mapper, IConfiguration configuration, ILogger<ItemServices> logger)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _configuration = configuration;
            _logger = logger;
        }

        public async ValueTask<ResponseHandle<ItemOutputDto>> GetAllItems()
        {
            try
            {
                var gItemList = await _unitOfWork.ItemRepository.GetAllAsync();

                if (gItemList == null)
                {
                    return new ResponseHandle<ItemOutputDto>
                    {
                        IsSuccess = false,
                        StatusCode = (int)HttpStatusCode.NotFound,
                        Data = null,
                        ErrorMessage = $"Can not get list of [Item]"
                    };
                }

                var cateListOutput = _mapper.Map<List<ItemOutputDto>>(gItemList);

                return new ResponseHandle<ItemOutputDto>
                {
                    IsSuccess = true,
                    StatusCode = (int)HttpStatusCode.OK,
                    Data = null,
                    ListData = cateListOutput,
                    ErrorMessage = string.Empty
                };
            }
            catch (Exception ex)
            {
                _logger.LogError($"ItemServices -> GetAllItems() " +
                                 $"- Have exception: {ex}, at {DateTime.UtcNow.ToLongTimeString()}");
                throw;
            }
        }

        public async ValueTask<ResponseHandle<ItemOutputDto>> GetItem(Guid id)
        {
            try
            {
                var gItem = await _unitOfWork.ItemRepository.GetItem(id);

                if (gItem == null)
                {
                    return new ResponseHandle<ItemOutputDto>
                    {
                        IsSuccess = false,
                        StatusCode = (int)HttpStatusCode.NotFound,
                        Data = null,
                        ErrorMessage = $"Can not get [Item] with [id]: {id}"
                    };
                }

                var itemOutput = _mapper.Map<ItemOutputDto>(gItem);

                return new ResponseHandle<ItemOutputDto>
                {
                    IsSuccess = true,
                    StatusCode = (int)HttpStatusCode.OK,
                    Data = itemOutput,
                    ErrorMessage = string.Empty
                };
            }
            catch (Exception ex)
            {
                _logger.LogError($"ItemService -> GetItem({JsonSerializer.Serialize(id)}) " +
                                 $"- Have exception: {ex}, at {DateTime.UtcNow.ToLongTimeString()}");
                throw;
            }
        }

        public async ValueTask<ResponseHandle<ItemOutputDto>> CreateItem(ItemCreateDto itemCreateDto)
        {
            try
            {
                var item = _mapper.Map<Item>(itemCreateDto);

                _unitOfWork.ItemRepository.Add(item);
                await _unitOfWork.CommitAsync();

                var itemOutput = _mapper.Map<ItemOutputDto>(item);

                return new ResponseHandle<ItemOutputDto>
                {
                    IsSuccess = true,
                    StatusCode = (int)HttpStatusCode.OK,
                    Data = itemOutput,
                    ErrorMessage = string.Empty
                };
            }
            catch (Exception ex)
            {
                _logger.LogError($"ItemServices -> CreateItem({JsonSerializer.Serialize(itemCreateDto)}) " +
                                 $"- Have exception: {ex}, at {DateTime.UtcNow.ToLongTimeString()}");
                throw;
            }
        }

        public async ValueTask<ResponseHandle<ItemOutputDto>> UpdateItem(ItemInputDto itemInputDto)
        {
            try
            {
                var gItem = await _unitOfWork.ItemRepository.GetAsync(c => c.Id.Equals(itemInputDto.Id));

                if (gItem == null)
                {
                    return new ResponseHandle<ItemOutputDto>
                    {
                        IsSuccess = false,
                        StatusCode = (int)HttpStatusCode.NotFound,
                        Data = null,
                        ErrorMessage = $"Can not get the [Item]: {JsonSerializer.Serialize(itemInputDto)}"
                    };
                }

                //Mapping field modify
                gItem.Name = itemInputDto.Name;
                gItem.Description = itemInputDto.Description;
                gItem.Price = itemInputDto.Price;
                gItem.CategoryId = itemInputDto.CategoryId;
                gItem.BrandId = itemInputDto.BrandId;
                gItem.ModifiedDate = DateTime.Now;

                _unitOfWork.ItemRepository.Update(gItem);
                await _unitOfWork.CommitAsync();

                var itemOutput = _mapper.Map<ItemOutputDto>(gItem);

                return new ResponseHandle<ItemOutputDto>
                {
                    IsSuccess = true,
                    StatusCode = (int)HttpStatusCode.OK,
                    Data = itemOutput,
                    ErrorMessage = string.Empty
                };
            }
            catch (Exception ex)
            {
                _logger.LogError($"ItemServices -> UpdateItem({JsonSerializer.Serialize(itemInputDto)}) " +
                                 $"- Have exception: {ex}, at {DateTime.UtcNow.ToLongTimeString()}");
                throw;
            }
        }

        public async ValueTask<ResponseHandle<ItemOutputDto>> DeleteItem(Guid id)
        {
            try
            {
                var gItem = await _unitOfWork.ItemRepository.GetAsync(c => c.Id.Equals(id));

                if (gItem == null)
                {
                    return new ResponseHandle<ItemOutputDto>
                    {
                        IsSuccess = false,
                        StatusCode = (int)HttpStatusCode.NotFound,
                        Data = null,
                        ErrorMessage = $"Can not get [Item] with [id]: {id}"
                    };
                }

                _unitOfWork.ItemRepository.Remove(gItem);
                await _unitOfWork.CommitAsync();


                var cateOutput = _mapper.Map<ItemOutputDto>(gItem);

                return new ResponseHandle<ItemOutputDto>
                {
                    IsSuccess = true,
                    StatusCode = (int)HttpStatusCode.OK,
                    Data = cateOutput,
                    ErrorMessage = string.Empty
                };
            }
            catch (Exception ex)
            {
                _logger.LogError($"ItemServices -> DeleteItem({JsonSerializer.Serialize(id)}) " +
                                 $"- Have exception: {ex}, at {DateTime.UtcNow.ToLongTimeString()}");
                throw;
            }
        }
    }
}
