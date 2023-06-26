using FluentAssertions;
using GlobalBlueProject.Models;
using GlobalBlueProject.Services.Purchases.Queries;

namespace GlobalBlueProject.Tests
{
    public class AustriaPurchaseAmountsTest
    {
        #region Service_should_return_error_if_vat_rate_is_invalid
        [Theory]
        [ClassData(typeof(DateGeneratorInvalidVatRates))]
        public async void Service_should_return_error_if_vat_rate_is_invalid(int vatRate, decimal? netValue, decimal? grossValue, decimal? vatValue)
        {
            // Arrange
            AustriaPurchaseAmountsQuery query = new(vatRate, netValue, grossValue, vatValue);
            AustriaPurchaseAmountsQueryHandler handler = new();

            // Act
            var act = await handler.Handle(query, CancellationToken.None);

            // Assert
            act.IsFailure.Should().BeTrue();
            act.Error.Should().Contain("VAT rate must be a valid value [10 or 13 or 20].");
        }
        #endregion

        #region Service_should_return_error_if_more_than_one_value_is_provided
        [Theory]
        [ClassData(typeof(DateGeneratorMoreThanOneValue))]
        public async void Service_should_return_error_if_more_than_one_value_is_provided(int vatRate, decimal? netValue, decimal? grossValue, decimal? vatValue)
        {
            // Arrange
            AustriaPurchaseAmountsQuery query = new(vatRate, netValue, grossValue, vatValue);
            AustriaPurchaseAmountsQueryHandler handler = new();

            // Act
            var act = await handler.Handle(query, CancellationToken.None);

            // Assert
            act.IsFailure.Should().BeTrue();
            act.Error.Should().Contain("Only one of the amounts values should be provided [NetValue or GrossValue or VatValue].");
        }
        #endregion

        #region Service_should_return_error_if_value_is_not_greater_than_zero
        [Theory]
        [ClassData(typeof(DateGeneratorInvalidValues))]
        public async void Service_should_return_error_if_value_is_not_greater_than_zero(int vatRate, decimal? netValue, decimal? grossValue, decimal? vatValue)
        {
            // Arrange
            AustriaPurchaseAmountsQuery query = new(vatRate, netValue, grossValue, vatValue);
            AustriaPurchaseAmountsQueryHandler handler = new();

            // Act
            var act = await handler.Handle(query, CancellationToken.None);

            // Assert
            act.IsFailure.Should().BeTrue();
            act.Error.Should().Contain("Amount values must be greater than zero.");
        }
        #endregion

        #region Service_should_return_expected_values
        [Theory]
        [ClassData(typeof(DateGeneratorExpectedValues))]
        public async void Service_should_return_expected_values(int vatRate, decimal? netValue, decimal? grossValue, decimal? vatValue, decimal resultNetValue, decimal resultGrossValue, decimal resultVatValue)
        {
            // Arrange
            AustriaPurchaseAmountsQuery query = new(vatRate, netValue, grossValue, vatValue);
            AustriaPurchaseAmountsQueryHandler handler = new();

            PurchaseAmountsDTO expectedResult =
                new()
                {
                    VatRate = vatRate,
                    NetValue = resultNetValue,
                    GrossValue = resultGrossValue,
                    VatValue = resultVatValue
                };

            // Act
            var act = await handler.Handle(query, CancellationToken.None);

            // Assert
            act.IsSuccess.Should().BeTrue();
            act.Value.Should().BeEquivalentTo(expectedResult);
        }
        #endregion

        #region private

        private class DateGeneratorInvalidVatRates : TheoryData<int, decimal?, decimal?, decimal?>
        {
            public DateGeneratorInvalidVatRates()
            {
                Add(9, null, null, null);
                Add(11, null, null, null);
                Add(12, null, null, null);
                Add(14, null, null, null);
                Add(19, null, null, null);
                Add(21, null, null, null);
            }
        }

        private class DateGeneratorMoreThanOneValue : TheoryData<int, decimal?, decimal?, decimal?>
        {
            public DateGeneratorMoreThanOneValue()
            {
                Add(10, 100, 100, null);
                Add(13, 100, null, 1000);
                Add(20, null, 100, 100);
            }
        }

        private class DateGeneratorInvalidValues : TheoryData<int, decimal?, decimal?, decimal?>
        {
            public DateGeneratorInvalidValues()
            {
                Add(10, null, null, 0);
                Add(13, null, null, -1);
                Add(20, null, null, -100);
                Add(10, null, 0, null);
                Add(13, null, -1, null);
                Add(20, null, -100, null);
                Add(10, 0, null, null);
                Add(13, -1, null, null);
                Add(20, -100, null, null);
            }
        }

        private class DateGeneratorExpectedValues : TheoryData<int, decimal?, decimal?, decimal?, decimal, decimal, decimal>
        {
            public DateGeneratorExpectedValues()
            {
                Add(10, null, null, 135, 1350, 1485, 135);
                Add(20, 245, null, null, 245, 294, 49);
                Add(13, null, 1832, null, 1621.24M, 1832, 210.76M);
            }
        }

        #endregion
    }
}