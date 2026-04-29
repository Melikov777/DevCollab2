using AutoMapper;
using DevCollab.Application.DTOs;
using DevCollab.Application.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

using AutoMapper.QueryableExtensions;

namespace DevCollab.Application.Features.Reviews.Queries;

public class GetReviewsByProjectQueryHandler : IRequestHandler<GetReviewsByProjectQuery, PagedResult<ReviewDto>>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public GetReviewsByProjectQueryHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<PagedResult<ReviewDto>> Handle(GetReviewsByProjectQuery request, CancellationToken cancellationToken)
    {
        var query = _context.Reviews
            .Where(r => r.ProjectId == request.ProjectId)
            .OrderByDescending(r => r.CreatedAt);

        var totalCount = await query.CountAsync(cancellationToken);

        var items = await query
            .Skip((request.PageNumber - 1) * request.PageSize)
            .Take(request.PageSize)
            .ProjectTo<ReviewDto>(_mapper.ConfigurationProvider)
            .ToListAsync(cancellationToken);

        return new PagedResult<ReviewDto>
        {
            Items = items,
            TotalCount = totalCount,
            PageNumber = request.PageNumber,
            PageSize = request.PageSize
        };
    }
}
