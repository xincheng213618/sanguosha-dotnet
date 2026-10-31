#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CharacterRuiJiOlEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterRuiJiOlRuntime> _resolve;
    private CharacterRuiJiOlEntries(global::System.Func<global::CardGame.Core.EngineOwners.CharacterRuiJiOlRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CharacterRuiJiOlEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CharacterRuiJiOlRuntime> resolve) => new(resolve);
}
