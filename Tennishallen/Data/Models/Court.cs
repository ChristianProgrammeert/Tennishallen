using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using Tennishallen.Data.Base;
using Tennishallen.Data.Models;

namespace Tennishallen.Data.Models;

public class Court : IBaseEntity<int>
{
    [Key] public int Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public double Price { get; set; }
}
