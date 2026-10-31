#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CharacterJiangWeiBoundaryMobileEntries
{
    internal bool IsMobileJiangWeiAssistedFrame(ProgramSkillFrame f, int actor) => _resolve().IsMobileJiangWeiAssistedFrame(f, actor);
    internal int[]? MobileJiangWeiAssistedTargets(ProgramSkillFrame f, int actor) => _resolve().MobileJiangWeiAssistedTargets(f, actor);
    internal bool TryBeginMobileJiangWeiTiaoxin(ProgramSkillFrame supplied, int actor, string bind, bool actorChoosesTarget, scoped out SkillProgramStepOutcome outcome) => _resolve().TryBeginMobileJiangWeiTiaoxin(supplied, actor, bind, actorChoosesTarget, out outcome);
}
