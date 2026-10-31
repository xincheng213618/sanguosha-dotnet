#nullable enable
namespace CardGame.Core.NativeOwners;
internal sealed class MobileZhugeGuoHost(Func<RuntimeEntries.CharacterZhugeGuoMobileEntries> getCharacterZhugeGuoMobileRuntime) : IMobileZhugeGuoHost
{
    public SkillProgramStepOutcome Resolve(ProgramSkillFrame f, SkillProgramEffectOp op) => getCharacterZhugeGuoMobileRuntime().BeginMobileZhugeGuo(f, op);
}
