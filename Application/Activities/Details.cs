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
    public class Details
    {
        public class Query : IRequest<Result<ActivityDto>>
        {
            public Guid Id { get; set; }
        }

        public class Handler : IRequestHandler<Query, Result<ActivityDto>>
        {
            private readonly DataContext _context;
            private readonly ILogger<Handler> _logger;
        private readonly IMapper _mapper;
            public Handler(DataContext context, ILogger<Handler> logger, IMapper mapper)
            {
                _mapper = mapper;
                _logger = logger;
                _context = context;                
            }

            public async Task<Result<ActivityDto>> Handle(Query request, CancellationToken cancellationToken)
            {
                try
                {
                    //ProjectTo è un metodo fornito da AutoMapper,
                    // una libreria utilizzata per mappare automaticamente un tipo di oggetto in un altro.
                     var activity =  await _context.Activities
                     .ProjectTo<ActivityDto>(_mapper.ConfigurationProvider)
                     .FirstOrDefaultAsync(x => x.Id == request.Id);

                     return Result<ActivityDto>.Success(activity);
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