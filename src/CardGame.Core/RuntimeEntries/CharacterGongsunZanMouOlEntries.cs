#nullable enable
namespace CardGame.Core.RuntimeEntries;
[global::CardGame.Core.ProgramRuntimeEntry(typeof(global::CardGame.Core.EngineOwners.CharacterGongsunZanMouOlRuntime))]
internal sealed partial class CharacterGongsunZanMouOlEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterGongsunZanMouOlRuntime> _resolve;
    private CharacterGongsunZanMouOlEntries(global::System.Func<global::CardGame.Core.EngineOwners.CharacterGongsunZanMouOlRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CharacterGongsunZanMouOlEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CharacterGongsunZanMouOlRuntime> resolve) => new(resolve);
}
