﻿using LanguageExt;
using LanguageExt.Common;
using QuizApi.Constants;

namespace QuizApi.Services.UserService;

public interface IUserService
{
    Task<Result<User>> RegisterAsync(RegisterModel registerModel);
    Task<Result<AuthModel>> GetTokenAsync(TokenRequestModel requestModel);
    Task<Result<Unit>> AddToRoleAsync(ManageRoleModel roleModel);
    Task<Result<Unit>> RemoveRoleAsync(ManageRoleModel roleModel);
} 