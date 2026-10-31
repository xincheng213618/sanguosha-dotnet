#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CharacterJiKangMobileEntries
{
    internal bool AdvanceMobileJiKangHealth() => _resolve().AdvanceMobileJiKangHealth();
    internal void AnswerMobileJiKang(global::CardGame.Core.PromptChoice selected) => _resolve().AnswerMobileJiKang(selected);
    internal global::CardGame.Core.SkillProgramStepOutcome BeginMobileJiKang(global::CardGame.Core.ProgramSkillFrame supplied, global::CardGame.Core.SkillProgramEffectOp op) => _resolve().BeginMobileJiKang(supplied, op);
    internal bool CanActivateMobileJiKang(global::CardGame.Core.ProgramActivationLegalityContext gate) => _resolve().CanActivateMobileJiKang(gate);
    internal void ContinueMobileJiKang(long id) => _resolve().ContinueMobileJiKang(id);
    internal bool DrainMobileJiKang(long id) => _resolve().DrainMobileJiKang(id);
    internal void FinishMobileJiKang(global::CardGame.Core.ProgramSkillFrame f) => _resolve().FinishMobileJiKang(f);
    internal bool KeepMobileJiKangIssued(global::CardGame.Core.ProgramSkillFrame f) => _resolve().KeepMobileJiKangIssued(f);
    internal void LoseMobileJiKangHp(global::CardGame.Core.ProgramSkillFrame f, int target, global::CardGame.Core.MobileJiKangStage after) => _resolve().LoseMobileJiKangHp(f, target, after);
    internal global::System.Collections.Generic.Dictionary<string, string> MobileJiKangArgs(global::CardGame.Core.ProgramSkillFrame f, string branch) => _resolve().MobileJiKangArgs(f, branch);
    internal global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.PromptChoice> MobileJiKangChoices(global::CardGame.Core.ProgramSkillFrame f) => _resolve().MobileJiKangChoices(f);
    internal global::System.Collections.Generic.IEnumerable<global::CardGame.Core.Card> MobileJiKangClubCards(int actor) => _resolve().MobileJiKangClubCards(actor);
    internal global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.PromptChoice> MobileJiKangCosts(long frame, int owner) => _resolve().MobileJiKangCosts(frame, owner);
    internal void MobileJiKangDrawReturned(global::CardGame.Core.ProgramSkillFrame supplied, global::CardGame.Core.NativeDrawOutcome outcome) => _resolve().MobileJiKangDrawReturned(supplied, outcome);
    internal bool MobileJiKangDyingProof() => _resolve().MobileJiKangDyingProof();
    internal bool MobileJiKangHasCost(global::CardGame.Core.CharacterState owner) => _resolve().MobileJiKangHasCost(owner);
    internal bool MobileJiKangObserverChild(global::CardGame.Core.ProgramSkillFrame f, global::CardGame.Core.ResolutionFrame child) => _resolve().MobileJiKangObserverChild(f, child);
    internal global::CardGame.Core.ProgramSkillFrame? MobileJiKangObserverRoot() => _resolve().MobileJiKangObserverRoot();
    internal bool MobileJiKangPaidInstruction(global::CardGame.Core.ProgramSkillFrame f) => _resolve().MobileJiKangPaidInstruction(f);
    internal bool MobileJiKangTargetAllowed(global::CardGame.Core.CardConversionSource source, int seat) => _resolve().MobileJiKangTargetAllowed(source, seat);
    internal void PayMobileJiKang(global::CardGame.Core.ProgramSkillFrame f, global::System.Collections.Generic.IReadOnlyList<int> cards, int actor, int subject, global::CardGame.Core.MobileJiKangStage after) => _resolve().PayMobileJiKang(f, cards, actor, subject, after);
    internal void PublishMobileJiKang(global::CardGame.Core.ProgramSkillFrame f) => _resolve().PublishMobileJiKang(f);
    internal bool ResumeMobileJiKang(long id) => _resolve().ResumeMobileJiKang(id);
    internal global::CardGame.Core.PromptChoice SelectMobileJiKangAi(global::CardGame.Core.PendingDecision decision, global::CardGame.Core.ProgramSkillFrame f) => _resolve().SelectMobileJiKangAi(decision, f);
    internal void StepMobileJiKang(global::CardGame.Core.ProgramSkillFrame f, global::CardGame.Core.MobileJiKangReceipt r) => _resolve().StepMobileJiKang(f, r);
}
