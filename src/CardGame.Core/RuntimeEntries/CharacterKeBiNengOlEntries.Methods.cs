#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CharacterKeBiNengOlEntries
{
    internal bool AdvanceKeBiNengSubtree() => _resolve().AdvanceKeBiNengSubtree();
    internal void AnswerKeBiNeng(PromptChoice choice) => _resolve().AnswerKeBiNeng(choice);
    internal void AssertKeBiNengNativeCompletion(CardUseFrame use) => _resolve().AssertKeBiNengNativeCompletion(use);
    internal SkillProgramStepOutcome BeginKeBiNeng(ProgramSkillFrame input) => _resolve().BeginKeBiNeng(input);
    internal void BeginKeBiNengPingduanBasicUse(ProgramSkillFrame frame, LegalAction selected) => _resolve().BeginKeBiNengPingduanBasicUse(frame, selected);
    internal void BindKeBiNengPingduanNativeUse(CardUseFrame use) => _resolve().BindKeBiNengPingduanNativeUse(use);
    internal IReadOnlyList<LegalAction> BuildKeBiNengPingduanBasicActions(ProgramSkillFrame frame) => _resolve().BuildKeBiNengPingduanBasicActions(frame);
    internal PromptChoice ChooseKeBiNengAi(PendingDecision p, ProgramSkillFrame f) => _resolve().ChooseKeBiNengAi(p, f);
    internal void ContinueKeBiNeng(long id) => _resolve().ContinueKeBiNeng(id);
    internal bool ContinuePaidKeBiNeng(ProgramSkillFrame f) => _resolve().ContinuePaidKeBiNeng(f);
    internal bool DrainKeBiNeng(long id) => _resolve().DrainKeBiNeng(id);
    internal void ExpireKeBiNengPingduanAlcohol(int actualTurn, int turnOwner) => _resolve().ExpireKeBiNengPingduanAlcohol(actualTurn, turnOwner);
    internal void FinishKeBiNeng(ProgramSkillFrame f, string reason) => _resolve().FinishKeBiNeng(f, reason);
    internal bool HasIssuedKeBiNengPingduanUse(long id, int actor) => _resolve().HasIssuedKeBiNengPingduanUse(id, actor);
    internal bool IsKeBiNengPingduanBasicQuery(CharacterState actor) => _resolve().IsKeBiNengPingduanBasicQuery(actor);
    internal bool IsKeBiNengPingduanPaymentFrame(ChainedStateBasicFrame paid) => _resolve().IsKeBiNengPingduanPaymentFrame(paid);
    internal bool IsKeBiNengPingduanPaymentFrame2(DrawFundedDistinctBasicFrame paid) => _resolve().IsKeBiNengPingduanPaymentFrame(paid);
    internal bool IsKeBiNengPingduanPaymentParent(long paymentId, long? parentId, long? requestId, int cursor, CardConversionSource source, CardKind kind, IReadOnlyList<int> targets) => _resolve().IsKeBiNengPingduanPaymentParent(paymentId, parentId, requestId, cursor, source, kind, targets);
    internal bool IsKeBiNengRecoveryParent(ProgramSkillFrame f, RecoveryReplacementFrame recovery) => _resolve().IsKeBiNengRecoveryParent(f, recovery);
    internal bool KeBiNengCardPaymentStage(KeBiNengPingduanReceipt r) => _resolve().KeBiNengCardPaymentStage(r);
    internal IReadOnlyList<PromptChoice> KeBiNengChoices(ProgramSkillFrame f) => _resolve().KeBiNengChoices(f);
    internal IReadOnlyList<KeBiNengPingduanCard> KeBiNengEquipmentCards(ProgramSkillFrame f) => _resolve().KeBiNengEquipmentCards(f);
    internal bool KeBiNengFirstChild(ProgramSkillFrame f, ResolutionFrame child) => _resolve().KeBiNengFirstChild(f, child);
    internal ProgramSkillFrame? KeBiNengObserverRoot() => _resolve().KeBiNengObserverRoot();
    internal CardMoveReason KeBiNengPaymentReason(KeBiNengPingduanReceipt r) => _resolve().KeBiNengPaymentReason(r);
    internal bool KeBiNengPaymentRidesOn(ProgramSkillFrame f, ResolutionFrame child) => _resolve().KeBiNengPaymentRidesOn(f, child);
    internal IReadOnlyList<KeBiNengPingduanCard> KeBiNengRecastCards(ProgramSkillFrame f) => _resolve().KeBiNengRecastCards(f);
    internal bool MatchesKeBiNengPingduanDeclaration(ProgramSkillFrame root, CardDeclarationFrame d) => _resolve().MatchesKeBiNengPingduanDeclaration(root, d);
    internal void ObserveKeBiNengAlcoholExpiry(IGameEvent payload) => _resolve().ObserveKeBiNengAlcoholExpiry(payload);
    internal void PayKeBiNengCard(ProgramSkillFrame f, KeBiNengPingduanCard card, bool recast) => _resolve().PayKeBiNengCard(f, card, recast);
    internal void PayKeBiNengDraw(ProgramSkillFrame f, int item, bool recast) => _resolve().PayKeBiNengDraw(f, item, recast);
    internal void PrepareKeBiNengBasic(ProgramSkillFrame f) => _resolve().PrepareKeBiNengBasic(f);
    internal void PublishKeBiNeng(ProgramSkillFrame f) => _resolve().PublishKeBiNeng(f);
    internal bool ResumeKeBiNeng(long id) => _resolve().ResumeKeBiNeng(id);
    internal bool ReturnKeBiNengNativeAttack() => _resolve().ReturnKeBiNengNativeAttack();
    internal bool ReturnKeBiNengNativeUse(CardUseFrame use) => _resolve().ReturnKeBiNengNativeUse(use);
    internal bool TryCancelKeBiNengPingduanBasicPayment(long programId) => _resolve().TryCancelKeBiNengPingduanBasicPayment(programId);
    internal bool TryReturnKeBiNengPingduanDeclaration(CardDeclarationFrame declaration, bool success) => _resolve().TryReturnKeBiNengPingduanDeclaration(declaration, success);
    internal bool ValidKeBiNeng(ProgramSkillFrame f) => _resolve().ValidKeBiNeng(f);
    internal bool HasKeBiNengCapability { get => _resolve().HasKeBiNengCapability; }
}
