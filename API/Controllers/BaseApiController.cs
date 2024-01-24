using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{

    [ApiController]
    [Route("api/[controller]")]
    public class BaseApiController : ControllerBase
    {
        private IMediator _mediator;

        //L'operatore ??= è noto come operatore di assegnazione con null-conditional e permette di assegnare il valore
        // a sinistra dell'operatore, alla variabile solo se la variabile è attualmente null.
        protected IMediator Mediator => _mediator ??= HttpContext.RequestServices.GetService<IMediator>();
        
    }
}