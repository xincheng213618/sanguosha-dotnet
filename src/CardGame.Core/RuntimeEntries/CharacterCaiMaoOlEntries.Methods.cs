#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CharacterCaiMaoOlEntries
{
    internal bool AdvanceCaiMaoChildren() => _resolve().AdvanceCaiMaoChildren();
    internal void AnswerCaiMao(PromptChoice choice) => _resolve().AnswerCaiMao(choice);
    internal SkillProgramStepOutcome BeginCaiMao(ProgramSkillFrame supplied, SkillProgramEffectOp op) => _resolve().BeginCaiMao(supplied, op);
    internal IReadOnlyList<PromptChoice> CaiMaoChoices(ProgramSkillFrame f) => _resolve().CaiMaoChoices(f);
    internal bool CaiMaoDamageParentMatches(ProgramSkillFrame f, ProgramSkillWindowContext context) => _resolve().CaiMaoDamageParentMatches(f, context);
    internal bool CaiMaoFirstChild(ProgramSkillFrame f, ResolutionFrame child) => _resolve().CaiMaoFirstChild(f, child);
    internal ProgramSkillFrame? CaiMaoObserverRoot() => _resolve().CaiMaoObserverRoot();
    internal bool CaiMaoSourceLive(ProgramSkillFrame f) => _resolve().CaiMaoSourceLive(f);
    internal double CaiMaoTargetScore(ProgramSkillFrame f, int target) => _resolve().CaiMaoTargetScore(f, target);
    internal PromptChoice ChooseCaiMaoAi(PendingDecision p, ProgramSkillFrame f) => _resolve().ChooseCaiMaoAi(p, f);
    internal void ContinueCaiMao(long id) => _resolve().ContinueCaiMao(id);
    internal bool ContinuePaidCaiMao(ProgramSkillFrame f) => _resolve().ContinuePaidCaiMao(f);
    internal bool DrainCaiMao(long id) => _resolve().DrainCaiMao(id);
    internal void FinishCaiMao(ProgramSkillFrame f) => _resolve().FinishCaiMao(f);
    internal void PayCaiMaoExchange(ProgramSkillFrame f) => _resolve().PayCaiMaoExchange(f);
    internal void PublishCaiMao(ProgramSkillFrame f) => _resolve().PublishCaiMao(f);
    internal bool ResumeCaiMao(long id) => _resolve().ResumeCaiMao(id);
    internal bool ValidCaiMao(ProgramSkillFrame f) => _resolve().ValidCaiMao(f);
    internal bool HasCaiMaoCapability { get => _resolve().HasCaiMaoCapability; }
}
