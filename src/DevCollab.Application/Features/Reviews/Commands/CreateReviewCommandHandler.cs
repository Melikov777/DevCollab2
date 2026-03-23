using AutoMapper;
using DevCollab.Application.DTOs;
using DevCollab.Application.Interfaces;
using DevCollab.Domain.Entities;
using DevCollab.Domain.Exceptions;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace DevCollab.Application.Features.Reviews.Commands;

public class CreateReviewCommandHandler : IRequestHandler<CreateReviewCommand, ReviewDto>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public CreateReviewCommandHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<ReviewDto> Handle(CreateReviewCommand request, CancellationToken cancellationToken)
    {
        var project = await _context.Projects.FindAsync(new object[] { request.CreateReviewDto.ProjectId }, cancellationToken);
        if (project == null)
            throw new NotFoundException(nameof(Domain.Entities.Project), request.CreateReviewDto.ProjectId);

        var review = _mapper.Map<Review>(request.CreateReviewDto);
        review.ReviewerId = request.ReviewerId;

        _context.Reviews.Add(review);
        await _context.SaveChangesAsync(cancellationToken);

        // Include reviewer for mapping
        var createdReview = await _context.Reviews
            .Include(r => r.Reviewer)
            .FirstAsync(r => r.Id == review.Id, cancellationToken);

        return _mapper.Map<ReviewDto>(createdReview);
    }
}
