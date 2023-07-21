using System;
using System.Linq;
using System.Linq.Expressions;
using Microsoft.AspNetCore.Mvc;

[Route("api/[controller]")]
[ApiController]
public class TransactionController : ControllerBase
{
    private readonly IGenericRepository<Transaction> _transactionRepository;

    public TransactionController(IGenericRepository<Transaction> transactionRepository)
    {
        _transactionRepository = transactionRepository;
    }

    [HttpGet]
    public IActionResult GetByParameter(
        string accountNumber,
        string referenceNumber,
        decimal? minAmountCredit,
        decimal? maxAmountCredit,
        decimal? minAmountDebit,
        decimal? maxAmountDebit,
        string description,
        DateTime? beginDate,
        DateTime? endDate)
    {
        // Filtre kriterlerine göre Lambda ifadesi oluşturma
        Expression<Func<Transaction, bool>> filter = transaction =>
            (string.IsNullOrEmpty(accountNumber) || transaction.AccountNumber == accountNumber) &&
            (string.IsNullOrEmpty(referenceNumber) || transaction.ReferenceNumber == referenceNumber) &&
            (!minAmountCredit.HasValue || transaction.AmountCredit >= minAmountCredit.Value) &&
            (!maxAmountCredit.HasValue || transaction.AmountCredit <= maxAmountCredit.Value) &&
            (!minAmountDebit.HasValue || transaction.AmountDebit >= minAmountDebit.Value) &&
            (!maxAmountDebit.HasValue || transaction.AmountDebit <= maxAmountDebit.Value) &&
            (string.IsNullOrEmpty(description) || transaction.Description == description) &&
            (!beginDate.HasValue || transaction.Date >= beginDate.Value) &&
            (!endDate.HasValue || transaction.Date <= endDate.Value);

        var transactions = _transactionRepository.GetByParameter(filter);
        return Ok(transactions);
    }
}

