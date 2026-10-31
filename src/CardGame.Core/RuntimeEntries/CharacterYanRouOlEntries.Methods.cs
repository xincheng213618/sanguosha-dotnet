#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CharacterYanRouOlEntries
{
    internal bool AdvanceYanRouSubtree() => _resolve().AdvanceYanRouSubtree();
    internal global::CardGame.Core.PromptChoice AiYanRou(global::CardGame.Core.PendingDecision p, global::CardGame.Core.ProgramSkillFrame f) => _resolve().AiYanRou(p, f);
    internal void AnswerYanRou(global::CardGame.Core.PromptChoice choice) => _resolve().AnswerYanRou(choice);
    internal global::CardGame.Core.SkillProgramStepOutcome BeginYanRou(global::CardGame.Core.ProgramSkillFrame supplied, global::CardGame.Core.SkillProgramEffectOp op) => _resolve().BeginYanRou(supplied, op);
    internal bool CanOfferYanRou(global::CardGame.Core.ProgramTriggerCandidate c, global::CardGame.Core.SkillProgramTrigger t, global::CardGame.Core.ProgramSkillWindowContext context) => _resolve().CanOfferYanRou(c, t, context);
    internal bool ContinuePaidYanRou(global::CardGame.Core.ProgramSkillFrame f) => _resolve().ContinuePaidYanRou(f);
    internal void ContinueYanRou(long id) => _resolve().ContinueYanRou(id);
    internal bool DrainYanRou(global::CardGame.Core.ProgramSkillFrame f) => _resolve().DrainYanRou(f);
    internal void FinishYanRou(global::CardGame.Core.ProgramSkillFrame f, bool applied) => _resolve().FinishYanRou(f, applied);
    internal bool IsYanRouIssuedMovement(global::CardGame.Core.ProgramSkillFrame f, global::CardGame.Core.CardMovementRecord m) => _resolve().IsYanRouIssuedMovement(f, m);
    internal bool IsYanRouRecoveryParent(global::CardGame.Core.ProgramSkillFrame f, global::CardGame.Core.RecoveryReplacementFrame recovery) => _resolve().IsYanRouRecoveryParent(f, recovery);
    internal void PublishYanRou(global::CardGame.Core.ProgramSkillFrame f) => _resolve().PublishYanRou(f);
    internal void RecordYanRouMovement(long id) => _resolve().RecordYanRouMovement(id);
    internal bool ResumeYanRou(long id) => _resolve().ResumeYanRou(id);
    internal bool ReturnYanRou(global::CardGame.Core.ProgramSkillFrame f) => _resolve().ReturnYanRou(f);
    internal bool YanRouCanDiscard(int chooser, int payer, global::CardGame.Core.Card card, global::CardGame.Core.CardLocation from) => _resolve().YanRouCanDiscard(chooser, payer, card, from);
    internal global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.PromptChoice> YanRouChoices(global::CardGame.Core.ProgramSkillFrame f) => _resolve().YanRouChoices(f);
    internal bool YanRouFirstChild(global::CardGame.Core.ProgramSkillFrame f, global::CardGame.Core.ResolutionFrame child) => _resolve().YanRouFirstChild(f, child);
    internal global::CardGame.Core.ProgramSkillFrame? YanRouObserverRoot() => _resolve().YanRouObserverRoot();
    internal bool YanRouParent(global::CardGame.Core.ProgramTriggerCandidate candidate, global::CardGame.Core.ProgramSkillWindowContext c) => _resolve().YanRouParent(candidate, c);
    internal bool YanRouSilverLion(global::CardGame.Core.ProgramSkillFrame f) => _resolve().YanRouSilverLion(f);
    internal global::CardGame.Core.CardConversionSource YanRouSource(global::CardGame.Core.ProgramSkillFrame f) => _resolve().YanRouSource(f);
    internal bool YanRouSpent(int owner, string skill, string hash) => _resolve().YanRouSpent(owner, skill, hash);
    internal int YanRouTurnDamage(int owner) => _resolve().YanRouTurnDamage(owner);
}
