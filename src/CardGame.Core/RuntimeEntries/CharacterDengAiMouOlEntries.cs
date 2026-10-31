#nullable enable
namespace CardGame.Core.RuntimeEntries;
[global::CardGame.Core.ProgramRuntimeEntry(typeof(global::CardGame.Core.EngineOwners.CharacterDengAiMouOlRuntime))]
internal sealed partial class CharacterDengAiMouOlEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterDengAiMouOlRuntime> _resolve;
    private CharacterDengAiMouOlEntries(global::System.Func<global::CardGame.Core.EngineOwners.CharacterDengAiMouOlRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CharacterDengAiMouOlEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CharacterDengAiMouOlRuntime> resolve) => new(resolve);
}
