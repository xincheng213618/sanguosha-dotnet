#nullable enable
namespace CardGame.Core.RuntimeEntries;
[global::CardGame.Core.ProgramRuntimeEntry(typeof(global::CardGame.Core.EngineOwners.CharacterFanChouOlRuntime))]
internal sealed partial class CharacterFanChouOlEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterFanChouOlRuntime> _resolve;
    private CharacterFanChouOlEntries(global::System.Func<global::CardGame.Core.EngineOwners.CharacterFanChouOlRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CharacterFanChouOlEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CharacterFanChouOlRuntime> resolve) => new(resolve);
}
