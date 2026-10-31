#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineSourceHuojieEntries
{
    internal bool CanBeginSourceHuojieJudgment(global::CardGame.Core.ProgramSkillFrame f, int target, string reason, string bind, global::CardGame.Core.SkillProgramCardSetVisibility visibility) => _resolve().CanBeginSourceHuojieJudgment(f, target, reason, bind, visibility);
    internal void ContinueSourceHuojie(global::CardGame.Core.ProgramSkillFrame f) => _resolve().ContinueSourceHuojie(f);
    internal bool IsSourceHuojieJudgment(global::CardGame.Core.JudgmentFrame j, global::CardGame.Core.ProgramSkillFrame f) => _resolve().IsSourceHuojieJudgment(j, f);
    internal bool IsSourceHuojieLightningJudgment(global::CardGame.Core.JudgmentFrame j) => _resolve().IsSourceHuojieLightningJudgment(j);
    internal bool ReturnSourceHuojieJudgment(global::CardGame.Core.JudgmentFrame j) => _resolve().ReturnSourceHuojieJudgment(j);
    internal void SourceHuojieActualReceivedDamage(global::CardGame.Core.DamageAppliedEvent fact) => _resolve().SourceHuojieActualReceivedDamage(fact);
    internal global::CardGame.Core.SkillProgramStepOutcome StartSourceHuojie(global::CardGame.Core.ProgramSkillFrame supplied) => _resolve().StartSourceHuojie(supplied);
}
