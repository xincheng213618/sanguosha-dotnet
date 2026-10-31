#nullable enable
namespace CardGame.Core.RuntimeEntries;
[global::CardGame.Core.ProgramRuntimeEntry(typeof(global::CardGame.Core.EngineOwners.CharacterPangJiOlRuntime))]
internal sealed partial class CharacterPangJiOlEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterPangJiOlRuntime> _resolve;
    private CharacterPangJiOlEntries(global::System.Func<global::CardGame.Core.EngineOwners.CharacterPangJiOlRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CharacterPangJiOlEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CharacterPangJiOlRuntime> resolve) => new(resolve);
}
