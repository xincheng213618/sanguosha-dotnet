#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CharacterLiuXieMouEntries
{
    private readonly Func<EngineOwners.CharacterLiuXieMouRuntime> _resolve;
    private CharacterLiuXieMouEntries(Func<EngineOwners.CharacterLiuXieMouRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CharacterLiuXieMouEntries Create(Func<EngineOwners.CharacterLiuXieMouRuntime> resolve) => new(resolve);
}
