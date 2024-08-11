using dss_common.Extensions;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using nidirect_pointer_poc_core.DTOs;
using nidirect_pointer_poc_infrastructure.Features.Pointer.Queries;

namespace nidirect_pointer_poc_api.Controllers;

/// <summary>
/// Pointer controller
/// </summary>
// [ServiceFilter(typeof(ApiKeyAuthFilter))] // uncomment to use api key authentication
[Authorize] // Uncomment to use Jwt authentication
[Route("api/v{version:apiVersion}/[controller]")]
[ApiController]
[ApiVersion("1.0")]
public class PointerController : ControllerBase
{
    private readonly IMediator _mediator;
    private const string MediaType = "application/json";

    /// <summary>
    /// Constructor
    /// </summary>
    /// <param name="mediator"></param>
    public PointerController(IMediator mediator)
    {
        _mediator = mediator;
    }

    // GET: api/Pointer/GetAll
    /// <summary>
    /// Gets a list a paged list of NI Addresses.
    /// </summary>
    /// <remarks>
    /// Sample Request: GET /Pointer/GetAll?PageNumber=1&amp;PageSize=10
    /// </remarks>
    /// <returns>A paged list of NI addresses.</returns>
    [HttpGet(nameof(GetAll))]
    [Produces(MediaType)]
    [ProducesResponseType(typeof(PointerDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status429TooManyRequests)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async ValueTask<IActionResult> GetAll([FromQuery] GetAllQuery request,
        CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(request, cancellationToken: cancellationToken).ConfigureAwait(false);
        return result.OnBoth(r => r.IsSuccess ? (IActionResult)Ok(r.Value) : NotFound(r.Error));
    }

    // GET: api/Pointer/GetAddressByPostcode
    /// <summary>
    /// Gets a list of addresses by postcode
    /// </summary>
    /// <remarks>
    /// Sample Request: GET /Pointer/GetAddressByPostcode?PostCode=bt11aa
    /// </remarks>
    /// <returns>A list of NI addresses by postcode</returns>
    [HttpGet(nameof(GetAddressByPostcode))]
    [Produces(MediaType)]
    [ProducesResponseType(typeof(PointerDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status429TooManyRequests)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async ValueTask<IActionResult> GetAddressByPostcode([FromQuery] GetAddressByPostCodeQuery request,
        CancellationToken cancellationToken)
    {
        var result = await _mediator
            .Send(new GetAddressByPostCodeQuery() { PostCode = request.PostCode }, cancellationToken)
            .ConfigureAwait(false);

        if (result.IsFailure)
        {
            return NotFound(result.Error);
        }

        return Ok(result.Value);
    }

    // GET: api/Pointer/GetByBuildingNumberPostCode
    /// <summary>
    /// Gets an address by postcode and building number
    /// </summary>
    /// <remarks>
    /// Sample Request: GET /Pointer/GetByBuildingNumberPostCode?PostCode=bt11aa&amp;BuildingNumber=20
    /// </remarks>
    /// <returns>An NI address by postcode and building number</returns>
    [HttpGet(nameof(GetByBuildingNumberPostCode))]
    [Produces(MediaType)]
    [ProducesResponseType(typeof(PointerDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status429TooManyRequests)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async ValueTask<IActionResult> GetByBuildingNumberPostCode(
        [FromQuery] GetByBuildingNumberPostCodeQuery request, CancellationToken cancellationToken)
    {
        var result = await _mediator
            .Send(
                new GetByBuildingNumberPostCodeQuery()
                { PostCode = request.PostCode, BuildingNumber = request.BuildingNumber }, cancellationToken)
            .ConfigureAwait(false);

        if (result.IsFailure)
        {
            return NotFound(result.Error);
        }

        return Ok(result.Value);
    }
}