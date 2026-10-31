#nullable enable

namespace CardGame.Core.NativeOwners;
internal sealed class MouFaZhengCapability(Func<RuntimeEntries.DeferredRuntime<EngineOwners.CharacterFaZhengMouMobileRuntime>> getCharacterFaZhengMouMobileRuntime) : IMouFaZhengHost
{
    public SkillProgramStepOutcome Start(ProgramSkillFrame f, SkillProgramEffectOp op) => getCharacterFaZhengMouMobileRuntime().Value.StartMouFaZheng(f, op);
}
