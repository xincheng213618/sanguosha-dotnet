#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CharacterTangJiOlEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterTangJiOlRuntime> _resolve;
    private CharacterTangJiOlEntries(global::System.Func<global::CardGame.Core.EngineOwners.CharacterTangJiOlRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CharacterTangJiOlEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CharacterTangJiOlRuntime> resolve) => new(resolve);
}
