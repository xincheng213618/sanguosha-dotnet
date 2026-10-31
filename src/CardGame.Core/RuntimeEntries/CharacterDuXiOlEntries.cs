#nullable enable
namespace CardGame.Core.RuntimeEntries;
[global::CardGame.Core.ProgramRuntimeEntry(typeof(global::CardGame.Core.EngineOwners.CharacterDuXiOlRuntime))]
internal sealed partial class CharacterDuXiOlEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterDuXiOlRuntime> _resolve;
    private CharacterDuXiOlEntries(global::System.Func<global::CardGame.Core.EngineOwners.CharacterDuXiOlRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CharacterDuXiOlEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CharacterDuXiOlRuntime> resolve) => new(resolve);
}
