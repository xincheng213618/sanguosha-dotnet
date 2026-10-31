#nullable enable
namespace CardGame.Core.RuntimeEntries;
[Core.ProgramRuntimeEntry(typeof(EngineOwners.CharacterDengAiAnniversaryGodRuntime))]
internal sealed partial class CharacterDengAiAnniversaryGodEntries
{
    private readonly Func<EngineOwners.CharacterDengAiAnniversaryGodRuntime> _resolve;
    private CharacterDengAiAnniversaryGodEntries(Func<EngineOwners.CharacterDengAiAnniversaryGodRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CharacterDengAiAnniversaryGodEntries Create(Func<EngineOwners.CharacterDengAiAnniversaryGodRuntime> resolve) => new(resolve);
}
