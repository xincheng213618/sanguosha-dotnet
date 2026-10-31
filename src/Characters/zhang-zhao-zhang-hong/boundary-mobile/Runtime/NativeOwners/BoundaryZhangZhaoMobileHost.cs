#nullable enable
using System.Text.Json.Serialization;

namespace CardGame.Core.NativeOwners;
internal sealed class BoundaryZhangZhaoMobileHost(global::System.Func<global::CardGame.Core.RuntimeEntries.CharacterZhangZhaoZhangHongBoundaryMobileEntries> getCharacterZhangZhaoZhangHongBoundaryMobileRuntime) : IBoundaryZhangZhaoMobileHost
{
    public SkillProgramStepOutcome Restore(ProgramSkillFrame f) => getCharacterZhangZhaoZhangHongBoundaryMobileRuntime().BeginBoundaryZhangZhaoPhase(f);
}
