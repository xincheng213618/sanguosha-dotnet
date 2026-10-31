#nullable enable
namespace CardGame.Core.RuntimeEntries;
[Core.ProgramRuntimeEntry(typeof(EngineOwners.CharacterCaoSongOlRuntime))]
internal sealed partial class CharacterCaoSongOlEntries
{
    private readonly Func<EngineOwners.CharacterCaoSongOlRuntime> _resolve;
    private CharacterCaoSongOlEntries(Func<EngineOwners.CharacterCaoSongOlRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CharacterCaoSongOlEntries Create(Func<EngineOwners.CharacterCaoSongOlRuntime> resolve) => new(resolve);
}
