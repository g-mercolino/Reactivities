using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Application.Core;
using AutoMapper;
using Domain;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.Logging;
using Persistence;

namespace Application.Activities
{
    public class Edit
    {
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
            private readonly IMapper _mapper;
            private readonly ILogger<Handler> _logger;
            public Handler(DataContext context, IMapper mapper, ILogger<Handler> logger)
            {
                _mapper = mapper;
                _context = context;      
                _logger = logger;          
            }

            public async Task<Result<Unit>> Handle(Command request, CancellationToken cancellationToken)
            {
                var activity = await _context.Activities.FindAsync(request.Activity.Id);

                if(activity == null) return null;

                try
                {
                      _mapper.Map(request.Activity, activity);
      
                      var result = await _context.SaveChangesAsync() > 0;

                      if(!result) return Result<Unit>.Failure("Failed to edit an Activity");      
                }
                catch (Exception ex)
                {
                     _logger.LogError(ex, $"{ex.Message} -StackTrace: {ex.StackTrace}");
                     
                }finally {
                    _logger.LogInformation($"\n\nAttempted edit the asctivity with id: {request.Activity.Id}\n");
                }

                 return Result<Unit>.Success(Unit.Value);

            }
        }
    }
}