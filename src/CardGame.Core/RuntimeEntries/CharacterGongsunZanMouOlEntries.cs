#nullable enable
namespace CardGame.Core.RuntimeEntries;
[Core.ProgramRuntimeEntry(typeof(EngineOwners.CharacterGongsunZanMouOlRuntime))]
internal sealed partial class CharacterGongsunZanMouOlEntries
{
    private readonly Func<EngineOwners.CharacterGongsunZanMouOlRuntime> _resolve;
    private CharacterGongsunZanMouOlEntries(Func<EngineOwners.CharacterGongsunZanMouOlRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CharacterGongsunZanMouOlEntries Create(Func<EngineOwners.CharacterGongsunZanMouOlRuntime> resolve) => new(resolve);
}
