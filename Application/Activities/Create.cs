using Application.Core;
using Application.Interfaces;
using Domain;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Persistence;

namespace Application.Activities
{
    public class Create
    {
        //differenza tra Query e Command che Command non ritorna nessun valore
        //infatti non mettiamo il tipo tra parentesi angolari
        public class Command : IRequest<Result<Unit>>
        {
            public Activity Activity { get; set; }
        }

        public class CommandValidator : AbstractValidator<Command> 
        {
            public CommandValidator()
            {
                RuleFor(x => x.Activity).SetValidator(new ActivityValidator());
            }
        }

        public class Handler : IRequestHandler<Command, Result<Unit>>
        {
            private readonly DataContext _context;
            private readonly ILogger<Handler> _logger;
            private readonly IUserAccessor _userAccessor;

            public Handler(DataContext context,  ILogger<Handler> logger, IUserAccessor userAccessor)
            {
                _userAccessor = userAccessor;
                _context = context;
                _logger = logger;
            }

            public async Task<Result<Unit>> Handle(Command request, CancellationToken cancellationToken)
            {
                //aggiungiuamo Activity in memory quindi non usiamo il metodo Async
                //siccome non stiamo toccando il DB

                try
                {
                    var user = await _context.Users.FirstOrDefaultAsync(x => 
                    x.UserName == _userAccessor.GetUsername());
                    
                    var attendee = new ActivityAttendee
                    {
                        AppUser = user,
                        Activity = request.Activity,
                        IsHost = true
                    };

                    request.Activity.Attendees.Add(attendee);

                    _context.Activities.Add(request.Activity);

                    var result = await _context.SaveChangesAsync() > 0;

                    if(!result) return Result<Unit>.Failure("Failed to create an Activity");
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, $"{ex.Message} -StackTrace: {ex.StackTrace}");
                }
                finally{
                    _logger.LogInformation($"\n\nAttempted a create activity with id: {request.Activity.Id}\n");
                }                

                    return Result<Unit>.Success(Unit.Value);

            }
        }
    }
}