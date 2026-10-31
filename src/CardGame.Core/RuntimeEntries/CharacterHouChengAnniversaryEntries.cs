#nullable enable
namespace CardGame.Core.RuntimeEntries;
[Core.ProgramRuntimeEntry(typeof(EngineOwners.CharacterHouChengAnniversaryRuntime))]
internal sealed partial class CharacterHouChengAnniversaryEntries
{
    private readonly Func<EngineOwners.CharacterHouChengAnniversaryRuntime> _resolve;
    private CharacterHouChengAnniversaryEntries(Func<EngineOwners.CharacterHouChengAnniversaryRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CharacterHouChengAnniversaryEntries Create(Func<EngineOwners.CharacterHouChengAnniversaryRuntime> resolve) => new(resolve);
}
