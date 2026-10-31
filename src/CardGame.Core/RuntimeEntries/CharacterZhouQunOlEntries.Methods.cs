#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CharacterZhouQunOlEntries
{
    internal bool AdvanceZhouQunChildren() => _resolve().AdvanceZhouQunChildren();
    internal void AnswerZhouQun(PromptChoice choice) => _resolve().AnswerZhouQun(choice);
    internal void ApplyNingwuJudgment(ProgramSkillFrame f) => _resolve().ApplyNingwuJudgment(f);
    internal SkillProgramStepOutcome BeginZhouQun(ProgramSkillFrame supplied, SkillProgramEffectOp op) => _resolve().BeginZhouQun(supplied, op);
    internal void BeginZhouQunMovement(ref ProgramSkillFrame f, ZhouQunStage stage) => _resolve().BeginZhouQunMovement(ref f, stage);
    internal bool CanBeginNingwuJudgment(ProgramSkillFrame f, int target, string reason, string bind, SkillProgramCardSetVisibility visibility) => _resolve().CanBeginNingwuJudgment(f, target, reason, bind, visibility);
    internal bool CanOfferZhouQun(ProgramTriggerCandidate candidate, SkillProgramTrigger trigger, ProgramSkillWindowContext context) => _resolve().CanOfferZhouQun(candidate, trigger, context);
    internal PromptChoice ChooseZhouQun(PendingDecision p, ProgramSkillFrame f) => _resolve().ChooseZhouQun(p, f);
    internal void CleanupAndFinishZhouQun(ProgramSkillFrame f) => _resolve().CleanupAndFinishZhouQun(f);
    internal void CompleteZhouQunMovement(long id) => _resolve().CompleteZhouQunMovement(id);
    internal bool ContinuePaidZhouQun(ProgramSkillFrame f) => _resolve().ContinuePaidZhouQun(f);
    internal void ContinueZhouQun(long id) => _resolve().ContinueZhouQun(id);
    internal bool DrainZhouQun(ProgramSkillFrame f) => _resolve().DrainZhouQun(f);
    internal void ExpireTianhouWeather(ProgramSkillFrame f) => _resolve().ExpireTianhouWeather(f);
    internal void FinishZhouQun(ProgramSkillFrame f) => _resolve().FinishZhouQun(f);
    internal void GrantTianhouWeather(ProgramSkillFrame f, int recipient) => _resolve().GrantTianhouWeather(f, recipient);
    internal bool IsNingwuJudgment(JudgmentFrame j, ProgramSkillFrame f) => _resolve().IsNingwuJudgment(j, f);
    internal bool IsNingwuUseIneffective(CardUseFrame use) => _resolve().IsNingwuUseIneffective(use);
    internal bool IsZhouQunRecoveryParent(ProgramSkillFrame f, RecoveryReplacementFrame recovery) => _resolve().IsZhouQunRecoveryParent(f, recovery);
    internal CardUseFrame? NingwuEligibleUse(int holder, ProgramSkillWindowContext context) => _resolve().NingwuEligibleUse(holder, context);
    internal void PayTianhouExchange(ProgramSkillFrame f) => _resolve().PayTianhouExchange(f);
    internal IEnumerable<CardSnapshot> ProjectZhouQunPublicShownCards() => _resolve().ProjectZhouQunPublicShownCards();
    internal void PublishZhouQun(ProgramSkillFrame f) => _resolve().PublishZhouQun(f);
    internal void RecordZhouQunHpPayment(ProgramSkillHpLostEvent e) => _resolve().RecordZhouQunHpPayment(e);
    internal void RecordZhouQunMoves(ProgramSkillFrame f, long batch, IReadOnlyList<CardMovementRecord> records) => _resolve().RecordZhouQunMoves(f, batch, records);
    internal bool ResumeZhouQun(long id) => _resolve().ResumeZhouQun(id);
    internal void ReturnChenshuoPublicHand(ProgramSkillFrame f) => _resolve().ReturnChenshuoPublicHand(f);
    internal bool ReturnZhouQunMovement(ProgramSkillFrame f) => _resolve().ReturnZhouQunMovement(f);
    internal void RevealNextChenshuo(ProgramSkillFrame f) => _resolve().RevealNextChenshuo(f);
    internal bool TryReturnNingwuJudgment(JudgmentFrame j) => _resolve().TryReturnNingwuJudgment(j);
    internal bool TryReturnZhouQunPaidPreparation(ProgramLifecycleTriggerWindowFrame frame) => _resolve().TryReturnZhouQunPaidPreparation(frame);
    internal PromptChoice ZhouQunChoice(ProgramSkillFrame f, string branch, string label, int? card = default, int? target = default) => _resolve().ZhouQunChoice(f, branch, label, card, target);
    internal IReadOnlyList<PromptChoice> ZhouQunChoices(ProgramSkillFrame f) => _resolve().ZhouQunChoices(f);
    internal int ZhouQunChooser(ProgramSkillFrame f) => _resolve().ZhouQunChooser(f);
    internal bool ZhouQunFirstChild(ProgramSkillFrame f, ResolutionFrame child) => _resolve().ZhouQunFirstChild(f, child);
    internal bool ZhouQunMovementMatches(ProgramSkillFrame f, CardMovementRecord m) => _resolve().ZhouQunMovementMatches(f, m);
    internal int[] ZhouQunNeighbors(int seat) => _resolve().ZhouQunNeighbors(seat);
    internal ProgramSkillFrame? ZhouQunObserverRoot() => _resolve().ZhouQunObserverRoot();
    internal bool ZhouQunParent(ProgramTriggerCandidate c, ProgramSkillWindowContext context) => _resolve().ZhouQunParent(c, context);
    internal IReadOnlyList<CardSnapshot> ZhouQunPrivatelyViewedTopCards(int viewerSeat) => _resolve().ZhouQunPrivatelyViewedTopCards(viewerSeat);
    internal IEnumerable<int> ZhouQunPublicShownCardIds() => _resolve().ZhouQunPublicShownCardIds();
    internal bool HasZhouQunCapability { get => _resolve().HasZhouQunCapability; }
}
