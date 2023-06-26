using CSharpFunctionalExtensions;
using GlobalBlueProject.Entities;
using GlobalBlueProject.Models;
using MediatR;

namespace GlobalBlueProject.Services.Purchases.Queries
{
    #region Request
    public sealed class AustriaPurchaseAmountsQuery : IRequest<Result<PurchaseAmountsDTO>>
    {
        public int VatRate { get; set; }
        public decimal? NetValue { get; set; }
        public decimal? GrossValue { get; set; }
        public decimal? VatValue { get; set; }

        public AustriaPurchaseAmountsQuery(int vatRate, decimal? netValue, decimal? grossValue, decimal? vatValue)
        {
            VatRate = vatRate;
            NetValue = netValue;
            GrossValue = grossValue;
            VatValue = vatValue;
        }
    }
    #endregion

    #region RequestHandler
    public sealed class AustriaPurchaseAmountsQueryHandler : IRequestHandler<AustriaPurchaseAmountsQuery, Result<PurchaseAmountsDTO>>
    {
        public async Task<Result<PurchaseAmountsDTO>> Handle(AustriaPurchaseAmountsQuery request, CancellationToken cancellationToken)
        {
            AustriaPurchaseAmountEntity entity = new(request.VatRate, request.NetValue, request.GrossValue, request.VatValue);

            if (entity.Validations.IsFailure)
                return Result.Failure<PurchaseAmountsDTO>(entity.Validations.Error);

            PurchaseAmountsDTO result =
                new()
                {
                    VatRate = entity.VatRate,
                    NetValue = entity.NetValue!.Value,
                    GrossValue = entity.GrossValue!.Value,
                    VatValue = entity.VatValue!.Value
                };

            await Task.CompletedTask;

            return Result.Success(result);
        }
    }
    #endregion
}
