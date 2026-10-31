#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CharacterDuFuRenOlEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterDuFuRenOlRuntime> _resolve;
    private CharacterDuFuRenOlEntries(global::System.Func<global::CardGame.Core.EngineOwners.CharacterDuFuRenOlRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CharacterDuFuRenOlEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CharacterDuFuRenOlRuntime> resolve) => new(resolve);
}
