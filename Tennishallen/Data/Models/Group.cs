using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;
using Tennishallen.Data.Base;

namespace Tennishallen.Data.Models;

public class Group : IBaseEntity<int>
{
    [Key] public int Id { get; set; }
    public Guid UserId { get; set; }
    public User User { get; set; }
    public GroupName Name { get; set; }
    public enum GroupName
    {
        Admin,
        Coach,
        Member,
    }
}