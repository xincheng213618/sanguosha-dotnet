#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CharacterSunYuanOlEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterSunYuanOlRuntime> _resolve;
    private CharacterSunYuanOlEntries(global::System.Func<global::CardGame.Core.EngineOwners.CharacterSunYuanOlRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CharacterSunYuanOlEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CharacterSunYuanOlRuntime> resolve) => new(resolve);
}
