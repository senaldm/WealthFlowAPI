using System.Net;
using WealthFlow.Application.Transactions.DTOs;
using WealthFlow.Application.Transactions.Interfaces;
using WealthFlow.Infrastructure.Transactions.Repositories;
using WealthFlow.Shared.Helpers;
using WealthFlow.Application.Users.Interfaces;
using WealthFlow.Domain.Entities.Transactions;
using static WealthFlow.Domain.Enums.Enum;

namespace WealthFlow.Application.Transactions.Services
{
    public class IncomeService : IIncomeService
    {
        private readonly IIncomeRepository _incomeRepository;
        private readonly IUserService _userService;

        public IncomeService(IIncomeRepository incomeRepository, IUserService userService)
        {
            _incomeRepository = incomeRepository;
            _userService = userService;
        }

        public async Task<Result<string>> DeleteBulkOrSingleIncomeDetailsAsync(List<Guid> incomeIds)
        {
            if(incomeIds == null || !incomeIds.Any())
                return Result<string>.Failure("You must select entries to remove", HttpStatusCode.BadRequest);


            bool isDeleted = await _incomeRepository.DeleteBulkOrSingleIncomeDetailsAsync(incomeIds);

            if (!isDeleted)
                return Result<string>.Failure("Internal Server Error. Try Again.", HttpStatusCode.InternalServerError);

            return Result<string>.Success("Succeffuly Delete the income details", HttpStatusCode.OK);
        }

        public async Task<Result<Object>> GetAllIncomeDetailsAsync(int pageNumber, int pageSize, SortBy sortBy, SortOrderBy orderBy)
        {
            Guid? userId = _userService.GetLoggedInUserId();

            if (userId == null)
                return Result<Object>.Failure("User not found.", HttpStatusCode.Unauthorized);

            List<Income> incomeDetails = await _incomeRepository.GetAllIncomeDetailsAsync(userId.Value, pageNumber, pageSize, sortBy, orderBy);
            return Result<Object>.Success(ExtractIncomeDTOFromIncome(incomeDetails));

        }
        
        private static List<IncomeDTO> ExtractIncomeDTOFromIncome(IEnumerable<Income>? incomeDetails)
        {
            if (incomeDetails == null)
                return null;

            return incomeDetails.Select(detail => new IncomeDTO
            {
                IncomeId = detail.IncomeId,
                IncomeName = detail.IncomeName,
                IncomeAmount = detail.IncomeAmount,
                IncomeDescription = detail.IncomeDescription,
                IncomeTypeId = detail.IncomeTypeId,
            }).ToList();

        }

        public async Task<Result<Object>> GetDateSpecificIncomeDetailsAsync(DateTime startingDate, DateTime endDate)
        {
            Guid? userId = _userService.GetLoggedInUserId();

            if (userId == null)
                return Result<Object>.Failure("User Not Found.", HttpStatusCode.Unauthorized);

            IEnumerable<Income> incomeDetails = await _incomeRepository
                .GetDateRangeSpecificIncomeDetailsAsync(startingDate, endDate, userId.Value);

            return Result<Object>.Success(ExtractIncomeDTOFromIncome(incomeDetails));
        }

        public async Task<Result<string>> StoreIncomeAsync(IncomeDTO incomeDTO)
        {
            Guid? userId = _userService.GetLoggedInUserId();
            if (!userId.HasValue)
                return Result<string>.Failure("There is issue with user credentials.Try again", HttpStatusCode.Unauthorized);

            Income incomeDetails = ConvertIncomeDTOIntoIncome(incomeDTO, userId.Value);

            bool isStored = await _incomeRepository.StoreIncomeDetailsAsync(incomeDetails);

            if (!isStored)
                return Result<string>.Failure("Couldn't to make the task. Please try again.", HttpStatusCode.InternalServerError);

            return Result<string>.Success("Income details added successfully.", HttpStatusCode.OK);
        }

        private Income ConvertIncomeDTOIntoIncome(IncomeDTO incomeDTO, Guid userId)
        {
            return new Income
            {
                IncomeId = Guid.NewGuid(),
                UserId = userId,
                IncomeName = incomeDTO.IncomeName,
                IncomeAmount = incomeDTO.IncomeAmount,
                IncomeDescription = incomeDTO.IncomeDescription,
                IncomeTypeId = incomeDTO.IncomeTypeId,
                PaymentMethodId = incomeDTO.PaymentMethodId,
                ReceiveDate = incomeDTO.ReceiveDate,
                CreatedAt = DateTime.UtcNow
            };
        }

        public async Task<Result<Object>> UpdateIncomeAsync(IncomeDTO incomeDTO)
        {
            Guid? userId = _userService.GetLoggedInUserId();

            if (userId == null)
                return Result<Object>.Failure("There is issue with user credentials.Try again", HttpStatusCode.Unauthorized);

            Income income = ConvertIncomeDTOIntoIncome(incomeDTO, userId.Value);
            income.UpdatedAt = DateTime.UtcNow;

            bool isUpdated = await _incomeRepository.UpdateIncomeDetailsAsync(income);

            if (!isUpdated)
                return Result<Object>.Failure("Couldn't to make the task. Please try again.", HttpStatusCode.InternalServerError);

            return Result<Object>.Success(incomeDTO);
        }
    

    }
}
