#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CharacterWoLongFengChuOlEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterWoLongFengChuOlRuntime> _resolve;
    private CharacterWoLongFengChuOlEntries(global::System.Func<global::CardGame.Core.EngineOwners.CharacterWoLongFengChuOlRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CharacterWoLongFengChuOlEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CharacterWoLongFengChuOlRuntime> resolve) => new(resolve);
}
