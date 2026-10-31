#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CharacterTengFangLanOlEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterTengFangLanOlRuntime> _resolve;
    private CharacterTengFangLanOlEntries(global::System.Func<global::CardGame.Core.EngineOwners.CharacterTengFangLanOlRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CharacterTengFangLanOlEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CharacterTengFangLanOlRuntime> resolve) => new(resolve);
}
