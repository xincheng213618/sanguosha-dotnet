#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CharacterLiuXieMouEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterLiuXieMouRuntime> _resolve;
    private CharacterLiuXieMouEntries(global::System.Func<global::CardGame.Core.EngineOwners.CharacterLiuXieMouRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CharacterLiuXieMouEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CharacterLiuXieMouRuntime> resolve) => new(resolve);
}
