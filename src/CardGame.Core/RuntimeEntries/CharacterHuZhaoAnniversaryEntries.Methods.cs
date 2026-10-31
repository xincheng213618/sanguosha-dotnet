#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CharacterHuZhaoAnniversaryEntries
{
    internal bool AdvanceHuZhaoSubtree() => _resolve().AdvanceHuZhaoSubtree();
    internal void AnswerHuZhaoMidu(global::CardGame.Core.PromptChoice c) => _resolve().AnswerHuZhaoMidu(c);
    internal global::CardGame.Core.SkillProgramStepOutcome BeginHuZhaoMidu(global::CardGame.Core.ProgramSkillFrame supplied) => _resolve().BeginHuZhaoMidu(supplied);
    internal void ContinueHuZhaoMidu(long id) => _resolve().ContinueHuZhaoMidu(id);
    internal bool ContinuePaidHuZhao(global::CardGame.Core.ProgramSkillFrame f) => _resolve().ContinuePaidHuZhao(f);
    internal bool DrainHuZhaoChildren(long id) => _resolve().DrainHuZhaoChildren(id);
    internal global::CardGame.Core.PromptChoice HuZhaoAi(global::CardGame.Core.PendingDecision p, global::CardGame.Core.ProgramSkillFrame f) => _resolve().HuZhaoAi(p, f);
    internal int HuZhaoAvailable(int seat) => _resolve().HuZhaoAvailable(seat);
    internal bool HuZhaoBeforeDamageObserver(long id) => _resolve().HuZhaoBeforeDamageObserver(id);
    internal global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.PromptChoice> HuZhaoChoices(global::CardGame.Core.ProgramSkillFrame f) => _resolve().HuZhaoChoices(f);
    internal bool HuZhaoDamageObserver(long id) => _resolve().HuZhaoDamageObserver(id);
    internal void HuZhaoDistance(global::CardGame.Core.ProgramDistanceQuery q, global::System.Collections.Generic.List<global::CardGame.Core.RuleQueryBaseTerm> terms) => _resolve().HuZhaoDistance(q, terms);
    internal bool HuZhaoFirstChild(global::CardGame.Core.ProgramSkillFrame f, global::CardGame.Core.ResolutionFrame child) => _resolve().HuZhaoFirstChild(f, child);
    internal bool HuZhaoMovement(global::CardGame.Core.ProgramSkillFrame f, global::CardGame.Core.SkillProgramEffect? e, global::CardGame.Core.ProgramMovementContinuation p) => _resolve().HuZhaoMovement(f, e, p);
    internal global::CardGame.Core.ProgramSkillFrame? HuZhaoObserverRoot() => _resolve().HuZhaoObserverRoot();
    internal bool HuZhaoRecoveryParent(global::CardGame.Core.ProgramSkillFrame f, global::CardGame.Core.RecoveryReplacementFrame recovery) => _resolve().HuZhaoRecoveryParent(f, recovery);
    internal global::CardGame.Core.CardConversionSource HuZhaoSource(global::CardGame.Core.ProgramSkillFrame f) => _resolve().HuZhaoSource(f);
    internal void PublishHuZhaoMidu(global::CardGame.Core.ProgramSkillFrame f) => _resolve().PublishHuZhaoMidu(f);
    internal bool ResumeHuZhaoMidu(long id) => _resolve().ResumeHuZhaoMidu(id);
    internal void ReturnHuZhaoDraw(global::CardGame.Core.ProgramSkillFrame supplied, global::CardGame.Core.NativeDrawOutcome outcome) => _resolve().ReturnHuZhaoDraw(supplied, outcome);
    internal bool ReturnHuZhaoMovement(global::CardGame.Core.ProgramSkillFrame f) => _resolve().ReturnHuZhaoMovement(f);
    internal bool ValidHuZhaoMidu(global::CardGame.Core.ProgramSkillFrame f) => _resolve().ValidHuZhaoMidu(f);
}
