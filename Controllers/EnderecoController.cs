using AplicacaoAulaBackend.DTO;
using AplicacaoAulaBackend.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AplicacaoAulaBackend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EnderecoController : ControllerBase
    {
        private readonly DbAulaBackendContext _context;

        public EnderecoController(DbAulaBackendContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<EnderecoResponseDTO>>> GetEnderecos()
        {
            return await _context.Enderecos
                .Select(e => new EnderecoResponseDTO
                {
                    IdEndereco = e.IdEndereco,
                    IdPessoa = e.IdPessoa,
                    Cep = e.Cep,
                    Numero = e.Numero,
                    Complemento = e.Complemento
                })
                .ToListAsync();
        }

        [HttpGet("Pessoa/{id}")]
        public async Task<ActionResult<IEnumerable<EnderecoResponseDTO>>> GetEnderecosByIdPessoa(ulong id)
        {
            return await _context.Enderecos
                .Where(e => e.IdPessoa == id)
                .Select(e => new EnderecoResponseDTO
                {
                    IdEndereco = e.IdEndereco,
                    IdPessoa = e.IdPessoa,
                    Cep = e.Cep,
                    Numero = e.Numero,
                    Complemento = e.Complemento
                })
                .ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<EnderecoResponseDTO>> GetEnderecoById(ulong id)
        {
            var endereco = await _context.Enderecos
                .Where(e => e.IdEndereco == id)
                .Select(e => new EnderecoResponseDTO
                {
                    IdEndereco = e.IdEndereco,
                    IdPessoa = e.IdPessoa,
                    Cep = e.Cep,
                    Numero = e.Numero,
                    Complemento = e.Complemento
                })
                .FirstOrDefaultAsync();

            if (endereco == null)
            {
                return NotFound();
            }

            return Ok(endereco);
        }

        [HttpPost]
        public async Task<ActionResult<EnderecoResponseDTO>> CreateEndereco(EnderecoRequestDTO dto)
        {
            if (!await _context.Pessoas.AnyAsync(p => p.IdPessoa == dto.IdPessoa))
            {
                return NotFound("Pessoa não encontrada.");
            }

            var endereco = new Endereco
            {
                IdPessoa = dto.IdPessoa,
                Cep = dto.Cep,
                Numero = dto.Numero,
                Complemento = dto.Complemento
            };

            _context.Enderecos.Add(endereco);
            await _context.SaveChangesAsync();

            var response = new EnderecoResponseDTO
            {
                IdEndereco = endereco.IdEndereco,
                IdPessoa = endereco.IdPessoa,
                Cep = endereco.Cep,
                Numero = endereco.Numero,
                Complemento = endereco.Complemento
            };

            return CreatedAtAction(nameof(GetEnderecoById), new { id = endereco.IdEndereco }, response);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateEndereco(ulong id, EnderecoRequestDTO dto)
        {
            var endereco = await _context.Enderecos
                .FirstOrDefaultAsync(e => e.IdEndereco == id);

            if (endereco == null)
            {
                return NotFound();
            }

            if (endereco.IdPessoa != dto.IdPessoa)
            {
                return Conflict("Não é possível alterar a pessoa do endereço.");
            }

            endereco.Cep = dto.Cep;
            endereco.Numero = dto.Numero;
            endereco.Complemento = dto.Complemento;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!EnderecoExiste(id))
                {
                    return NotFound();
                }

                throw;
            }

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteEndereco(ulong id)
        {
            var endereco = await _context.Enderecos
                .FirstOrDefaultAsync(e => e.IdEndereco == id);

            if (endereco == null)
            {
                return NotFound();
            }

            _context.Enderecos.Remove(endereco);

            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool EnderecoExiste(ulong id)
        {
            return _context.Enderecos
                .Any(e => e.IdEndereco == id);
        }
    }
}
