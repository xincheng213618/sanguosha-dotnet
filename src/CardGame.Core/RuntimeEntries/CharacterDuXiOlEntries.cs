#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CharacterDuXiOlEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterDuXiOlRuntime> _resolve;
    private CharacterDuXiOlEntries(global::System.Func<global::CardGame.Core.EngineOwners.CharacterDuXiOlRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CharacterDuXiOlEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CharacterDuXiOlRuntime> resolve) => new(resolve);
}
