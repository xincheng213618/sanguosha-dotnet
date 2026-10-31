#nullable enable
namespace CardGame.Core.RuntimeEntries;
[global::CardGame.Core.ProgramRuntimeEntry(typeof(global::CardGame.Core.EngineOwners.CharacterYangYiOlRuntime))]
internal sealed partial class CharacterYangYiOlEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterYangYiOlRuntime> _resolve;
    private CharacterYangYiOlEntries(global::System.Func<global::CardGame.Core.EngineOwners.CharacterYangYiOlRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CharacterYangYiOlEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CharacterYangYiOlRuntime> resolve) => new(resolve);
}
