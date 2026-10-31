#nullable enable

namespace CardGame.Core.NativeOwners;
internal sealed class MouXiahouShiCapability(Func<RuntimeEntries.DeferredRuntime<EngineOwners.CharacterXiahouShiMouMobileRuntime>> getCharacterXiahouShiMouMobileRuntime) : IMouXiahouShiHost
{
    public SkillProgramStepOutcome Discard(ProgramSkillFrame f) => getCharacterXiahouShiMouMobileRuntime().Value.BeginMouXiahouShiDiscard(f);
    public SkillProgramStepOutcome Support(ProgramSkillFrame f) => getCharacterXiahouShiMouMobileRuntime().Value.BeginMouXiahouShiSupport(f);
    public SkillProgramStepOutcome Qiaoshi(ProgramSkillFrame f) => getCharacterXiahouShiMouMobileRuntime().Value.BeginMouXiahouShiQiaoshi(f);
}
