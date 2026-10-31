#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CharacterXuRongOlEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterXuRongOlRuntime> _resolve;
    private CharacterXuRongOlEntries(global::System.Func<global::CardGame.Core.EngineOwners.CharacterXuRongOlRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CharacterXuRongOlEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CharacterXuRongOlRuntime> resolve) => new(resolve);
}
