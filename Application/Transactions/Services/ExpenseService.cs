using System.Net;
using WealthFlow.Application.Transactions.DTOs;
using WealthFlow.Application.Transactions.Interfaces;
using WealthFlow.Application.Users.Interfaces;
using WealthFlow.Domain.Entities.Transactions;
using WealthFlow.Infrastructure.Transactions.Repositories;
using WealthFlow.Shared.Helpers;
using static WealthFlow.Domain.Enums.Enum;

namespace WealthFlow.Application.Transactions.Services
{
    public class ExpenseService : IExpenseService
    {
        private readonly IExpenseRepository _expenseRepository;
        private readonly IUserService _userService;

        public ExpenseService(IExpenseRepository expenseRepository, IUserService userService)
        {
            _expenseRepository = expenseRepository;
            _userService = userService;
        }
        public async Task<Result<string>> DeleteBulkOrSingleExpenseDetailsAsync(List<Guid> expenseIds)
        {
            if (expenseIds == null || !expenseIds.Any())
                return Result<string>.Failure("You must select entries to remove", HttpStatusCode.BadRequest);


            bool isDeleted = await _expenseRepository.RemoveBulkOrSingleExpenseDetailsAsync(expenseIds);

            if (!isDeleted)
                return Result<string>.Failure("Internal Server Error. Try Again.", HttpStatusCode.InternalServerError);

            return Result<string>.Success("Succeffuly Delete the income details", HttpStatusCode.OK);
        }

        public async Task<Result<object>> GetAllExpenseDetailsAsync(int pageNumber, int pageSize, SortBy sort, SortOrderBy order)
        {
            Guid? userId = _userService.GetLoggedInUserId();

            if (userId == null)
                return Result<Object>.Failure("User not found.", HttpStatusCode.Unauthorized);

            List<Expense> expenseDetails = await _expenseRepository.GetAllExpenseDetailsAsync(userId.Value, pageNumber, pageSize, sort, order);
            
            if(expenseDetails ==  null || !expenseDetails.Any())
                return Result<Object>.Success("No any data to obtain.", HttpStatusCode.NoContent);

            return Result<Object>.Success(ExtractExpenseDTOFromExpense(expenseDetails));
        }

        private static List<ExpenseDTO> ExtractExpenseDTOFromExpense(List<Expense> expenseDetails)
        {

            return expenseDetails.Select(detail => new ExpenseDTO
            {
                ExpenseId = detail.ExpenseId,
                ExpenseName = detail.ExpenseName,
                ExpenseAmount = detail.ExpenseAmount,
                ExpenseDescription = detail.ExpenseDescription,
                ExpenseTypeId = detail.ExpenseTypeId,
            }).ToList();

        }

        public async Task<Result<object>> GetDateSpecificExpenseDetailsAsync(DateTime startingDate, DateTime endDate)
        {
            Guid? userId = _userService.GetLoggedInUserId();

            if (userId == null)
                return Result<Object>.Failure("User Not Found.", HttpStatusCode.Unauthorized);

            List<Expense> expenseDetails = await _expenseRepository
                .GetDateRangeSpecificExpenseDetailsAsync(startingDate, endDate, userId.Value);

            if (expenseDetails == null || !expenseDetails.Any())
                return Result<Object>.Success("No any data to obtain.", HttpStatusCode.NoContent);

            return Result<Object>.Success(ExtractExpenseDTOFromExpense(expenseDetails));
        }

        public async Task<Result<string>> StoreBulkOrSingleExpenseDetailsAsync(List<ExpenseDTO> expenseDTOs)
        {
            Guid? userId = _userService.GetLoggedInUserId();
            if (!userId.HasValue)
                return Result<string>.Failure("There is issue with user credentials.Try again", HttpStatusCode.Unauthorized);

            if (expenseDTOs == null || !expenseDTOs.Any())
                return Result<string>.Failure("You must add at lease one entry", HttpStatusCode.BadRequest);

            List<Expense> expenseDetails = ConvertExpenseDTOIntoExpense(expenseDTOs, userId.Value);

            bool isStored = await _expenseRepository.StoreBulkOrSingleExpenseDetailsAsync(expenseDetails);

            if (!isStored)
                return Result<string>.Failure("Couldn't to make the task. Please try again.", HttpStatusCode.InternalServerError);

            return Result<string>.Success("Income details added successfully.", HttpStatusCode.OK);
        }

        private List<Expense> ConvertExpenseDTOIntoExpense(List<ExpenseDTO> expenseDTOs, Guid userId)
        {
            return expenseDTOs.Select(expenseDTO => new Expense
            {
                ExpenseId = Guid.NewGuid(),
                UserId = userId,
                ExpenseName = expenseDTO.ExpenseName,
                ExpenseAmount = expenseDTO.ExpenseAmount,
                ExpenseDescription = expenseDTO.ExpenseDescription,
                ExpenseTypeId = expenseDTO.ExpenseTypeId,
                PaymentMethodId = expenseDTO.PaymentMethodId,
                ExpenseDate = expenseDTO.ExpenseDate,
                CreatedAt = DateTime.UtcNow
            }).ToList();
        }

        public async Task<Result<object>> UpdateExpenseAsync(ExpenseDTO expenseDTO)
        {
            Guid? userId = _userService.GetLoggedInUserId();

            if (userId == null)
                return Result<Object>.Failure("There is issue with user credentials.Try again", HttpStatusCode.Unauthorized);

            if (expenseDTO == null)
                return Result<Object>.Failure("You must create a update on an entry", HttpStatusCode.BadRequest);

            List<ExpenseDTO> expenseDtos = new List<ExpenseDTO> { expenseDTO };

            List<Expense> expenseDetailsBulk = ConvertExpenseDTOIntoExpense(expenseDtos, userId.Value);
            Expense expense = expenseDetailsBulk.FirstOrDefault();

            expense.UpdatedAt = DateTime.UtcNow;

            bool isUpdated = await _expenseRepository.UpdateExpenseDetailsAsync(expense);

            if (!isUpdated)
                return Result<Object>.Failure("Couldn't to make the task. Please try again.", HttpStatusCode.InternalServerError);

            return Result<Object>.Success(expenseDTO);
        }
    }
}
