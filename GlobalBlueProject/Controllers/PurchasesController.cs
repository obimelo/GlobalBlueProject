using CSharpFunctionalExtensions;
using GlobalBlueProject.Base;
using GlobalBlueProject.Models;
using GlobalBlueProject.Services.Purchases.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace GlobalBlueProject.Controllers
{
    [Route("api/v1/purchases")]
    public sealed class PurchasesController : BaseController
    {
        #region Private Variables
        private readonly IMediator _mediator;
        #endregion

        #region Constructor
        public PurchasesController(IMediator mediator) => _mediator = mediator;
        #endregion

        #region Gets

        /// <summary>
        /// Returns the amounts [Net, Gross and VAT] of a purchase in Austria
        /// </summary>
        /// <param name="vatRate">VAT rate</param>
        /// <param name="netValue">Base Net Value</param>
        /// <param name="grossValue">Base Gross Value</param>
        /// <param name="vatValue">Base VAT Value</param>
        /// <returns></returns>
        [HttpGet("austriaPurchaseAmounts/{vatRate}")]
        [Produces("application/json")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Envelope<PurchaseAmountsDTO>))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(Envelope<PurchaseAmountsDTO>))]
        public async Task<ActionResult<Envelope<PurchaseAmountsDTO>>> AustriaPurchaseAmounts
            (
                [FromRoute] int vatRate,
                [FromQuery] decimal? netValue,
                [FromQuery] decimal? grossValue,
                [FromQuery] decimal? vatValue
            )
        {
            Result<PurchaseAmountsDTO> result =
                await _mediator.Send(new AustriaPurchaseAmountsQuery(vatRate, netValue, grossValue, vatValue));

            return OkFromResult(result);
        }

        #endregion
    }
}
