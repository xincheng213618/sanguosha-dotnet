#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CharacterTangZiOlEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterTangZiOlRuntime> _resolve;
    private CharacterTangZiOlEntries(global::System.Func<global::CardGame.Core.EngineOwners.CharacterTangZiOlRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CharacterTangZiOlEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CharacterTangZiOlRuntime> resolve) => new(resolve);
}
