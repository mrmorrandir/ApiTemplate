using ApiTemplate.Application.Common.Mappings;
using ApiTemplate.Application.Interfaces;
using ApiTemplate.Domain;

namespace ApiTemplate.Application.Samples.Commands.Add;

public class AddSampleCommand : IRequest<Result<Guid>>, IMapTo<Sample>
{
    public string Name { get; init; } = string.Empty;
}

public class AddSampleCommandHandler : IRequestHandler<AddSampleCommand, Result<Guid>>
{
    private readonly IMapper _mapper;
    private readonly ISampleDbContext _sampleDbContext;

    public AddSampleCommandHandler(IMapper mapper, ISampleDbContext sampleDbContext)
    {
        _mapper = mapper;
        _sampleDbContext = sampleDbContext;
    }
    
    public async Task<Result<Guid>> Handle(AddSampleCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var sample = _mapper.Map<Sample>(request);
            await _sampleDbContext.Samples.AddAsync(sample, cancellationToken);
            await _sampleDbContext.SaveChangesAsync(cancellationToken);
            return Result.Ok(sample.Id);
        }
        catch (Exception ex)
        {
            return Result.Fail<Guid>(new[] { "Error while adding sample", ex.Message });
        }
    }
}