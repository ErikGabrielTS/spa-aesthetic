using System;
using System.Collections.Generic;

namespace AplicacaoAulaBackend.Models;

public partial class Categoria
{
    public ulong IdCategoria { get; set; }

    public string Descricao { get; set; } = null!;

    public virtual ICollection<Item> Items { get; set; } = new List<Item>();
}
