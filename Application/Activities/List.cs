using Application.Core;
using Domain;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Persistence;

namespace Application.Activities
{
    public class List
    {
        public class Query : IRequest<Result<List<Activity>>> {}

        public class Handler : IRequestHandler<Query, Result<List<Activity>>>
        {
            //siccome vogliamo ottenere la lista di Axctivity, utilizziamo anche il constructor
            private readonly DataContext _context;
            private readonly ILogger<Handler> _logger;
            public Handler(DataContext context, ILogger<Handler> logger)
            {
                _logger = logger;
                _context = context;                
            }

            public async Task<Result<List<Activity>>> Handle(Query request, CancellationToken token)
            { 

                try
                {
                    return Result<List<Activity>>.Success(await _context.Activities.ToListAsync());
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, $"{ex.Message} -StackTrace: {ex.StackTrace}");
                    throw;
                }
                finally{
                    _logger.LogInformation("\n\nAttempted get all Activity\n");
                }
               
            }
        }
    }
}