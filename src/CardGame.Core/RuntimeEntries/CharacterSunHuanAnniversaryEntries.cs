#nullable enable
namespace CardGame.Core.RuntimeEntries;
[Core.ProgramRuntimeEntry(typeof(EngineOwners.CharacterSunHuanAnniversaryRuntime))]
internal sealed partial class CharacterSunHuanAnniversaryEntries
{
    private readonly Func<EngineOwners.CharacterSunHuanAnniversaryRuntime> _resolve;
    private CharacterSunHuanAnniversaryEntries(Func<EngineOwners.CharacterSunHuanAnniversaryRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CharacterSunHuanAnniversaryEntries Create(Func<EngineOwners.CharacterSunHuanAnniversaryRuntime> resolve) => new(resolve);
}
