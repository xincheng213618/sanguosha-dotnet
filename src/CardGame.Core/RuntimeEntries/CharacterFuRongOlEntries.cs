#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CharacterFuRongOlEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterFuRongOlRuntime> _resolve;
    private CharacterFuRongOlEntries(global::System.Func<global::CardGame.Core.EngineOwners.CharacterFuRongOlRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CharacterFuRongOlEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CharacterFuRongOlRuntime> resolve) => new(resolve);
}
