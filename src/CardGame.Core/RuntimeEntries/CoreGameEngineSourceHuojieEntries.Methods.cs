#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineSourceHuojieEntries
{
    internal bool CanBeginSourceHuojieJudgment(ProgramSkillFrame f, int target, string reason, string bind, SkillProgramCardSetVisibility visibility) => _resolve().CanBeginSourceHuojieJudgment(f, target, reason, bind, visibility);
    internal void ContinueSourceHuojie(ProgramSkillFrame f) => _resolve().ContinueSourceHuojie(f);
    internal bool IsSourceHuojieJudgment(JudgmentFrame j, ProgramSkillFrame f) => _resolve().IsSourceHuojieJudgment(j, f);
    internal bool IsSourceHuojieLightningJudgment(JudgmentFrame j) => _resolve().IsSourceHuojieLightningJudgment(j);
    internal bool ReturnSourceHuojieJudgment(JudgmentFrame j) => _resolve().ReturnSourceHuojieJudgment(j);
    internal void SourceHuojieActualReceivedDamage(DamageAppliedEvent fact) => _resolve().SourceHuojieActualReceivedDamage(fact);
    internal SkillProgramStepOutcome StartSourceHuojie(ProgramSkillFrame supplied) => _resolve().StartSourceHuojie(supplied);
}
