#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CharacterBianFuRenOlEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterBianFuRenOlRuntime> _resolve;
    private CharacterBianFuRenOlEntries(global::System.Func<global::CardGame.Core.EngineOwners.CharacterBianFuRenOlRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CharacterBianFuRenOlEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CharacterBianFuRenOlRuntime> resolve) => new(resolve);
}
