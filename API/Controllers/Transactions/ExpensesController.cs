using Microsoft.AspNetCore.Mvc;
using StackExchange.Redis;
using WealthFlow.Application.Transactions.DTOs;
using WealthFlow.Application.Transactions.Interfaces;
using WealthFlow.Shared.Helpers;
using static WealthFlow.Domain.Enums.Enum;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace WealthFlow.API.Controllers.Transactions
{
    [Route("api/[controller]")]
    [ApiController]
    public class ExpensesController : ControllerBase
    {
        private readonly IExpenseService _expenseService;

        public ExpensesController(IExpenseService expenseService)
        {
            _expenseService = expenseService;
        }

        [HttpPost("store")]
        public async Task<Result<string>> StoreExpenseAsync([FromBody] List<ExpenseDTO> expenseDTOs)
        {
            return await _expenseService.StoreBulkOrSingleExpenseDetailsAsync(expenseDTOs);
        }

        [HttpPost("update")]
        public async Task<Result<Object>> UpdateIncomeAsync([FromBody] ExpenseDTO expenseDTO)
        {
            return await _expenseService.UpdateExpenseAsync(expenseDTO);
        }

        [HttpGet("all")]
        public async Task<Result<Object>> GetAllIncomeDetailsAsync(int pageNumber, int pageSize, SortBy sort, SortOrderBy order)
        {
            return await _expenseService.GetAllExpenseDetailsAsync(pageNumber, pageSize, sort, order);
        }

        [HttpGet("date-range")]
        public async Task<Result<Object>> GetDateSpeciftcIncomeDetailsAsync(DateTime startingDate, DateTime endDate)
        {
            return await _expenseService.GetDateSpecificExpenseDetailsAsync(startingDate, endDate);
        }
    }
}
