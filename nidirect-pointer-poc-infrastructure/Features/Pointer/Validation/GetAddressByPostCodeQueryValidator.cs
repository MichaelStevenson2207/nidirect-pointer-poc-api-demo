using FluentValidation;
using nidirect_pointer_poc_infrastructure.Features.Pointer.Queries;

namespace nidirect_pointer_poc_infrastructure.Features.Pointer.Validation
{
    public class GetAddressByPostCodeQueryValidator : AbstractValidator<GetAddressByPostCodeQuery>
    {
        public GetAddressByPostCodeQueryValidator()
        {
            RuleFor(x => x.PostCode).NotNull().WithMessage("Postcode not set");
        }
    }
}