#nullable enable

namespace CardGame.Core.NativeOwners;
internal sealed class MouXiahouDunMobileCapability(Func<RuntimeEntries.DeferredRuntime<EngineOwners.CharacterXiahouDunMouMobileRuntime>> getCharacterXiahouDunMouMobileRuntime) : IMouXiahouDunMobileHost
{
    public SkillProgramStepOutcome Start(ProgramSkillFrame f, SkillProgramEffectOp op) => getCharacterXiahouDunMouMobileRuntime().Value.StartMouXiahouDunMobile(f, op);
}
