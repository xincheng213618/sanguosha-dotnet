#nullable enable
namespace CardGame.Core.NativeOwners;
internal sealed class MobileShiTanfengCapability(Func<RuntimeEntries.CharacterXiahouShangMobileShiEntries> getCharacterXiahouShangMobileShiRuntime) : IMobileShiTanfengHost
{
    public SkillProgramStepOutcome SelectTarget(ProgramSkillFrame frame, SkillProgramEffect effect) => getCharacterXiahouShangMobileShiRuntime().SelectMobileShiTanfengTarget(frame, effect);
}
