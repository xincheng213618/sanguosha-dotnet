#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CharacterXinPiOlEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterXinPiOlRuntime> _resolve;
    private CharacterXinPiOlEntries(global::System.Func<global::CardGame.Core.EngineOwners.CharacterXinPiOlRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CharacterXinPiOlEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CharacterXinPiOlRuntime> resolve) => new(resolve);
}
