#nullable enable
namespace CardGame.Core.RuntimeEntries;
[Core.ProgramRuntimeEntry(typeof(EngineOwners.CharacterKanZeAnniversaryRuntime))]
internal sealed partial class CharacterKanZeAnniversaryEntries
{
    private readonly Func<EngineOwners.CharacterKanZeAnniversaryRuntime> _resolve;
    private CharacterKanZeAnniversaryEntries(Func<EngineOwners.CharacterKanZeAnniversaryRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CharacterKanZeAnniversaryEntries Create(Func<EngineOwners.CharacterKanZeAnniversaryRuntime> resolve) => new(resolve);
}
