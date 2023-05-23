using FluentValidation;
using nidirect_pointer_poc_infrastructure.Features.Pointer.Queries;

namespace nidirect_pointer_poc_infrastructure.Features.Pointer.Validation
{
    public class GetByBuildingNumberPostCodeQueryValidator : AbstractValidator<GetByBuildingNumberPostCodeQuery>
    {
        public GetByBuildingNumberPostCodeQueryValidator()
        {
            RuleFor(x => x.PostCode).NotNull().WithMessage("Postcode not set");
            RuleFor(x => x.BuildingNumber).NotNull().WithMessage("Building number not set");
        }
    }
}