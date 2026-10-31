#nullable enable
namespace CardGame.Core.NativeOwners;
internal sealed class BoundaryJianYongHost(Func<RuntimeEntries.CharacterJianYongBoundaryMobileEntries> getCharacterJianYongBoundaryMobileRuntime) : IBoundaryJianYongHost
{
    public SkillProgramStepOutcome Resolve(ProgramSkillFrame f, SkillProgramEffectOp op) => op == SkillProgramEffectOp.ResolveBoundaryJianYongZongshi ? getCharacterJianYongBoundaryMobileRuntime().BeginBoundaryJianYongZongshiChoice(f) : getCharacterJianYongBoundaryMobileRuntime().RecordBoundaryJianYongFailure(f);
}
