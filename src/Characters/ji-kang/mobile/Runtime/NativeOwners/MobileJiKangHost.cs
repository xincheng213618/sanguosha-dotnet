#nullable enable

namespace CardGame.Core.NativeOwners;
internal sealed class MobileJiKangHost(Func<RuntimeEntries.DeferredRuntime<EngineOwners.CharacterJiKangMobileRuntime>> getCharacterJiKangMobileRuntime) : IMobileJiKangHost
{
    public SkillProgramStepOutcome Resolve(ProgramSkillFrame f, SkillProgramEffectOp op) => getCharacterJiKangMobileRuntime().Value.BeginMobileJiKang(f, op);
}
