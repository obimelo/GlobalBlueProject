using CSharpFunctionalExtensions;
using System.Text;

namespace GlobalBlueProject.Entities
{
    internal sealed class AustriaPurchaseAmountEntity
    {
        #region Private Variables
        private readonly List<int> _validVatRates = new() { 10, 13, 20 };
        private decimal VatRatePercent => VatRate / 100M + 1;
        #endregion

        #region Public Properties
        public int VatRate { get; }
        public decimal? NetValue { get; private set; }
        public decimal? GrossValue { get; private set; }
        public decimal? VatValue { get; private set; }
        public Result Validations { get; }
        #endregion

        #region Constructor
        public AustriaPurchaseAmountEntity(int vatRate, decimal? netValue, decimal? grossValue, decimal? vatValue)
        {
            VatRate = vatRate;
            NetValue = netValue;
            GrossValue = grossValue;
            VatValue = vatValue;

            Validations = DoValidations();

            if (Validations.IsSuccess)
                CalculateAmountValues();
        }
        #endregion

        #region private

        #region DoValidations
        private Result DoValidations()
        {
            StringBuilder errorMessage = new();

            if (!_validVatRates.Contains(VatRate))
                errorMessage.AppendLine($"- VAT rate must be a valid value [{string.Join(" or ", _validVatRates)}].");

            var amountValues = new List<decimal?> { NetValue, GrossValue, VatValue };

            if (amountValues.Where(x => x != null).Count() != 1)
                errorMessage.AppendLine("- Only one of the amounts values should be provided [NetValue or GrossValue or VatValue].");

            if (amountValues.Where(x => x != null && x <= 0).Any())
                errorMessage.AppendLine("- Amount values must be greater than zero.");

            if (errorMessage.Length > 0)
                return Result.Failure(errorMessage.ToString());

            return Result.Success();
        }
        #endregion

        #region CalculateAmountValues
        private void CalculateAmountValues()
        {
            if (NetValue != null)
            {
                NetValue = decimal.Round(NetValue.Value, 2);

                GrossValue = decimal.Round(NetValue.Value * VatRatePercent, 2);

                VatValue = GrossValue - NetValue;

                return;
            }

            if (GrossValue != null)
            {
                GrossValue = decimal.Round(GrossValue.Value, 2);

                NetValue = decimal.Round(GrossValue.Value / VatRatePercent, 2);

                VatValue = GrossValue - NetValue;

                return;
            }

            if (VatValue != null)
            {
                VatValue = decimal.Round(VatValue.Value, 2);

                NetValue = decimal.Round(VatValue.Value / (VatRatePercent - 1), 2);

                GrossValue = VatValue + NetValue;

                return;
            }
        }
        #endregion

        #endregion
    }
}
