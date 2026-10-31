#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CharacterHuZhaoAnniversaryEntries
{
    internal bool AdvanceHuZhaoSubtree() => _resolve().AdvanceHuZhaoSubtree();
    internal void AnswerHuZhaoMidu(PromptChoice c) => _resolve().AnswerHuZhaoMidu(c);
    internal SkillProgramStepOutcome BeginHuZhaoMidu(ProgramSkillFrame supplied) => _resolve().BeginHuZhaoMidu(supplied);
    internal void ContinueHuZhaoMidu(long id) => _resolve().ContinueHuZhaoMidu(id);
    internal bool ContinuePaidHuZhao(ProgramSkillFrame f) => _resolve().ContinuePaidHuZhao(f);
    internal bool DrainHuZhaoChildren(long id) => _resolve().DrainHuZhaoChildren(id);
    internal PromptChoice HuZhaoAi(PendingDecision p, ProgramSkillFrame f) => _resolve().HuZhaoAi(p, f);
    internal int HuZhaoAvailable(int seat) => _resolve().HuZhaoAvailable(seat);
    internal IReadOnlyList<PromptChoice> HuZhaoChoices(ProgramSkillFrame f) => _resolve().HuZhaoChoices(f);
    internal void HuZhaoDistance(ProgramDistanceQuery q, List<RuleQueryBaseTerm> terms) => _resolve().HuZhaoDistance(q, terms);
    internal bool HuZhaoFirstChild(ProgramSkillFrame f, ResolutionFrame child) => _resolve().HuZhaoFirstChild(f, child);
    internal ProgramSkillFrame? HuZhaoObserverRoot() => _resolve().HuZhaoObserverRoot();
    internal bool HuZhaoRecoveryParent(ProgramSkillFrame f, RecoveryReplacementFrame recovery) => _resolve().HuZhaoRecoveryParent(f, recovery);
    internal CardConversionSource HuZhaoSource(ProgramSkillFrame f) => _resolve().HuZhaoSource(f);
    internal void PublishHuZhaoMidu(ProgramSkillFrame f) => _resolve().PublishHuZhaoMidu(f);
    internal bool ResumeHuZhaoMidu(long id) => _resolve().ResumeHuZhaoMidu(id);
    internal void ReturnHuZhaoDraw(ProgramSkillFrame supplied, NativeDrawOutcome outcome) => _resolve().ReturnHuZhaoDraw(supplied, outcome);
    internal bool ReturnHuZhaoMovement(ProgramSkillFrame f) => _resolve().ReturnHuZhaoMovement(f);
    internal bool ValidHuZhaoMidu(ProgramSkillFrame f) => _resolve().ValidHuZhaoMidu(f);
}
