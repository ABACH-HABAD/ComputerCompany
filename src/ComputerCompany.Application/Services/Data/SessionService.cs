using ComputerCompany.Application.Abstractions.Services.Token;
using ComputerCompany.Application.Abstractions.Services.Validation;
using ComputerCompany.Application.Results;
using ComputerCompany.Core.Abstractions.Repositories;
using ComputerCompany.Core.Models;

namespace ComputerCompany.Application.Services.Data;

public class SessionService
(
    IJwtTokenService jwtTokenService,
    IRefreshTokenService refreshTokenService,
    IValidator<SessionModel> validator,
    ISessionRepository repository
) :
BaseDataService<SessionModel, ISessionRepository>(validator, repository, "сессия"),
ISessionService
{
    public async Task<LoginResult> RefreshTokensAsync(string refreshToken, string? ip = null, CancellationToken cancellationToken = default)
    {
        if (ip == null) return LoginResult.Fail("Нейдействительный IP адрес");

        try
        {
            //Проверяем активные сесии
            SessionModel? sessionModel = await _repository.GetByTokenAsync(refreshToken, ip, cancellationToken);
            if (sessionModel == null) return LoginResult.Fail("У пользователя нет активных сессий");

            //Удаляем активную сессию
            Result deleteResult = await DeleteAsync(sessionModel.Id, cancellationToken);
            if (!deleteResult.IsSuccess) return LoginResult.Fail(deleteResult.Message);

            //Генерируем токены
            string access = jwtTokenService.CreateToken(sessionModel.Account);
            string refresh = refreshTokenService.CreateToken();

            //Начинаем новую сессию
            Result addResult = await AddAsync(new SessionModel(Guid.NewGuid(), refresh, sessionModel.Account, ip), cancellationToken);
            if (!addResult.IsSuccess) return LoginResult.Fail(addResult.Message);

            return LoginResult.Success(access, refresh);
        }
        catch (Exception ex)
        {
            return LoginResult.Fail(ex.Message);
        }
    }

    public async Task<DataResult<SessionModel>> GetByAccountAsync(Guid accountId, string? ip = null, CancellationToken cancellationToken = default)
    {
        if (ip == null) return DataResult<SessionModel>.Fail("Нейдействительный IP адрес");

        try
        {
            SessionModel? sessionModel = await _repository.GetByAccountAsync(accountId, ip, cancellationToken);

            if (sessionModel == null) return DataResult<SessionModel>.Fail("У пользователя нет активных сессий");
            else return DataResult<SessionModel>.Success(sessionModel);
        }
        catch (Exception ex)
        {
            return DataResult<SessionModel>.Fail(ex.Message);
        }
    }

    public async Task<Result> LogoutAsync(Guid accountId, string? ip = null, CancellationToken cancellationToken = default)
    {
        if (ip == null) return Result.Fail("Нейдействительный IP адрес");

        try
        {
            SessionModel? sessionModel = await _repository.GetByAccountAsync(accountId, ip, cancellationToken);
            if (sessionModel != null) await _repository.DeleteAsync(sessionModel.Id, cancellationToken);

            return Result.Success();
        }
        catch (Exception ex)
        {
            return Result.Fail(ex.Message);
        }
    }
}