#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CharacterRuanHuiOlEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterRuanHuiOlRuntime> _resolve;
    private CharacterRuanHuiOlEntries(global::System.Func<global::CardGame.Core.EngineOwners.CharacterRuanHuiOlRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CharacterRuanHuiOlEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CharacterRuanHuiOlRuntime> resolve) => new(resolve);
}
