using Application.Core;
using MediatR;
using Microsoft.Extensions.Logging;
using Persistence;

namespace Application.Activities
{
    public class Delete
    {
        public class Command : IRequest<Result<Unit>>
        {
            public Guid Id { get; set; }
        }

        public class Handler : IRequestHandler<Command, Result<Unit>>
        {
            private readonly DataContext _context;
            private readonly ILogger<Handler> _logger;
            public Handler(DataContext context, ILogger<Handler> logger)
            {
                 _logger = logger;
                 _context = context;
            }

            public async Task<Result<Unit>> Handle(Command request, CancellationToken cancellationToken)
            {
                // try
                // {
                    var activity = await _context.Activities.FindAsync(request.Id);

                    if(activity == null) return null;

                    _context.Remove(activity);   

                    var result = await _context.SaveChangesAsync() > 0; 
                    
                    if(!result) return Result<Unit>.Failure("Failed to delete an Activity");                
                // }
                // catch (Exception ex)
                // {
                //     _logger.LogError(ex, $"{ex.Message} -StackTrace: {ex.StackTrace}");

                // }finally{
                    
                    _logger.LogInformation($"\n\nAttempted delete an Activity\n");
                // }

                return Result<Unit>.Success(Unit.Value);

            }
        }
    }
}

