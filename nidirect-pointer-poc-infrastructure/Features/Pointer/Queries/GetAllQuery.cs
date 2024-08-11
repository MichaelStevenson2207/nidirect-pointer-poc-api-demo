using AutoMapper;
using AutoMapper.QueryableExtensions;
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

public class GetAllQuery : IRequest<Result<PagedResult<PointerDto>>>
{
    public GetAllQuery()
    {
        PageNumber = 1;
        PageSize = 10;
    }
    public GetAllQuery(int pageNumber, int pageSize)
    {
        PageNumber = pageNumber;
        PageSize = pageSize;
    }

    public required int PageNumber { get; set; }
    public required int PageSize { get; set; }
}

public class GetAllQueryValidator : AbstractValidator<GetAllQuery>
{
    public GetAllQueryValidator()
    {
        RuleFor(m => m.PageNumber).GreaterThan(0);
    }
}

public class GetAllQueryHandler : IRequestHandler<GetAllQuery, Result<PagedResult<PointerDto>>>
{
    private readonly PointerContext _pointerContext;
    private readonly IMapper _mapper;
    private readonly IUriService _uriService;
    private readonly IDistributedCache _cache;

    public GetAllQueryHandler(PointerContext pointerContext, IMapper mapper, IUriService uriService, IDistributedCache cache)
    {
        _pointerContext = pointerContext;
        _mapper = mapper;
        _uriService = uriService;
        _cache = cache;
    }

    public async Task<Result<PagedResult<PointerDto>>> Handle(GetAllQuery request, CancellationToken cancellationToken)
    {
        var key = $"GetAll_{request.PageNumber}_{request.PageSize}";

        var getAddresses = await _cache.GetRecordAsync<PagedResult<PointerDto>>(key);

        if (getAddresses is null)
        {
            var pagination = await _pointerContext.Pointer.AsQueryable()
                .CountAsync(cancellationToken);

            var addresses = await _pointerContext.Pointer.AsQueryable()
                .OrderBy(p => p.Id)
                .Skip((request.PageNumber - 1) * request.PageSize)
                .Take(request.PageSize)
                .ProjectTo<PointerDto>(_mapper.ConfigurationProvider)
                .ToListAsync(cancellationToken)
                .ConfigureAwait(false);

            var paginationDetails = new PaginationDetails
            {
                PageNumber = request.PageNumber,
                PageSize = request.PageSize
            };

            paginationDetails.WithTotal(pagination);

            var paginationResponse = PagedResult<PointerDto>.CreatePaginatedResponse(_uriService, paginationDetails, addresses);

            await _cache.SetRecordAsync(key, paginationResponse);

            return Result.Ok(paginationResponse);
        }

        return Result.Ok(getAddresses);
    }
}