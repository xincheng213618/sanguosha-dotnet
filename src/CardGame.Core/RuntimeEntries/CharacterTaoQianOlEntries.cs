#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CharacterTaoQianOlEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterTaoQianOlRuntime> _resolve;
    private CharacterTaoQianOlEntries(global::System.Func<global::CardGame.Core.EngineOwners.CharacterTaoQianOlRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CharacterTaoQianOlEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CharacterTaoQianOlRuntime> resolve) => new(resolve);
}
