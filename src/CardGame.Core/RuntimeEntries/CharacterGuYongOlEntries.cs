#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CharacterGuYongOlEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterGuYongOlRuntime> _resolve;
    private CharacterGuYongOlEntries(global::System.Func<global::CardGame.Core.EngineOwners.CharacterGuYongOlRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CharacterGuYongOlEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CharacterGuYongOlRuntime> resolve) => new(resolve);
}
