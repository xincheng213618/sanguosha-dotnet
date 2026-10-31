#nullable enable
namespace CardGame.Core.RuntimeEntries;
[Core.ProgramRuntimeEntry(typeof(EngineOwners.CharacterDengAiMouOlRuntime))]
internal sealed partial class CharacterDengAiMouOlEntries
{
    private readonly Func<EngineOwners.CharacterDengAiMouOlRuntime> _resolve;
    private CharacterDengAiMouOlEntries(Func<EngineOwners.CharacterDengAiMouOlRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CharacterDengAiMouOlEntries Create(Func<EngineOwners.CharacterDengAiMouOlRuntime> resolve) => new(resolve);
}
