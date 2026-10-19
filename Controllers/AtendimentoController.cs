using AplicacaoAulaBackend.Models;
using Microsoft.AspNetCore.Mvc;

namespace AplicacaoAulaBackend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AtendimentoController : ControllerBase
    {

        private readonly DbAulaBackendContext _context;

        public AtendimentoController(DbAulaBackendContext context)
        {
            _context = context;
        }


    }
}
