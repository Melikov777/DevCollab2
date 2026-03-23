using AutoMapper;
using DevCollab.Application.DTOs;
using DevCollab.Application.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace DevCollab.Application.Features.Reviews.Queries;

public class GetReviewsByProjectQueryHandler : IRequestHandler<GetReviewsByProjectQuery, List<ReviewDto>>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public GetReviewsByProjectQueryHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<List<ReviewDto>> Handle(GetReviewsByProjectQuery request, CancellationToken cancellationToken)
    {
        var reviews = await _context.Reviews
            .Include(r => r.Reviewer)
            .Where(r => r.ProjectId == request.ProjectId)
            .OrderByDescending(r => r.CreatedAt)
            .ToListAsync(cancellationToken);

        return _mapper.Map<List<ReviewDto>>(reviews);
    }
}
