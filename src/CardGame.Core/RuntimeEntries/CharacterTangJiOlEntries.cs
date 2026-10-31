#nullable enable
namespace CardGame.Core.RuntimeEntries;
[global::CardGame.Core.ProgramRuntimeEntry(typeof(global::CardGame.Core.EngineOwners.CharacterTangJiOlRuntime))]
internal sealed partial class CharacterTangJiOlEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterTangJiOlRuntime> _resolve;
    private CharacterTangJiOlEntries(global::System.Func<global::CardGame.Core.EngineOwners.CharacterTangJiOlRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CharacterTangJiOlEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CharacterTangJiOlRuntime> resolve) => new(resolve);
}
