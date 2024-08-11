using AutoMapper;
using dss_common.Functional;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;
using nidirect_pointer_poc_core.DTOs;
using nidirect_pointer_poc_infrastructure.Data;
using nidirect_pointer_poc_infrastructure.Features.Common;
using nidirect_pointer_poc_infrastructure.Features.Extensions;

namespace nidirect_pointer_poc_infrastructure.Features.Pointer.Queries;

public class GetAddressByPostCodeQuery : IRequest<Result<List<PointerDto>>>
{
    public required string PostCode { get; set; }
}

public class GetAddressByPostCodeQueryHandler : AbstractBaseHandler<GetAddressByPostCodeQuery, Result<List<PointerDto>>>
{
    public GetAddressByPostCodeQueryHandler(PointerContext pointerContext, IMapper mapper, IValidator<GetAddressByPostCodeQuery> validator, IDistributedCache cache)
        : base(pointerContext, mapper, validator, cache)
    {

    }

    public override async Task<Result<List<PointerDto>>> Handle(GetAddressByPostCodeQuery request, CancellationToken cancellationToken)
    {
        var validationResult = await Validator.ValidateAsync(request, cancellationToken);

        if (!validationResult.IsValid)
            return Result.Fail<List<PointerDto>>(validationResult.ToString());

        var postCode = request.PostCode.ToUpper();

        var getAddresses = await Cache.GetRecordAsync<PointerDto[]>(postCode);

        if (getAddresses is null)
        {
            var maybeAddress = await PointerContext.Pointer.AsNoTracking()
                .Where(i => i.Postcode == StringExtensions.FormatPostCode(request.PostCode) && i.BuildingStatus!.Equals("BUILT") && i.AddressStatus!.Equals("APPROVED"))
                .OrderBy(i => i.BuildingNumber)
                .ToListAsync(cancellationToken: cancellationToken);

            if (maybeAddress.Count == 0)
                return Result.Fail<List<PointerDto>>("Postcode not found");

            var apiAddress = Mapper.Map<List<PointerDto>>(maybeAddress);

            if (apiAddress.Count == 0)
            {
                return Result.Fail<List<PointerDto>>("Postcode not found");
            }

            await Cache.SetRecordAsync(postCode, apiAddress);

            return Result.Ok(apiAddress);
        }

        var address = Mapper.Map<List<PointerDto>>(getAddresses);

        return Result.Ok(address);
    }
}