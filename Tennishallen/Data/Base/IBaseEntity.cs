namespace Tennishallen.Data.Base;

public interface IBaseEntity<TIdentifier> where TIdentifier : IEquatable<TIdentifier>
{
    /// <summary>
    ///     The id of the model
    /// </summary>
    public TIdentifier Id { get; set; }
}