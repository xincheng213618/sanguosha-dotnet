#nullable enable
namespace CardGame.Core.NativeOwners;
internal sealed class MobileZhugeGuoHost(Func<RuntimeEntries.DeferredRuntime<EngineOwners.CharacterZhugeGuoMobileRuntime>> getCharacterZhugeGuoMobileRuntime) : IMobileZhugeGuoHost
{
    public SkillProgramStepOutcome Resolve(ProgramSkillFrame f, SkillProgramEffectOp op) => getCharacterZhugeGuoMobileRuntime().Value.BeginMobileZhugeGuo(f, op);
}
