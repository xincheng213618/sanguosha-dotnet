#nullable enable
namespace CardGame.Core.RuntimeEntries;
[Core.ProgramRuntimeEntry(typeof(EngineOwners.CharacterBaiLingYunAnniversaryRuntime))]
internal sealed partial class CharacterBaiLingYunAnniversaryEntries
{
    private readonly Func<EngineOwners.CharacterBaiLingYunAnniversaryRuntime> _resolve;
    private CharacterBaiLingYunAnniversaryEntries(Func<EngineOwners.CharacterBaiLingYunAnniversaryRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CharacterBaiLingYunAnniversaryEntries Create(Func<EngineOwners.CharacterBaiLingYunAnniversaryRuntime> resolve) => new(resolve);
}
