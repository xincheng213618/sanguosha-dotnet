#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CharacterGuoTuOlEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterGuoTuOlRuntime> _resolve;
    private CharacterGuoTuOlEntries(global::System.Func<global::CardGame.Core.EngineOwners.CharacterGuoTuOlRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CharacterGuoTuOlEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CharacterGuoTuOlRuntime> resolve) => new(resolve);
}
