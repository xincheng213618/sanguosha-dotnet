#nullable enable

namespace CardGame.Core.NativeOwners;
internal sealed class BoundaryQuanCongHost(Func<RuntimeEntries.CharacterQuanCongBoundaryMobileEntries> getCharacterQuanCongBoundaryMobileRuntime) : IBoundaryQuanCongProgramHost
{
    public SkillProgramStepOutcome Resolve(ProgramSkillFrame frame) => getCharacterQuanCongBoundaryMobileRuntime().BeginBoundaryYaoming(frame);
}
