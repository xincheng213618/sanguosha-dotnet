#nullable enable
namespace CardGame.Core.NativeOwners;
internal sealed class MobileZhugeGuoHost(global::System.Func<global::CardGame.Core.RuntimeEntries.CharacterZhugeGuoMobileEntries> getCharacterZhugeGuoMobileRuntime) : IMobileZhugeGuoHost
{
    public SkillProgramStepOutcome Resolve(ProgramSkillFrame f, SkillProgramEffectOp op) => getCharacterZhugeGuoMobileRuntime().BeginMobileZhugeGuo(f, op);
}
