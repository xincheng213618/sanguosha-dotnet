#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CharacterWuAnGuoOlEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterWuAnGuoOlRuntime> _resolve;
    private CharacterWuAnGuoOlEntries(global::System.Func<global::CardGame.Core.EngineOwners.CharacterWuAnGuoOlRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CharacterWuAnGuoOlEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CharacterWuAnGuoOlRuntime> resolve) => new(resolve);
}
