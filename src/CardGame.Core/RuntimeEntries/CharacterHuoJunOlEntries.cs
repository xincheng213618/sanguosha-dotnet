#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CharacterHuoJunOlEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterHuoJunOlRuntime> _resolve;
    private CharacterHuoJunOlEntries(global::System.Func<global::CardGame.Core.EngineOwners.CharacterHuoJunOlRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CharacterHuoJunOlEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CharacterHuoJunOlRuntime> resolve) => new(resolve);
}
