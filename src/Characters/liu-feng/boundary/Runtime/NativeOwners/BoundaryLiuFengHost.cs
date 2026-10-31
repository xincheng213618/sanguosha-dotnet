#nullable enable

namespace CardGame.Core.NativeOwners;
internal sealed class BoundaryLiuFengHost(Func<RuntimeEntries.CharacterLiuFengBoundaryEntries> getCharacterLiuFengBoundaryRuntime) : IBoundaryLiuFengHost
{
    public SkillProgramStepOutcome Begin(ProgramSkillFrame f, SkillProgramEffectOp op) => getCharacterLiuFengBoundaryRuntime().BeginBoundaryLiuFeng(f, op);
}
