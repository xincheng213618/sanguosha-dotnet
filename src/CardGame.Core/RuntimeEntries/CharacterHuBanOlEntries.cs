#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CharacterHuBanOlEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterHuBanOlRuntime> _resolve;
    private CharacterHuBanOlEntries(global::System.Func<global::CardGame.Core.EngineOwners.CharacterHuBanOlRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CharacterHuBanOlEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CharacterHuBanOlRuntime> resolve) => new(resolve);
}
