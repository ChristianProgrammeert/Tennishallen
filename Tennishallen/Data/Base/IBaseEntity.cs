namespace Tennishallen.Data.Base;

public interface IBaseEntity<TIdentifier> where TIdentifier : IEquatable<TIdentifier>
{
    public TIdentifier Id { get; set; }
}