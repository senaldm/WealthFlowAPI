using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WealthFlow.Shared.Helpers;
using WealthFlow.Application.Transactions.DTOs;
using WealthFlow.Application.Transactions.Interfaces;
using static WealthFlow.Domain.Enums.Enum;

namespace WealthFlow.API.Controllers.Transactions
{
    [Route("api/income")]
    [ApiController]

    public class IncomeController : ControllerBase
    {
        private readonly IIncomeService _incomeService;

        public IncomeController(IIncomeService incomeService)
        {
            _incomeService = incomeService;
        }

        [HttpPost("store")]
        public async Task<Result<string>> StoreIncomeAsync([FromBody] IncomeDTO incomeDTO)
        {
            return await _incomeService.StoreIncomeAsync(incomeDTO);
        }

        [HttpPost("update")]
        public async Task<Result<Object>> UpdateIncomeAsync([FromBody] IncomeDTO incomeDTO)
        {
            return await _incomeService.UpdateIncomeAsync(incomeDTO);
        }

        [HttpGet("all")]
        public async Task<Result<Object>> GetAllIncomeDetailsAsync(int pageNumber, int pageSize, SortBy sortBy, SortOrderBy orderBy)
        {
            return await _incomeService.GetAllIncomeDetailsAsync(pageNumber, pageSize, sortBy, orderBy);
        }

        [HttpGet("date-range")]
        public async Task<Result<Object>> GetDateSpeciftcIncomeDetailsAsync(DateTime startingDate, DateTime endDate)
        {
            return await _incomeService.GetDateSpecificIncomeDetailsAsync(startingDate, endDate);
        }
    }
}
