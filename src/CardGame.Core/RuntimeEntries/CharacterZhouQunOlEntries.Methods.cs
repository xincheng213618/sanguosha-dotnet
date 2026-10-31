#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CharacterZhouQunOlEntries
{
    internal bool AdvanceZhouQunChildren() => _resolve().AdvanceZhouQunChildren();
    internal void AnswerZhouQun(global::CardGame.Core.PromptChoice choice) => _resolve().AnswerZhouQun(choice);
    internal void ApplyNingwuJudgment(global::CardGame.Core.ProgramSkillFrame f) => _resolve().ApplyNingwuJudgment(f);
    internal global::CardGame.Core.SkillProgramStepOutcome BeginZhouQun(global::CardGame.Core.ProgramSkillFrame supplied, global::CardGame.Core.SkillProgramEffectOp op) => _resolve().BeginZhouQun(supplied, op);
    internal void BeginZhouQunMovement(ref global::CardGame.Core.ProgramSkillFrame f, global::CardGame.Core.ZhouQunStage stage) => _resolve().BeginZhouQunMovement(ref f, stage);
    internal bool CanBeginNingwuJudgment(global::CardGame.Core.ProgramSkillFrame f, int target, string reason, string bind, global::CardGame.Core.SkillProgramCardSetVisibility visibility) => _resolve().CanBeginNingwuJudgment(f, target, reason, bind, visibility);
    internal bool CanOfferZhouQun(global::CardGame.Core.ProgramTriggerCandidate candidate, global::CardGame.Core.SkillProgramTrigger trigger, global::CardGame.Core.ProgramSkillWindowContext context) => _resolve().CanOfferZhouQun(candidate, trigger, context);
    internal global::CardGame.Core.PromptChoice ChooseZhouQun(global::CardGame.Core.PendingDecision p, global::CardGame.Core.ProgramSkillFrame f) => _resolve().ChooseZhouQun(p, f);
    internal void CleanupAndFinishZhouQun(global::CardGame.Core.ProgramSkillFrame f) => _resolve().CleanupAndFinishZhouQun(f);
    internal void CompleteZhouQunMovement(long id) => _resolve().CompleteZhouQunMovement(id);
    internal bool ContinuePaidZhouQun(global::CardGame.Core.ProgramSkillFrame f) => _resolve().ContinuePaidZhouQun(f);
    internal void ContinueZhouQun(long id) => _resolve().ContinueZhouQun(id);
    internal bool DrainZhouQun(global::CardGame.Core.ProgramSkillFrame f) => _resolve().DrainZhouQun(f);
    internal void ExpireTianhouWeather(global::CardGame.Core.ProgramSkillFrame f) => _resolve().ExpireTianhouWeather(f);
    internal void FinishZhouQun(global::CardGame.Core.ProgramSkillFrame f) => _resolve().FinishZhouQun(f);
    internal void GrantTianhouWeather(global::CardGame.Core.ProgramSkillFrame f, int recipient) => _resolve().GrantTianhouWeather(f, recipient);
    internal bool HasZhouQunBeforeDamageObserver(long id) => _resolve().HasZhouQunBeforeDamageObserver(id);
    internal bool HasZhouQunDamageObserver(long id) => _resolve().HasZhouQunDamageObserver(id);
    internal bool IsNingwuJudgment(global::CardGame.Core.JudgmentFrame j, global::CardGame.Core.ProgramSkillFrame f) => _resolve().IsNingwuJudgment(j, f);
    internal bool IsNingwuUseIneffective(global::CardGame.Core.CardUseFrame use) => _resolve().IsNingwuUseIneffective(use);
    internal bool IsZhouQunMovement(global::CardGame.Core.ProgramSkillFrame f, global::CardGame.Core.SkillProgramEffect? effect, global::CardGame.Core.ProgramMovementContinuation p) => _resolve().IsZhouQunMovement(f, effect, p);
    internal bool IsZhouQunRecoveryParent(global::CardGame.Core.ProgramSkillFrame f, global::CardGame.Core.RecoveryReplacementFrame recovery) => _resolve().IsZhouQunRecoveryParent(f, recovery);
    internal global::CardGame.Core.CardUseFrame? NingwuEligibleUse(int holder, global::CardGame.Core.ProgramSkillWindowContext context) => _resolve().NingwuEligibleUse(holder, context);
    internal void PayTianhouExchange(global::CardGame.Core.ProgramSkillFrame f) => _resolve().PayTianhouExchange(f);
    internal global::System.Collections.Generic.IEnumerable<global::CardGame.Core.CardSnapshot> ProjectZhouQunPublicShownCards() => _resolve().ProjectZhouQunPublicShownCards();
    internal void PublishZhouQun(global::CardGame.Core.ProgramSkillFrame f) => _resolve().PublishZhouQun(f);
    internal void RecordZhouQunHpPayment(global::CardGame.Core.ProgramSkillHpLostEvent e) => _resolve().RecordZhouQunHpPayment(e);
    internal void RecordZhouQunMoves(global::CardGame.Core.ProgramSkillFrame f, long batch, global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.CardMovementRecord> records) => _resolve().RecordZhouQunMoves(f, batch, records);
    internal bool ResumeZhouQun(long id) => _resolve().ResumeZhouQun(id);
    internal void ReturnChenshuoPublicHand(global::CardGame.Core.ProgramSkillFrame f) => _resolve().ReturnChenshuoPublicHand(f);
    internal bool ReturnZhouQunMovement(global::CardGame.Core.ProgramSkillFrame f) => _resolve().ReturnZhouQunMovement(f);
    internal void RevealNextChenshuo(global::CardGame.Core.ProgramSkillFrame f) => _resolve().RevealNextChenshuo(f);
    internal bool TryReturnNingwuJudgment(global::CardGame.Core.JudgmentFrame j) => _resolve().TryReturnNingwuJudgment(j);
    internal bool TryReturnZhouQunPaidPreparation(global::CardGame.Core.ProgramLifecycleTriggerWindowFrame frame) => _resolve().TryReturnZhouQunPaidPreparation(frame);
    internal global::CardGame.Core.PromptChoice ZhouQunChoice(global::CardGame.Core.ProgramSkillFrame f, string branch, string label, int? card = default, int? target = default) => _resolve().ZhouQunChoice(f, branch, label, card, target);
    internal global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.PromptChoice> ZhouQunChoices(global::CardGame.Core.ProgramSkillFrame f) => _resolve().ZhouQunChoices(f);
    internal int ZhouQunChooser(global::CardGame.Core.ProgramSkillFrame f) => _resolve().ZhouQunChooser(f);
    internal bool ZhouQunFirstChild(global::CardGame.Core.ProgramSkillFrame f, global::CardGame.Core.ResolutionFrame child) => _resolve().ZhouQunFirstChild(f, child);
    internal bool ZhouQunMovementMatches(global::CardGame.Core.ProgramSkillFrame f, global::CardGame.Core.CardMovementRecord m) => _resolve().ZhouQunMovementMatches(f, m);
    internal int[] ZhouQunNeighbors(int seat) => _resolve().ZhouQunNeighbors(seat);
    internal global::CardGame.Core.ProgramSkillFrame? ZhouQunObserverRoot() => _resolve().ZhouQunObserverRoot();
    internal bool ZhouQunParent(global::CardGame.Core.ProgramTriggerCandidate c, global::CardGame.Core.ProgramSkillWindowContext context) => _resolve().ZhouQunParent(c, context);
    internal global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.CardSnapshot> ZhouQunPrivatelyViewedTopCards(int viewerSeat) => _resolve().ZhouQunPrivatelyViewedTopCards(viewerSeat);
    internal global::System.Collections.Generic.IEnumerable<int> ZhouQunPublicShownCardIds() => _resolve().ZhouQunPublicShownCardIds();
    internal bool HasZhouQunCapability { get => _resolve().HasZhouQunCapability; }
}
