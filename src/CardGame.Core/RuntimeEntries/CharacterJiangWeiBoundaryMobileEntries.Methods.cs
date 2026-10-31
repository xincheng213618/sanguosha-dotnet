#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CharacterJiangWeiBoundaryMobileEntries
{
    internal bool IsMobileJiangWeiAssistedFrame(global::CardGame.Core.ProgramSkillFrame f, int actor) => _resolve().IsMobileJiangWeiAssistedFrame(f, actor);
    internal int[]? MobileJiangWeiAssistedTargets(global::CardGame.Core.ProgramSkillFrame f, int actor) => _resolve().MobileJiangWeiAssistedTargets(f, actor);
    internal bool TryBeginMobileJiangWeiTiaoxin(global::CardGame.Core.ProgramSkillFrame supplied, int actor, string bind, bool actorChoosesTarget, scoped out global::CardGame.Core.SkillProgramStepOutcome outcome) => _resolve().TryBeginMobileJiangWeiTiaoxin(supplied, actor, bind, actorChoosesTarget, out outcome);
}
