#nullable enable
namespace CardGame.Core.RuntimeEntries;
[Core.ProgramRuntimeEntry(typeof(EngineOwners.CharacterZhangHengAnniversaryRuntime))]
internal sealed partial class CharacterZhangHengAnniversaryEntries
{
    private readonly Func<EngineOwners.CharacterZhangHengAnniversaryRuntime> _resolve;
    private CharacterZhangHengAnniversaryEntries(Func<EngineOwners.CharacterZhangHengAnniversaryRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CharacterZhangHengAnniversaryEntries Create(Func<EngineOwners.CharacterZhangHengAnniversaryRuntime> resolve) => new(resolve);
}
