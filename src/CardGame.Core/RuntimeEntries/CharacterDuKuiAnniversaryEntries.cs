#nullable enable
namespace CardGame.Core.RuntimeEntries;
[Core.ProgramRuntimeEntry(typeof(EngineOwners.CharacterDuKuiAnniversaryRuntime))]
internal sealed partial class CharacterDuKuiAnniversaryEntries
{
    private readonly Func<EngineOwners.CharacterDuKuiAnniversaryRuntime> _resolve;
    private CharacterDuKuiAnniversaryEntries(Func<EngineOwners.CharacterDuKuiAnniversaryRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CharacterDuKuiAnniversaryEntries Create(Func<EngineOwners.CharacterDuKuiAnniversaryRuntime> resolve) => new(resolve);
}
