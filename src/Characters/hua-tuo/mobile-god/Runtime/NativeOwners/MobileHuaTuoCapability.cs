#nullable enable

namespace CardGame.Core.NativeOwners;
internal sealed class MobileHuaTuoCapability(Func<RuntimeEntries.DeferredRuntime<EngineOwners.CharacterHuaTuoMobileGodRuntime>> getCharacterHuaTuoMobileGodRuntime) : IMobileHuaTuoHost
{
    public SkillProgramStepOutcome Resolve(ProgramSkillFrame frame, SkillProgramEffectOp op) => getCharacterHuaTuoMobileGodRuntime().Value.BeginMobileHt(frame, op);
}
