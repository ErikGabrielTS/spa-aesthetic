using AplicacaoAulaBackend.DTO;
using AplicacaoAulaBackend.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AplicacaoAulaBackend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FuncionarioController : ControllerBase
    {
        private readonly DbAulaBackendContext _context;

        public FuncionarioController(DbAulaBackendContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<FuncionarioResponseDTO>>> GetAll()
        {
            var funcionarios = await _context.Funcionarios.Select(f => new FuncionarioResponseDTO
            {
                    IdFuncionario = f.IdPessoaNavigation.IdPessoa,
                    Nome = f.IdPessoaNavigation.Nome,
                    Cpf = f.IdPessoaNavigation.Cpf,
                    DataNascimento = f.IdPessoaNavigation.DataNascimento,
                    Sexo = f.IdPessoaNavigation.Sexo,
                    Email = f.IdPessoaNavigation.Email,
                    Telefone = f.IdPessoaNavigation.Telefone,
                    Cargo = f.Cargo,
                    DataAdmissao = f.DataAdmissao
            }).ToListAsync();

            return Ok(funcionarios);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<FuncionarioResponseDTO>> GetFuncionarioById(ulong id)
        {
            var funcionario = await _context.Funcionarios
                .Where(f => f.IdPessoa == id)
                .Select(f => new FuncionarioResponseDTO
                {
                    IdFuncionario = f.IdPessoaNavigation.IdPessoa,
                    Nome = f.IdPessoaNavigation.Nome,
                    Cpf = f.IdPessoaNavigation.Cpf,
                    DataNascimento = f.IdPessoaNavigation.DataNascimento,
                    Sexo = f.IdPessoaNavigation.Sexo,
                    Email = f.IdPessoaNavigation.Email,
                    Telefone = f.IdPessoaNavigation.Telefone,
                    Cargo = f.Cargo,
                    DataAdmissao = f.DataAdmissao
                }).FirstOrDefaultAsync();

            if (funcionario == null)
            {
                return NotFound();
            }

            return Ok(funcionario);
        }

        [HttpPost]
        public async Task<ActionResult<FuncionarioResponseDTO>> CreateFuncionario(FuncionarioRequestDTO dto)
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
                AtivoPessoa = "S",
                Funcionario = new Funcionario
                {
                    Cargo = dto.Cargo,
                    DataAdmissao = dto.DataAdmissao,
                }
            };

            _context.Pessoas.Add(pessoa);
            await _context.SaveChangesAsync();

            var response = new FuncionarioResponseDTO
            {
                IdFuncionario = pessoa.IdPessoa,
                Nome = pessoa.Nome,
                Cpf = pessoa.Cpf,
                DataNascimento = pessoa.DataNascimento,
                Sexo = pessoa.Sexo,
                Email = pessoa.Email,
                Telefone = pessoa.Telefone,
                Cargo = pessoa.Funcionario.Cargo,
                DataAdmissao = pessoa.Funcionario.DataAdmissao
            };

            return CreatedAtAction(nameof(GetFuncionarioById), new { id = pessoa.IdPessoa }, response);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateFuncionario(
        ulong id,
        FuncionarioRequestDTO dto)
        {
            var funcionario = await _context.Funcionarios
                .Include(f => f.IdPessoaNavigation)
                .FirstOrDefaultAsync(f => f.IdPessoa == id);

            if (funcionario == null)
            {
                return NotFound();
            }

            if (funcionario.IdPessoaNavigation.Cpf != dto.Cpf)
            {
                return Conflict("Não é possível alterar o CPF após o cadastro.");
            }

            if (await _context.Pessoas.AnyAsync(p => p.Email == dto.Email && p.IdPessoa != id))
            {
                return Conflict("Email já cadastrado.");
            }

            funcionario.IdPessoaNavigation.Nome = dto.Nome;
            funcionario.IdPessoaNavigation.DataNascimento = dto.DataNascimento;
            funcionario.IdPessoaNavigation.Sexo = dto.Sexo;
            funcionario.IdPessoaNavigation.Email = dto.Email;
            funcionario.IdPessoaNavigation.Telefone = dto.Telefone;
            funcionario.IdPessoaNavigation.AtivoPessoa = "S";
            funcionario.Cargo = dto.Cargo;
            funcionario.DataAdmissao = dto.DataAdmissao;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!FuncionarioExiste(id))
                {
                    return NotFound();
                }

                throw;
            }

            return NoContent();
        }

        // A implementação atual apenas exclui o registro em "Funcionario", o registro em "Pessoa" ainda permanece
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteFuncionario(ulong id)
        {
            var funcionario = await _context.Funcionarios
                .FirstOrDefaultAsync(f => f.IdPessoa == id);

            if (funcionario == null)
            {
                return NotFound();
            }

            _context.Funcionarios.Remove(funcionario);

            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool FuncionarioExiste(ulong id)
        {
            return _context.Funcionarios
                .Any(f => f.IdPessoa == id);
        }
    }
}
