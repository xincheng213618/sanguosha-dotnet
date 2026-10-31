#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CharacterLvKaiOlEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterLvKaiOlRuntime> _resolve;
    private CharacterLvKaiOlEntries(global::System.Func<global::CardGame.Core.EngineOwners.CharacterLvKaiOlRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CharacterLvKaiOlEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CharacterLvKaiOlRuntime> resolve) => new(resolve);
}
