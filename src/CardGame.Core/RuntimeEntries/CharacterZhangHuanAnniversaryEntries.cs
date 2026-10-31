#nullable enable
namespace CardGame.Core.RuntimeEntries;
[Core.ProgramRuntimeEntry(typeof(EngineOwners.CharacterZhangHuanAnniversaryRuntime))]
internal sealed partial class CharacterZhangHuanAnniversaryEntries
{
    private readonly Func<EngineOwners.CharacterZhangHuanAnniversaryRuntime> _resolve;
    private CharacterZhangHuanAnniversaryEntries(Func<EngineOwners.CharacterZhangHuanAnniversaryRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CharacterZhangHuanAnniversaryEntries Create(Func<EngineOwners.CharacterZhangHuanAnniversaryRuntime> resolve) => new(resolve);
}
