using Application.Core;
using Domain;
using MediatR;
using Microsoft.Extensions.Logging;
using Persistence;

namespace Application.Activities
{
    public class Details
    {
        public class Query : IRequest<Result<Activity>>
        {
            public Guid Id { get; set; }
        }

        public class Handler : IRequestHandler<Query, Result<Activity>>
        {
            private readonly DataContext _context;
            private readonly ILogger<Handler> _logger;
            public Handler(DataContext context, ILogger<Handler> logger)
            {
                _logger = logger;
                _context = context;                
            }

            public async Task<Result<Activity>> Handle(Query request, CancellationToken cancellationToken)
            {
                try
                {
                     var activity =  await _context.Activities.FindAsync(request.Id);

                     return Result<Activity>.Success(activity);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, $"{ex.Message} -StackTrace: {ex.StackTrace}");
                    throw;
                }
                finally
                {
                    _logger.LogInformation($"\n\nAttempted get a single Activity with id: {request.Id}\n");
                }
                
               
            }
        }
    }
}