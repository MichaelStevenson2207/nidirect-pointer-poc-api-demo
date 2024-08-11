using AutoMapper;
using nidirect_pointer_poc_core.DTOs;
using nidirect_pointer_poc_infrastructure.Features.Common;
using nidirect_pointer_poc_infrastructure.Features.Pointer.Queries;

namespace nidirect_pointer_poc_infrastructure.Features.Pointer.Mapping;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<PointerDto, nidirect_pointer_poc_core.Entities.Pointer>()
            .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));

        CreateMap<nidirect_pointer_poc_core.Entities.Pointer, PointerDto>()
            .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));

        CreateMap<GetAllQuery, PaginationDetails>();
    }
}