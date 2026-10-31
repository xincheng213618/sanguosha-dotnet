#nullable enable

namespace CardGame.Core.NativeOwners;
internal sealed class MouGuoHuaiCapability(Func<RuntimeEntries.DeferredRuntime<EngineOwners.CharacterGuoHuaiMouMobileRuntime>> getCharacterGuoHuaiMouMobileRuntime) : IMouGuoHuaiMobileHost
{
    public SkillProgramStepOutcome Resolve(ProgramSkillFrame f, SkillProgramEffectOp op) => getCharacterGuoHuaiMouMobileRuntime().Value.BeginMouGh(f, op);
}
