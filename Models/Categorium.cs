using System;
using System.Collections.Generic;

namespace AplicacaoAulaBackend.Models;

public partial class Categorium
{
    public ulong IdCategoria { get; set; }

    public string Categoria { get; set; } = null!;

    public virtual ICollection<Item> Items { get; set; } = new List<Item>();
}
