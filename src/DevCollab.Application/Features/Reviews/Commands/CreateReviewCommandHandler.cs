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
    private readonly IApplicationDbContext _context; // Needed for project existence check and eager loading
    private readonly IRepository<Review> _repository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public CreateReviewCommandHandler(IApplicationDbContext context, IRepository<Review> repository, IUnitOfWork unitOfWork, IMapper mapper)
    {
        _context = context;
        _repository = repository;
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<ReviewDto> Handle(CreateReviewCommand request, CancellationToken cancellationToken)
    {
        var project = await _context.Projects
            .FirstOrDefaultAsync(p => p.Id == request.CreateReviewDto.ProjectId, cancellationToken);

        if (project == null)
            throw new NotFoundException(nameof(Domain.Entities.Project), request.CreateReviewDto.ProjectId);

        var review = _mapper.Map<Review>(request.CreateReviewDto);
        review.ReviewerId = request.ReviewerId;

        await _repository.AddAsync(review, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        // Include reviewer for mapping
        var createdReview = await _context.Reviews
            .Include(r => r.Reviewer)
            .FirstAsync(r => r.Id == review.Id, cancellationToken);

        return _mapper.Map<ReviewDto>(createdReview);
    }
}
