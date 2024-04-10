using System.ComponentModel.DataAnnotations;
using Tennishallen.Data.Base;

namespace Tennishallen.Data.Models;

public class Group : IBaseEntity<int>
{
    public enum GroupName
    {
        Admin,
        Coach,
        Member
    }

    public Guid UserId { get; set; }
    public User User { get; set; }
    public GroupName Name { get; set; }
    [Key] public int Id { get; set; }
}