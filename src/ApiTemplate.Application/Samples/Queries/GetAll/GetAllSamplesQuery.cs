using ApiTemplate.Application.Interfaces;
using AutoMapper.QueryableExtensions;

namespace ApiTemplate.Application.Samples.Queries.GetAll;

public class GetAllSamplesQuery : IRequest<Result<PaginatedList<SampleDto>>>, IPagination
{
    public string? Name { get; init; }
    public int? Offset { get; init; }
    public int? Limit { get; init; }
}

public class GetAllSamplesQueryHandler : IRequestHandler<GetAllSamplesQuery, Result<PaginatedList<SampleDto>>>
{
    private readonly IMapper _mapper;
    private readonly ISampleDbContext _sampleDbContext;

    public GetAllSamplesQueryHandler(IMapper mapper, ISampleDbContext sampleDbContext)
    {
        _mapper = mapper;
        _sampleDbContext = sampleDbContext;
    }

    public async Task<Result<PaginatedList<SampleDto>>> Handle(GetAllSamplesQuery request, CancellationToken cancellationToken)
    {
        try
        {
            var query = _sampleDbContext.Samples.AsQueryable();
            if (!string.IsNullOrWhiteSpace(request.Name))
                query = query.Where(s => s.Name.Contains(request.Name));

            var result = await query.OrderBy(s => s)
                .ProjectTo<SampleDto>(_mapper.ConfigurationProvider, cancellationToken)
                .PaginatedListAsync(request.Offset, request.Limit, cancellationToken);
            return Result.Ok(result);
        }
        catch (Exception ex)
        {
            return Result.Fail<PaginatedList<SampleDto>>(new[] { "Error while getting samples", ex.Message });
        }
    }
}