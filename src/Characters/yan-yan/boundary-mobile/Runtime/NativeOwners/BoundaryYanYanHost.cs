#nullable enable
namespace CardGame.Core.NativeOwners;
internal sealed class BoundaryYanYanHost(global::System.Func<global::CardGame.Core.RuntimeEntries.CharacterYanYanBoundaryMobileEntries> getCharacterYanYanBoundaryMobileRuntime) : IBoundaryYanYanHost
{
    public SkillProgramStepOutcome Execute(ProgramSkillFrame f, SkillProgramEffectOp op) => op switch
    {
        SkillProgramEffectOp.PrepareBoundaryYanYanJuzhan => getCharacterYanYanBoundaryMobileRuntime().PrepareBoundaryYanYan(f),
        SkillProgramEffectOp.FinishBoundaryYanYanJuzhan => getCharacterYanYanBoundaryMobileRuntime().FinishBoundaryYanYan(f),
        _ => getCharacterYanYanBoundaryMobileRuntime().ClaimBoundaryYanYan(f)};
}
