using Application.Core;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Domain;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Persistence;

namespace Application.Activities
{
    public class List
    {
        public class Query : IRequest<Result<List<ActivityDto>>> {}

        public class Handler : IRequestHandler<Query, Result<List<ActivityDto>>>
        {
            //siccome vogliamo ottenere la lista di Axctivity, utilizziamo anche il constructor
            private readonly DataContext _context;
            private readonly ILogger<Handler> _logger;
            private readonly IMapper _mapper;
            public Handler(DataContext context, ILogger<Handler> logger, IMapper mapper)
            {
                _mapper = mapper;
                _logger = logger;
                _context = context;                
            }

            public async Task<Result<List<ActivityDto>>> Handle(Query request, CancellationToken token)
            { 

                try
                {
                    var activities = await _context.Activities
                        .ProjectTo<ActivityDto>(_mapper.ConfigurationProvider)
                        .ToListAsync(token);    

                    return Result<List<ActivityDto>>.Success(activities);
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