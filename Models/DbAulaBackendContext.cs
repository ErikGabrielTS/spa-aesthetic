using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Pomelo.EntityFrameworkCore.MySql.Scaffolding.Internal;

namespace AplicacaoAulaBackend.Models;

public partial class DbAulaBackendContext : DbContext
{
    public DbAulaBackendContext()
    {
    }

    public DbAulaBackendContext(DbContextOptions<DbAulaBackendContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Atendimento> Atendimentos { get; set; }

    public virtual DbSet<AtendimentoItem> AtendimentoItems { get; set; }

    public virtual DbSet<Categoria> Categorias { get; set; }

    public virtual DbSet<Endereco> Enderecos { get; set; }

    public virtual DbSet<EntradaProduto> EntradaProdutos { get; set; }

    public virtual DbSet<Estoque> Estoques { get; set; }

    public virtual DbSet<Funcionario> Funcionarios { get; set; }

    public virtual DbSet<Item> Items { get; set; }

    public virtual DbSet<Pessoa> Pessoas { get; set; }

    public virtual DbSet<Produto> Produtos { get; set; }

    public virtual DbSet<VwAtendimentosDetalhe> VwAtendimentosDetalhes { get; set; }

    public virtual DbSet<VwClientesEndereco> VwClientesEnderecos { get; set; }

    public virtual DbSet<VwFuncionariosEndereco> VwFuncionariosEnderecos { get; set; }

    public virtual DbSet<VwProdutosCategoriasEstoque> VwProdutosCategoriasEstoques { get; set; }

    public virtual DbSet<VwServicosCategoria> VwServicosCategorias { get; set; }

//    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
//#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
//        => optionsBuilder.UseMySql("server=localhost;port=3306;database=db_aula_backend;user=root;password=root", Microsoft.EntityFrameworkCore.ServerVersion.Parse("26.7.0-mysql"));

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder
            .UseCollation("utf8mb4_0900_ai_ci")
            .HasCharSet("utf8mb4");

        modelBuilder.Entity<Atendimento>(entity =>
        {
            entity.HasKey(e => e.IdAtendimento).HasName("PRIMARY");

            entity.ToTable("atendimento");

            entity.HasIndex(e => e.IdPessoa, "atendimento_pessoa_FK");

            entity.Property(e => e.IdAtendimento).HasColumnName("id_atendimento");
            entity.Property(e => e.DataAtendimento)
                .HasColumnType("datetime")
                .HasColumnName("data_atendimento");
            entity.Property(e => e.IdPessoa).HasColumnName("id_pessoa");
            entity.Property(e => e.StatusAtendimento)
                .HasMaxLength(1)
                .IsFixedLength()
                .HasColumnName("status_atendimento");

            entity.HasOne(d => d.IdPessoaNavigation).WithMany(p => p.Atendimentos)
                .HasForeignKey(d => d.IdPessoa)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("atendimento_pessoa_FK");
        });

        modelBuilder.Entity<AtendimentoItem>(entity =>
        {
            entity.HasKey(e => new { e.IdAtendimento, e.IdItem })
                .HasName("PRIMARY")
                .HasAnnotation("MySql:IndexPrefixLength", new[] { 0, 0 });

            entity.ToTable("atendimento_item");

            entity.HasIndex(e => e.IdItem, "atendimento_item_item_FK");

            entity.HasIndex(e => e.IdFuncionario, "atendimento_item_pessoa_FK");

            entity.Property(e => e.IdAtendimento).HasColumnName("id_atendimento");
            entity.Property(e => e.IdItem).HasColumnName("id_item");
            entity.Property(e => e.IdFuncionario).HasColumnName("id_funcionario");
            entity.Property(e => e.QuantidadeItens).HasColumnName("quantidade_itens");
            entity.Property(e => e.ValorUnitarioEntrada)
                .HasPrecision(10, 2)
                .HasColumnName("valor_unitario_entrada");

            entity.HasOne(d => d.IdAtendimentoNavigation).WithMany(p => p.AtendimentoItems)
                .HasForeignKey(d => d.IdAtendimento)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("atendimento_item_atendimento_FK");

            entity.HasOne(d => d.IdFuncionarioNavigation).WithMany(p => p.AtendimentoItems)
                .HasForeignKey(d => d.IdFuncionario)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("atendimento_item_pessoa_FK");

            entity.HasOne(d => d.IdItemNavigation).WithMany(p => p.AtendimentoItems)
                .HasForeignKey(d => d.IdItem)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("atendimento_item_item_FK");
        });

        modelBuilder.Entity<Categoria>(entity =>
        {
            entity.HasKey(e => e.IdCategoria).HasName("PRIMARY");

            entity.ToTable("categoria");

            entity.HasIndex(e => e.Descricao, "categoria_unique").IsUnique();

            entity.Property(e => e.IdCategoria).HasColumnName("id_categoria");
            entity.Property(e => e.Descricao)
                .HasMaxLength(35)
                .HasColumnName("descricao");
        });

        modelBuilder.Entity<Endereco>(entity =>
        {
            entity.HasKey(e => e.IdEndereco).HasName("PRIMARY");

            entity.ToTable("endereco");

            entity.HasIndex(e => e.IdPessoa, "endereco_pessoa_FK");

            entity.Property(e => e.IdEndereco).HasColumnName("id_endereco");
            entity.Property(e => e.Cep)
                .HasMaxLength(8)
                .IsFixedLength()
                .HasColumnName("cep");
            entity.Property(e => e.Complemento)
                .HasMaxLength(150)
                .HasColumnName("complemento");
            entity.Property(e => e.IdPessoa).HasColumnName("id_pessoa");
            entity.Property(e => e.Numero)
                .HasMaxLength(10)
                .HasColumnName("numero");

            entity.HasOne(d => d.IdPessoaNavigation).WithMany(p => p.Enderecos)
                .HasForeignKey(d => d.IdPessoa)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("endereco_pessoa_FK");
        });

        modelBuilder.Entity<EntradaProduto>(entity =>
        {
            entity.HasKey(e => new { e.IdEntradaProduto, e.IdItem })
                .HasName("PRIMARY")
                .HasAnnotation("MySql:IndexPrefixLength", new[] { 0, 0 });

            entity.ToTable("entrada_produto");

            entity.HasIndex(e => e.IdItem, "entrada_produto_estoque_FK");

            entity.Property(e => e.IdEntradaProduto)
                .ValueGeneratedOnAdd()
                .HasColumnName("id_entrada_produto");
            entity.Property(e => e.IdItem).HasColumnName("id_item");
            entity.Property(e => e.DataEntradaProduto)
                .HasColumnType("datetime")
                .HasColumnName("data_entrada_produto");
            entity.Property(e => e.QuantidadeEntrada)
                .HasPrecision(10, 2)
                .HasColumnName("quantidade_entrada");
            entity.Property(e => e.Unidade)
                .HasMaxLength(10)
                .HasColumnName("unidade");
            entity.Property(e => e.ValorUnitarioEntrada)
                .HasPrecision(10, 2)
                .HasColumnName("valor_unitario_entrada");

            entity.HasOne(d => d.IdItemNavigation).WithMany(p => p.EntradaProdutos)
                .HasForeignKey(d => d.IdItem)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("entrada_produto_estoque_FK");
        });

        modelBuilder.Entity<Estoque>(entity =>
        {
            entity.HasKey(e => e.IdItem).HasName("PRIMARY");

            entity.ToTable("estoque");

            entity.Property(e => e.IdItem)
                .ValueGeneratedNever()
                .HasColumnName("id_item");
            entity.Property(e => e.QuantidadeEstoque).HasColumnName("quantidade_estoque");

            entity.HasOne(d => d.IdItemNavigation).WithOne(p => p.Estoque)
                .HasForeignKey<Estoque>(d => d.IdItem)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("estoque_produto_FK");
        });

        modelBuilder.Entity<Funcionario>(entity =>
        {
            entity.HasKey(e => e.IdPessoa).HasName("PRIMARY");

            entity.ToTable("funcionario");

            entity.Property(e => e.IdPessoa)
                .ValueGeneratedNever()
                .HasColumnName("id_pessoa");
            entity.Property(e => e.Cargo)
                .HasMaxLength(20)
                .HasColumnName("cargo");
            entity.Property(e => e.DataAdmissao).HasColumnName("data_admissao");

            entity.HasOne(d => d.IdPessoaNavigation).WithOne(p => p.Funcionario)
                .HasForeignKey<Funcionario>(d => d.IdPessoa)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("funcionario_pessoa_FK");
        });

        modelBuilder.Entity<Item>(entity =>
        {
            entity.HasKey(e => e.IdItem).HasName("PRIMARY");

            entity.ToTable("item");

            entity.HasIndex(e => e.IdCategoria, "item_categoria_FK");

            entity.HasIndex(e => e.Item1, "item_unique").IsUnique();

            entity.Property(e => e.IdItem).HasColumnName("id_item");
            entity.Property(e => e.AtivoProdutoServico)
                .HasMaxLength(1)
                .IsFixedLength()
                .HasColumnName("ativo_produto_servico");
            entity.Property(e => e.DescricaoItem)
                .HasMaxLength(200)
                .HasColumnName("descricao_item");
            entity.Property(e => e.IdCategoria).HasColumnName("id_categoria");
            entity.Property(e => e.Item1)
                .HasMaxLength(40)
                .HasColumnName("item");
            entity.Property(e => e.Preco)
                .HasPrecision(10, 2)
                .HasColumnName("preco");
            entity.Property(e => e.TipoItem)
                .HasMaxLength(1)
                .IsFixedLength()
                .HasColumnName("tipo_item");

            entity.HasOne(d => d.IdCategoriaNavigation).WithMany(p => p.Items)
                .HasForeignKey(d => d.IdCategoria)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("item_categoria_FK");
        });

        modelBuilder.Entity<Pessoa>(entity =>
        {
            entity.HasKey(e => e.IdPessoa).HasName("PRIMARY");

            entity.ToTable("pessoa");

            entity.HasIndex(e => e.Cpf, "pessoa_unique_cpf").IsUnique();

            entity.HasIndex(e => e.Email, "pessoa_unique_email").IsUnique();

            entity.Property(e => e.IdPessoa).HasColumnName("id_pessoa");
            entity.Property(e => e.AtivoPessoa)
                .HasMaxLength(1)
                .IsFixedLength()
                .HasColumnName("ativo_pessoa");
            entity.Property(e => e.Cpf)
                .HasMaxLength(11)
                .IsFixedLength()
                .HasColumnName("cpf");
            entity.Property(e => e.DataNascimento).HasColumnName("data_nascimento");
            entity.Property(e => e.Email)
                .HasMaxLength(100)
                .HasColumnName("email");
            entity.Property(e => e.Nome)
                .HasMaxLength(100)
                .HasColumnName("nome");
            entity.Property(e => e.Sexo)
                .HasMaxLength(1)
                .IsFixedLength()
                .HasColumnName("sexo");
            entity.Property(e => e.Telefone)
                .HasMaxLength(11)
                .IsFixedLength()
                .HasColumnName("telefone");
        });

        modelBuilder.Entity<Produto>(entity =>
        {
            entity.HasKey(e => e.IdItem).HasName("PRIMARY");

            entity.ToTable("produto");

            entity.Property(e => e.IdItem)
                .ValueGeneratedNever()
                .HasColumnName("id_item");
            entity.Property(e => e.CodigoPreco).HasColumnName("codigo_preco");
            entity.Property(e => e.PrecoVenda)
                .HasPrecision(10, 2)
                .HasColumnName("preco_venda");
            entity.Property(e => e.Unidade)
                .HasMaxLength(10)
                .HasColumnName("unidade");

            entity.HasOne(d => d.IdItemNavigation).WithOne(p => p.Produto)
                .HasForeignKey<Produto>(d => d.IdItem)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("produto_item_FK");
        });

        modelBuilder.Entity<VwAtendimentosDetalhe>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("vw_atendimentos_detalhes");

            entity.Property(e => e.Cliente)
                .HasMaxLength(100)
                .HasColumnName("cliente");
            entity.Property(e => e.CpfCliente)
                .HasMaxLength(11)
                .IsFixedLength()
                .HasColumnName("cpf_cliente");
            entity.Property(e => e.DataAtendimento)
                .HasColumnType("datetime")
                .HasColumnName("data_atendimento");
            entity.Property(e => e.Funcionario)
                .HasMaxLength(100)
                .HasColumnName("funcionario");
            entity.Property(e => e.IdAtendimento).HasColumnName("id_atendimento");
            entity.Property(e => e.IdCliente).HasColumnName("id_cliente");
            entity.Property(e => e.IdFuncionario).HasColumnName("id_funcionario");
            entity.Property(e => e.IdItem).HasColumnName("id_item");
            entity.Property(e => e.Produto)
                .HasMaxLength(40)
                .HasColumnName("produto");
            entity.Property(e => e.QuantidadeItens).HasColumnName("quantidade_itens");
            entity.Property(e => e.StatusAtendimento)
                .HasMaxLength(1)
                .IsFixedLength()
                .HasColumnName("status_atendimento");
            entity.Property(e => e.ValorTotalItem)
                .HasPrecision(20, 2)
                .HasColumnName("valor_total_item");
            entity.Property(e => e.ValorUnitarioEntrada)
                .HasPrecision(10, 2)
                .HasColumnName("valor_unitario_entrada");
        });

        modelBuilder.Entity<VwClientesEndereco>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("vw_clientes_enderecos");

            entity.Property(e => e.AtivoPessoa)
                .HasMaxLength(1)
                .IsFixedLength()
                .HasColumnName("ativo_pessoa");
            entity.Property(e => e.Cep)
                .HasMaxLength(8)
                .IsFixedLength()
                .HasColumnName("cep");
            entity.Property(e => e.Complemento)
                .HasMaxLength(150)
                .HasColumnName("complemento");
            entity.Property(e => e.Cpf)
                .HasMaxLength(11)
                .IsFixedLength()
                .HasColumnName("cpf");
            entity.Property(e => e.Email)
                .HasMaxLength(100)
                .HasColumnName("email");
            entity.Property(e => e.IdEndereco)
                .HasDefaultValueSql("'0'")
                .HasColumnName("id_endereco");
            entity.Property(e => e.IdPessoa).HasColumnName("id_pessoa");
            entity.Property(e => e.Nome)
                .HasMaxLength(100)
                .HasColumnName("nome");
            entity.Property(e => e.Numero)
                .HasMaxLength(10)
                .HasColumnName("numero");
            entity.Property(e => e.Telefone)
                .HasMaxLength(11)
                .IsFixedLength()
                .HasColumnName("telefone");
        });

        modelBuilder.Entity<VwFuncionariosEndereco>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("vw_funcionarios_enderecos");

            entity.Property(e => e.AtivoPessoa)
                .HasMaxLength(1)
                .IsFixedLength()
                .HasColumnName("ativo_pessoa");
            entity.Property(e => e.Cargo)
                .HasMaxLength(20)
                .HasColumnName("cargo");
            entity.Property(e => e.Cep)
                .HasMaxLength(8)
                .IsFixedLength()
                .HasColumnName("cep");
            entity.Property(e => e.Complemento)
                .HasMaxLength(150)
                .HasColumnName("complemento");
            entity.Property(e => e.Cpf)
                .HasMaxLength(11)
                .IsFixedLength()
                .HasColumnName("cpf");
            entity.Property(e => e.DataAdmissao).HasColumnName("data_admissao");
            entity.Property(e => e.Email)
                .HasMaxLength(100)
                .HasColumnName("email");
            entity.Property(e => e.IdEndereco)
                .HasDefaultValueSql("'0'")
                .HasColumnName("id_endereco");
            entity.Property(e => e.IdPessoa).HasColumnName("id_pessoa");
            entity.Property(e => e.Nome)
                .HasMaxLength(100)
                .HasColumnName("nome");
            entity.Property(e => e.Numero)
                .HasMaxLength(10)
                .HasColumnName("numero");
            entity.Property(e => e.Telefone)
                .HasMaxLength(11)
                .IsFixedLength()
                .HasColumnName("telefone");
        });

        modelBuilder.Entity<VwProdutosCategoriasEstoque>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("vw_produtos_categorias_estoque");

            entity.Property(e => e.AtivoProdutoServico)
                .HasMaxLength(1)
                .IsFixedLength()
                .HasColumnName("ativo_produto_servico");
            entity.Property(e => e.Categoria)
                .HasMaxLength(35)
                .HasColumnName("categoria");
            entity.Property(e => e.CodigoPreco).HasColumnName("codigo_preco");
            entity.Property(e => e.DescricaoItem)
                .HasMaxLength(200)
                .HasColumnName("descricao_item");
            entity.Property(e => e.IdCategoria).HasColumnName("id_categoria");
            entity.Property(e => e.IdItem).HasColumnName("id_item");
            entity.Property(e => e.Item)
                .HasMaxLength(40)
                .HasColumnName("item");
            entity.Property(e => e.PrecoVenda)
                .HasPrecision(10, 2)
                .HasColumnName("preco_venda");
            entity.Property(e => e.QuantidadeEstoque).HasColumnName("quantidade_estoque");
            entity.Property(e => e.Unidade)
                .HasMaxLength(10)
                .HasColumnName("unidade");
        });

        modelBuilder.Entity<VwServicosCategoria>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("vw_servicos_categorias");

            entity.Property(e => e.AtivoProdutoServico)
                .HasMaxLength(1)
                .IsFixedLength()
                .HasColumnName("ativo_produto_servico");
            entity.Property(e => e.Categoria)
                .HasMaxLength(35)
                .HasColumnName("categoria");
            entity.Property(e => e.DescricaoItem)
                .HasMaxLength(200)
                .HasColumnName("descricao_item");
            entity.Property(e => e.IdCategoria).HasColumnName("id_categoria");
            entity.Property(e => e.IdItem).HasColumnName("id_item");
            entity.Property(e => e.Item)
                .HasMaxLength(40)
                .HasColumnName("item");
            entity.Property(e => e.Preco)
                .HasPrecision(10, 2)
                .HasColumnName("preco");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
