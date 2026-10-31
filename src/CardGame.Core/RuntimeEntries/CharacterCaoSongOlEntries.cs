#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CharacterCaoSongOlEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterCaoSongOlRuntime> _resolve;
    private CharacterCaoSongOlEntries(global::System.Func<global::CardGame.Core.EngineOwners.CharacterCaoSongOlRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CharacterCaoSongOlEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CharacterCaoSongOlRuntime> resolve) => new(resolve);
}
