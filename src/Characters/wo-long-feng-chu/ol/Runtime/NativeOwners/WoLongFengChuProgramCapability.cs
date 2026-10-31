#nullable enable
namespace CardGame.Core.NativeOwners;
internal sealed class WoLongFengChuProgramCapability(global::System.Func<global::CardGame.Core.RuntimeEntries.CharacterWoLongFengChuOlEntries> getCharacterWoLongFengChuOlRuntime) : IWoLongFengChuProgramHost
{
    public SkillProgramStepOutcome ResolveLuanfeng(ProgramSkillFrame frame) => getCharacterWoLongFengChuOlRuntime().ResolveLuanfeng(frame);
}
