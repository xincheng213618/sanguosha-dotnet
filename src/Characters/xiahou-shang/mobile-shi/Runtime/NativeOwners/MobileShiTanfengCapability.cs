#nullable enable
namespace CardGame.Core.NativeOwners;
internal sealed class MobileShiTanfengCapability(Func<RuntimeEntries.DeferredRuntime<EngineOwners.CharacterXiahouShangMobileShiRuntime>> getCharacterXiahouShangMobileShiRuntime) : IMobileShiTanfengHost
{
    public SkillProgramStepOutcome SelectTarget(ProgramSkillFrame frame, SkillProgramEffect effect) => getCharacterXiahouShangMobileShiRuntime().Value.SelectMobileShiTanfengTarget(frame, effect);
}
