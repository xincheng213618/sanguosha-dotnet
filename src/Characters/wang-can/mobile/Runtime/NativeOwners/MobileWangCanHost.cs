#nullable enable

namespace CardGame.Core.NativeOwners;
internal sealed class MobileWangCanHost(Func<RuntimeEntries.DeferredRuntime<EngineOwners.CharacterWangCanMobileRuntime>> getCharacterWangCanMobileRuntime) : IMobileWangCanHost
{
    public SkillProgramStepOutcome Resolve(ProgramSkillFrame frame, SkillProgramEffectOp op) => getCharacterWangCanMobileRuntime().Value.BeginMobileWangCanXi(frame, op);
}
