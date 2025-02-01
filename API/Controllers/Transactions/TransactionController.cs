using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using static WealthFlow.Domain.Enums.Enum;
using WealthFlow.Shared.Helpers;

namespace WealthFlow.API.Controllers.Transactions
{
    [Route("api/transactions")]
    [ApiController]
    public class TransactionController : ControllerBase
    {
        [HttpGet("sort-options")]

        public Result<Object> GetSortOptions()
        {
            var sortByOptions = Enum.GetValues(typeof(SortBy)).Cast<SortBy>().ToList();
            var sortOrderByOptions = Enum.GetValues(typeof(SortOrderBy)).Cast<SortOrderBy>().ToList();

            var sortOptions = new
            {
                sortBy = sortByOptions,
                sortOrderBy = sortOrderByOptions
            };

            return Result<Object>.Success(sortOptions);
        }
    }
}
