#nullable enable
using System.Globalization;

namespace CardGame.Core.NativeOwners;
internal sealed class BoundaryQuanCongHost(global::System.Func<global::CardGame.Core.RuntimeEntries.CharacterQuanCongBoundaryMobileEntries> getCharacterQuanCongBoundaryMobileRuntime) : IBoundaryQuanCongProgramHost
{
    public SkillProgramStepOutcome Resolve(ProgramSkillFrame frame) => getCharacterQuanCongBoundaryMobileRuntime().BeginBoundaryYaoming(frame);
}
