using Domain;
using MediatR;
using Persistence;

namespace Application.Activities
{
    public class Create
    {
        //differenza tra Query e Command che Command non ritorna nessun valore
        //infatti non mettiamo il tipo tra parentesi angolari
        public class Command : IRequest 
        {
            public Activity Activity { get; set; }
        }

        public class Handler : IRequestHandler<Command>
        {
            private readonly DataContext _context;

            public Handler(DataContext context)
            {
                _context = context;
            }

            public async Task Handle(Command request, CancellationToken cancellationToken)
            {
                //aggiungiuamo Activity in memory quindi non usiamo il metodo Async
                //siccome non stiamo toccando il DB
                _context.Activities.Add(request.Activity);

                await _context.SaveChangesAsync();
            }
        }
    }
}