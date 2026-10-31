#nullable enable
namespace CardGame.Core.NativeOwners;
internal sealed class BoundaryZhangNiHost(Func<RuntimeEntries.CharacterZhangNiBoundaryMobileEntries> getCharacterZhangNiBoundaryMobileRuntime) : IBoundaryZhangNiHost
{
    public SkillProgramStepOutcome Resolve(ProgramSkillFrame f, SkillProgramEffect e) => e.Op == SkillProgramEffectOp.ConsumeBoundaryZhangNiDrawSkip ? getCharacterZhangNiBoundaryMobileRuntime().ConsumeBoundaryWurongSkip(f) : getCharacterZhangNiBoundaryMobileRuntime().BeginBoundaryWurong(f, e);
}
