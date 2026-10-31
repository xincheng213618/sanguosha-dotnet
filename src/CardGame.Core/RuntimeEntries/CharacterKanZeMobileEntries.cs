#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CharacterKanZeMobileEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterKanZeMobileRuntime> _resolve;
    private CharacterKanZeMobileEntries(global::System.Func<global::CardGame.Core.EngineOwners.CharacterKanZeMobileRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CharacterKanZeMobileEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CharacterKanZeMobileRuntime> resolve) => new(resolve);
}
