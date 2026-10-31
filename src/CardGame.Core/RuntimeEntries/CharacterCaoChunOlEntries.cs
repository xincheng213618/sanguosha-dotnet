#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CharacterCaoChunOlEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterCaoChunOlRuntime> _resolve;
    private CharacterCaoChunOlEntries(global::System.Func<global::CardGame.Core.EngineOwners.CharacterCaoChunOlRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CharacterCaoChunOlEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CharacterCaoChunOlRuntime> resolve) => new(resolve);
}
