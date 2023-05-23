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

namespace nidirect_pointer_poc_infrastructure.Features.Pointer.Queries
{
    public class GetByBuildingNumberPostCodeQuery : IRequest<Result<PointerDto>>
    {
        public required string PostCode { get; set; }
        public required string BuildingNumber { get; set; }
    }

    public class GetByBuildingNumberPostCodeQueryHandler : AbstractBaseHandler<GetByBuildingNumberPostCodeQuery, Result<PointerDto>>
    {
        public GetByBuildingNumberPostCodeQueryHandler(PointerContext pointerContext, IMapper mapper, IValidator<GetByBuildingNumberPostCodeQuery> validator, IDistributedCache cache) : base(pointerContext, mapper, validator, cache)
        {

        }

        public override async Task<Result<PointerDto>> Handle(GetByBuildingNumberPostCodeQuery request, CancellationToken cancellationToken)
        {
            var validationResult = await Validator.ValidateAsync(request, cancellationToken);

            if (!validationResult.IsValid)
                return Result.Fail<PointerDto>(validationResult.ToString());

            var postCode = request.PostCode.ToUpper();
            var buildingNumber = request.BuildingNumber.ToUpper();

            var getAddresses = await Cache.GetRecordAsync<PointerDto>($"{buildingNumber}_{postCode}");

            if (getAddresses is null)
            {
                var maybeAddress = await PointerContext.Pointer.AsNoTracking()
                    .Where(i => i.Postcode == StringExtensions.FormatPostCode(postCode) && i.BuildingNumber == buildingNumber && i.BuildingStatus!.Equals("BUILT") && i.AddressStatus!.Equals("APPROVED"))
                    .OrderBy(i => i.BuildingNumber)
                    .FirstOrDefaultAsync(cancellationToken: cancellationToken);

                if (maybeAddress == null)
                    return Result.Fail<PointerDto>("Address not found");

                var apiAddress = Mapper.Map<PointerDto>(maybeAddress);

                await Cache.SetRecordAsync($"{buildingNumber}_{postCode}", apiAddress);

                return Result.Ok(apiAddress);
            }

            var address = Mapper.Map<PointerDto>(getAddresses);

            return Result.Ok(address);
        }
    }
}