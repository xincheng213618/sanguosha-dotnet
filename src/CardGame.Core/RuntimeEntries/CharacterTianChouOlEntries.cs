#nullable enable
namespace CardGame.Core.RuntimeEntries;
[global::CardGame.Core.ProgramRuntimeEntry(typeof(global::CardGame.Core.EngineOwners.CharacterTianChouOlRuntime))]
internal sealed partial class CharacterTianChouOlEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterTianChouOlRuntime> _resolve;
    private CharacterTianChouOlEntries(global::System.Func<global::CardGame.Core.EngineOwners.CharacterTianChouOlRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CharacterTianChouOlEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CharacterTianChouOlRuntime> resolve) => new(resolve);
}
