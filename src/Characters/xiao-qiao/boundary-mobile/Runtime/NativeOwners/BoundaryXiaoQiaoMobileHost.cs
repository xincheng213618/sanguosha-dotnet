#nullable enable

namespace CardGame.Core.NativeOwners;
internal sealed class BoundaryXiaoQiaoMobileHost(Func<RuntimeEntries.CharacterXiaoQiaoBoundaryMobileEntries> getCharacterXiaoQiaoBoundaryMobileRuntime) : IBoundaryXiaoQiaoMobileHost
{
    public SkillProgramStepOutcome Rewrite(ProgramSkillFrame f) => getCharacterXiaoQiaoBoundaryMobileRuntime().BeginBoundaryXiaoQiaoJudgment(f);
}
