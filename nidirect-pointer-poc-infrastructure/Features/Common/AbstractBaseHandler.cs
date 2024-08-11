using AutoMapper;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.Caching.Distributed;
using nidirect_pointer_poc_infrastructure.Data;

namespace nidirect_pointer_poc_infrastructure.Features.Common;

public abstract class AbstractBaseHandler<T, TR> : IRequestHandler<T, TR> where T : IRequest<TR>
{
    protected readonly PointerContext PointerContext;
    protected readonly IValidator<T> Validator;
    protected readonly IMapper Mapper;
    protected readonly IDistributedCache Cache;

    protected AbstractBaseHandler(PointerContext pointerContext, IMapper mapper, IValidator<T> validator, IDistributedCache cache)
    {
        Mapper = mapper;
        PointerContext = pointerContext;
        Validator = validator;
        Cache = cache;
    }

    public abstract Task<TR> Handle(T request, CancellationToken cancellationToken);
}