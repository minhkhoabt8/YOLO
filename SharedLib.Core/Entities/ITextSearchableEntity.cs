namespace SharedLib.Core.Entities;

public interface ITextSearchableEntity
{
    // Texts to search along with their relative weights
    public IReadOnlyDictionary<Func<string>, double> SearchTextsWithWeights { get; }
}