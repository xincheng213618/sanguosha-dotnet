#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CharacterChenDengOlEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterChenDengOlRuntime> _resolve;
    private CharacterChenDengOlEntries(global::System.Func<global::CardGame.Core.EngineOwners.CharacterChenDengOlRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CharacterChenDengOlEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CharacterChenDengOlRuntime> resolve) => new(resolve);
}
