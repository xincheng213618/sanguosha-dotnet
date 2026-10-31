#nullable enable

namespace CardGame.Core.NativeOwners;
internal sealed class BoundaryZhangZhaoMobileHost(Func<RuntimeEntries.CharacterZhangZhaoZhangHongBoundaryMobileEntries> getCharacterZhangZhaoZhangHongBoundaryMobileRuntime) : IBoundaryZhangZhaoMobileHost
{
    public SkillProgramStepOutcome Restore(ProgramSkillFrame f) => getCharacterZhangZhaoZhangHongBoundaryMobileRuntime().BeginBoundaryZhangZhaoPhase(f);
}
