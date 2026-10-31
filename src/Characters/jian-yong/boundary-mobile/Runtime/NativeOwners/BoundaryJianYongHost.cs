#nullable enable
namespace CardGame.Core.NativeOwners;
internal sealed class BoundaryJianYongHost(global::System.Func<global::CardGame.Core.RuntimeEntries.CharacterJianYongBoundaryMobileEntries> getCharacterJianYongBoundaryMobileRuntime) : IBoundaryJianYongHost
{
    public SkillProgramStepOutcome Resolve(ProgramSkillFrame f, SkillProgramEffectOp op) => op == SkillProgramEffectOp.ResolveBoundaryJianYongZongshi ? getCharacterJianYongBoundaryMobileRuntime().BeginBoundaryJianYongZongshiChoice(f) : getCharacterJianYongBoundaryMobileRuntime().RecordBoundaryJianYongFailure(f);
}
