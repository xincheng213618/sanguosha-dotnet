#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CharacterRuanYuOlEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterRuanYuOlRuntime> _resolve;
    private CharacterRuanYuOlEntries(global::System.Func<global::CardGame.Core.EngineOwners.CharacterRuanYuOlRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CharacterRuanYuOlEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CharacterRuanYuOlRuntime> resolve) => new(resolve);
}
