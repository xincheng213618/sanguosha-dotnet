#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CharacterGuoSiOlEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterGuoSiOlRuntime> _resolve;
    private CharacterGuoSiOlEntries(global::System.Func<global::CardGame.Core.EngineOwners.CharacterGuoSiOlRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CharacterGuoSiOlEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CharacterGuoSiOlRuntime> resolve) => new(resolve);
}
