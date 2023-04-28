﻿using SalesManagementWebsite.Contracts.Dtos.Response;
using SalesManagementWebsite.Contracts.Dtos.User;

namespace SalesManagementWebsite.API.Services.UserServices
{
    public interface IUserService
    {
        public Task<ResponseHandle<UserOuputDto>> Login(UserLoginDto userRegisterDto);
        public Task<ResponseHandle<UserOuputDto>> Register(UserRegisterDto userRegisterDto);
        public Task<ResponseHandle<UserOuputDto>> GetUser(string userName);
        public Task<ResponseHandle<UsersListOuputDto>> GetAllUsers();
        public Task<ResponseHandle<UserOuputDto>> UpdateUser(UserInputDto userInputDto);
    }
}
