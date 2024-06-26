using Application.Core;
using Application.Interfaces;
using Domain;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence;

namespace Application.Activities
{
    public class UpdateAttendance
    {
        public class Command : IRequest<Result<Unit>>
        {
            public Guid Id { get; set; }
        }

        public class Handler : IRequestHandler<Command, Result<Unit>>
        {
            private readonly DataContext _ctx;
            private readonly IUserAccessor _userAccessor;

            public Handler(DataContext ctx, IUserAccessor userAccessor)
            {
                _userAccessor = userAccessor;
                _ctx = ctx;
            }

            public async Task<Result<Unit>> Handle(Command request, CancellationToken cancellationToken)
            {
                var activity = await _ctx.Activities
                    .Include(a => a.Attendees).ThenInclude(u => u.AppUser)
                    .SingleOrDefaultAsync(x => x.Id == request.Id);

                if (activity == null) return null;

                var user = await _ctx.Users.FirstOrDefaultAsync(x => 
                    x.UserName == _userAccessor.GetUsername());

                if (user == null) return null;

                var HostUsername = activity.Attendees.FirstOrDefault(x => x.IsHost)?.AppUser?.UserName;

                var attendance = activity.Attendees.FirstOrDefault(x => x.AppUser.UserName == user.UserName);

                if(attendance != null && HostUsername == user.UserName)
                    activity.IsCancelled = !activity.IsCancelled;

                if(attendance != null && HostUsername != user.UserName)
                    activity.Attendees.Remove(attendance);

                if(attendance == null)
                {
                    attendance = new ActivityAttendee
                    {
                        AppUser = user,
                        Activity = activity,
                        IsHost = false
                    };

                    activity.Attendees.Add(attendance);
                }

                var result = await _ctx.SaveChangesAsync() > 0;

                return result ? Result<Unit>.Success(Unit.Value) : Result<Unit>.Failure("Problem updating attendace");
            }
        }
    }
}