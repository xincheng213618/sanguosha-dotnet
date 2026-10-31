#nullable enable
namespace CardGame.Core.RuntimeEntries;
[global::CardGame.Core.ProgramRuntimeEntry(typeof(global::CardGame.Core.EngineOwners.CharacterKanZeOlRuntime))]
internal sealed partial class CharacterKanZeOlEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterKanZeOlRuntime> _resolve;
    private CharacterKanZeOlEntries(global::System.Func<global::CardGame.Core.EngineOwners.CharacterKanZeOlRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CharacterKanZeOlEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CharacterKanZeOlRuntime> resolve) => new(resolve);
}
