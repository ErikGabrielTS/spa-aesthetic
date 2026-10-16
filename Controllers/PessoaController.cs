using AplicacaoAulaBackend.DTO;
using AplicacaoAulaBackend.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections;

namespace AplicacaoAulaBackend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PessoaController : ControllerBase
    {
        private readonly DbAulaBackendContext _context;

        public PessoaController(DbAulaBackendContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<ActionResult<IEnumerable>>> GetAll()
        {
            var pessoas = await _context.Pessoas.Select(p => new PessoaResponseDTO
            {
                IdPessoa = p.IdPessoa,
                Nome = p.Nome,
                Cpf = p.Cpf,
                DataNascimento = p.DataNascimento,
                Sexo = p.Sexo,
                Email = p.Email,
                Telefone = p.Telefone,
            }).ToListAsync();

            return Ok(pessoas);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<PessoaResponseDTO>> GetPessoaById(ulong id)
        {
            var pessoa = await _context.Pessoas
                .Where(p => p.IdPessoa == id)
                .Select(p => new PessoaResponseDTO
                {
                    IdPessoa = p.IdPessoa,
                    Nome = p.Nome,
                    Cpf = p.Cpf,
                    DataNascimento = p.DataNascimento,
                    Sexo = p.Sexo,
                    Email = p.Email,
                    Telefone = p.Telefone
                }).FirstOrDefaultAsync();

            if (pessoa == null)
            {
                return NotFound();
            }

            return Ok(pessoa);
        }

        [HttpPost]
        public async Task<ActionResult<PessoaResponseDTO>> CreatePessoa(PessoaRequestDTO dto)
        {
            if (await _context.Pessoas.AnyAsync(p => p.Cpf == dto.Cpf))
            {
                return Conflict("CPF já cadastrado.");
            }

            if (await _context.Pessoas.AnyAsync(p => p.Email == dto.Email))
            {
                return Conflict("Email já cadastrado.");
            }

            var pessoa = new Pessoa
            {
                Nome = dto.Nome,
                Cpf = dto.Cpf,
                DataNascimento = dto.DataNascimento,
                Sexo = dto.Sexo,
                Email = dto.Email,
                Telefone = dto.Telefone,
                AtivoPessoa = "S"
            };

            _context.Pessoas.Add(pessoa);
            await _context.SaveChangesAsync();

            var response = new PessoaResponseDTO
            {
                IdPessoa = pessoa.IdPessoa,
                Nome = pessoa.Nome,
                Cpf = pessoa.Cpf,
                DataNascimento = pessoa.DataNascimento,
                Sexo = pessoa.Sexo,
                Email = pessoa.Email,
                Telefone = pessoa.Telefone
            };

            return CreatedAtAction(nameof(GetPessoaById), new { id = pessoa.IdPessoa }, response);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdatePessoa(
        ulong id,
        PessoaRequestDTO dto)
        {
            var pessoa = await _context.Pessoas
                .FirstOrDefaultAsync(p => p.IdPessoa == id);

            if (pessoa == null)
            {
                return NotFound();
            }

            if (pessoa.Cpf != dto.Cpf)
            {
                return Conflict("Não é possível alterar o CPF após o cadastro.");
            }

            if (await _context.Pessoas.AnyAsync(p => p.Email == dto.Email && p.IdPessoa != id))
            {
                return Conflict("Email já cadastrado.");
            }

            pessoa.Nome = dto.Nome;
            pessoa.DataNascimento = dto.DataNascimento;
            pessoa.Sexo = dto.Sexo;
            pessoa.Email = dto.Email;
            pessoa.Telefone = dto.Telefone;
            pessoa.AtivoPessoa = "S";
            
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PessoaExiste(id))
                {
                    return NotFound();
                }

                throw;
            }

            return NoContent();
        }

        private bool PessoaExiste(ulong id)
        {
            return _context.Pessoas
                .Any(p => p.IdPessoa == id);
        }
    }
}
