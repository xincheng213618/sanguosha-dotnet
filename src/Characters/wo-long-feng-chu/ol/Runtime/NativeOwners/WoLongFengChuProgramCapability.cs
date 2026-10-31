#nullable enable
namespace CardGame.Core.NativeOwners;
internal sealed class WoLongFengChuProgramCapability(Func<RuntimeEntries.CharacterWoLongFengChuOlEntries> getCharacterWoLongFengChuOlRuntime) : IWoLongFengChuProgramHost
{
    public SkillProgramStepOutcome ResolveLuanfeng(ProgramSkillFrame frame) => getCharacterWoLongFengChuOlRuntime().ResolveLuanfeng(frame);
}
