#nullable enable
using System.Text.Json.Serialization;

namespace CardGame.Core.NativeOwners;
internal sealed class BoundaryXiaoQiaoMobileHost(global::System.Func<global::CardGame.Core.RuntimeEntries.CharacterXiaoQiaoBoundaryMobileEntries> getCharacterXiaoQiaoBoundaryMobileRuntime) : IBoundaryXiaoQiaoMobileHost
{
    public SkillProgramStepOutcome Rewrite(ProgramSkillFrame f) => getCharacterXiaoQiaoBoundaryMobileRuntime().BeginBoundaryXiaoQiaoJudgment(f);
}
