#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CharacterYanRouOlEntries
{
    internal bool AdvanceYanRouSubtree() => _resolve().AdvanceYanRouSubtree();
    internal PromptChoice AiYanRou(PendingDecision p, ProgramSkillFrame f) => _resolve().AiYanRou(p, f);
    internal void AnswerYanRou(PromptChoice choice) => _resolve().AnswerYanRou(choice);
    internal SkillProgramStepOutcome BeginYanRou(ProgramSkillFrame supplied, SkillProgramEffectOp op) => _resolve().BeginYanRou(supplied, op);
    internal bool CanOfferYanRou(ProgramTriggerCandidate c, SkillProgramTrigger t, ProgramSkillWindowContext context) => _resolve().CanOfferYanRou(c, t, context);
    internal bool ContinuePaidYanRou(ProgramSkillFrame f) => _resolve().ContinuePaidYanRou(f);
    internal void ContinueYanRou(long id) => _resolve().ContinueYanRou(id);
    internal bool DrainYanRou(ProgramSkillFrame f) => _resolve().DrainYanRou(f);
    internal void FinishYanRou(ProgramSkillFrame f, bool applied) => _resolve().FinishYanRou(f, applied);
    internal bool IsYanRouIssuedMovement(ProgramSkillFrame f, CardMovementRecord m) => _resolve().IsYanRouIssuedMovement(f, m);
    internal bool IsYanRouRecoveryParent(ProgramSkillFrame f, RecoveryReplacementFrame recovery) => _resolve().IsYanRouRecoveryParent(f, recovery);
    internal void PublishYanRou(ProgramSkillFrame f) => _resolve().PublishYanRou(f);
    internal void RecordYanRouMovement(long id) => _resolve().RecordYanRouMovement(id);
    internal bool ResumeYanRou(long id) => _resolve().ResumeYanRou(id);
    internal bool ReturnYanRou(ProgramSkillFrame f) => _resolve().ReturnYanRou(f);
    internal bool YanRouCanDiscard(int chooser, int payer, Card card, CardLocation from) => _resolve().YanRouCanDiscard(chooser, payer, card, from);
    internal IReadOnlyList<PromptChoice> YanRouChoices(ProgramSkillFrame f) => _resolve().YanRouChoices(f);
    internal bool YanRouFirstChild(ProgramSkillFrame f, ResolutionFrame child) => _resolve().YanRouFirstChild(f, child);
    internal ProgramSkillFrame? YanRouObserverRoot() => _resolve().YanRouObserverRoot();
    internal bool YanRouParent(ProgramTriggerCandidate candidate, ProgramSkillWindowContext c) => _resolve().YanRouParent(candidate, c);
    internal bool YanRouSilverLion(ProgramSkillFrame f) => _resolve().YanRouSilverLion(f);
    internal CardConversionSource YanRouSource(ProgramSkillFrame f) => _resolve().YanRouSource(f);
    internal bool YanRouSpent(int owner, string skill, string hash) => _resolve().YanRouSpent(owner, skill, hash);
    internal int YanRouTurnDamage(int owner) => _resolve().YanRouTurnDamage(owner);
}
