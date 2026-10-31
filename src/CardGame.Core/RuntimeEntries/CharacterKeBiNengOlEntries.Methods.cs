#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CharacterKeBiNengOlEntries
{
    internal bool AdvanceKeBiNengSubtree() => _resolve().AdvanceKeBiNengSubtree();
    internal void AnswerKeBiNeng(global::CardGame.Core.PromptChoice choice) => _resolve().AnswerKeBiNeng(choice);
    internal void AssertKeBiNeng(global::CardGame.Core.ProgramSkillFrame f) => _resolve().AssertKeBiNeng(f);
    internal void AssertKeBiNengNativeCompletion(global::CardGame.Core.CardUseFrame use) => _resolve().AssertKeBiNengNativeCompletion(use);
    internal global::CardGame.Core.SkillProgramStepOutcome BeginKeBiNeng(global::CardGame.Core.ProgramSkillFrame input) => _resolve().BeginKeBiNeng(input);
    internal void BeginKeBiNengPingduanBasicUse(global::CardGame.Core.ProgramSkillFrame frame, global::CardGame.Core.LegalAction selected) => _resolve().BeginKeBiNengPingduanBasicUse(frame, selected);
    internal void BindKeBiNengPingduanNativeUse(global::CardGame.Core.CardUseFrame use) => _resolve().BindKeBiNengPingduanNativeUse(use);
    internal global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.LegalAction> BuildKeBiNengPingduanBasicActions(global::CardGame.Core.ProgramSkillFrame frame) => _resolve().BuildKeBiNengPingduanBasicActions(frame);
    internal global::CardGame.Core.PromptChoice ChooseKeBiNengAi(global::CardGame.Core.PendingDecision p, global::CardGame.Core.ProgramSkillFrame f) => _resolve().ChooseKeBiNengAi(p, f);
    internal void ContinueKeBiNeng(long id) => _resolve().ContinueKeBiNeng(id);
    internal bool ContinuePaidKeBiNeng(global::CardGame.Core.ProgramSkillFrame f) => _resolve().ContinuePaidKeBiNeng(f);
    internal bool DrainKeBiNeng(long id) => _resolve().DrainKeBiNeng(id);
    internal void ExpireKeBiNengPingduanAlcohol(int actualTurn, int turnOwner) => _resolve().ExpireKeBiNengPingduanAlcohol(actualTurn, turnOwner);
    internal void FinishKeBiNeng(global::CardGame.Core.ProgramSkillFrame f, string reason) => _resolve().FinishKeBiNeng(f, reason);
    internal bool HasIssuedKeBiNengPingduanUse(long id, int actor) => _resolve().HasIssuedKeBiNengPingduanUse(id, actor);
    internal bool HasKeBiNengBeforeDamageObserver(long id) => _resolve().HasKeBiNengBeforeDamageObserver(id);
    internal bool HasKeBiNengDamageObserver(long id) => _resolve().HasKeBiNengDamageObserver(id);
    internal bool IsKeBiNengDying() => _resolve().IsKeBiNengDying();
    internal bool IsKeBiNengPingduanBasicQuery(global::CardGame.Core.CharacterState actor) => _resolve().IsKeBiNengPingduanBasicQuery(actor);
    internal bool IsKeBiNengPingduanPaymentFrame(global::CardGame.Core.ChainedStateBasicFrame paid) => _resolve().IsKeBiNengPingduanPaymentFrame(paid);
    internal bool IsKeBiNengPingduanPaymentFrame2(global::CardGame.Core.DrawFundedDistinctBasicFrame paid) => _resolve().IsKeBiNengPingduanPaymentFrame(paid);
    internal bool IsKeBiNengPingduanPaymentParent(long paymentId, long? parentId, long? requestId, int cursor, global::CardGame.Core.CardConversionSource source, global::CardGame.Core.CardKind kind, global::System.Collections.Generic.IReadOnlyList<int> targets) => _resolve().IsKeBiNengPingduanPaymentParent(paymentId, parentId, requestId, cursor, source, kind, targets);
    internal bool IsKeBiNengRecoveryParent(global::CardGame.Core.ProgramSkillFrame f, global::CardGame.Core.RecoveryReplacementFrame recovery) => _resolve().IsKeBiNengRecoveryParent(f, recovery);
    internal bool KeBiNengCardPaymentStage(global::CardGame.Core.KeBiNengPingduanReceipt r) => _resolve().KeBiNengCardPaymentStage(r);
    internal global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.PromptChoice> KeBiNengChoices(global::CardGame.Core.ProgramSkillFrame f) => _resolve().KeBiNengChoices(f);
    internal global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.KeBiNengPingduanCard> KeBiNengEquipmentCards(global::CardGame.Core.ProgramSkillFrame f) => _resolve().KeBiNengEquipmentCards(f);
    internal bool KeBiNengFirstChild(global::CardGame.Core.ProgramSkillFrame f, global::CardGame.Core.ResolutionFrame child) => _resolve().KeBiNengFirstChild(f, child);
    internal global::CardGame.Core.ProgramSkillFrame? KeBiNengObserverRoot() => _resolve().KeBiNengObserverRoot();
    internal global::CardGame.Core.CardMoveReason KeBiNengPaymentReason(global::CardGame.Core.KeBiNengPingduanReceipt r) => _resolve().KeBiNengPaymentReason(r);
    internal bool KeBiNengPaymentRidesOn(global::CardGame.Core.ProgramSkillFrame f, global::CardGame.Core.ResolutionFrame child) => _resolve().KeBiNengPaymentRidesOn(f, child);
    internal global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.KeBiNengPingduanCard> KeBiNengRecastCards(global::CardGame.Core.ProgramSkillFrame f) => _resolve().KeBiNengRecastCards(f);
    internal bool MatchesKeBiNengPingduanDeclaration(global::CardGame.Core.ProgramSkillFrame root, global::CardGame.Core.CardDeclarationFrame d) => _resolve().MatchesKeBiNengPingduanDeclaration(root, d);
    internal void ObserveKeBiNengAlcoholExpiry(global::CardGame.Core.IGameEvent payload) => _resolve().ObserveKeBiNengAlcoholExpiry(payload);
    internal void PayKeBiNengCard(global::CardGame.Core.ProgramSkillFrame f, global::CardGame.Core.KeBiNengPingduanCard card, bool recast) => _resolve().PayKeBiNengCard(f, card, recast);
    internal void PayKeBiNengDraw(global::CardGame.Core.ProgramSkillFrame f, int item, bool recast) => _resolve().PayKeBiNengDraw(f, item, recast);
    internal void PrepareKeBiNengBasic(global::CardGame.Core.ProgramSkillFrame f) => _resolve().PrepareKeBiNengBasic(f);
    internal void PublishKeBiNeng(global::CardGame.Core.ProgramSkillFrame f) => _resolve().PublishKeBiNeng(f);
    internal bool ResumeKeBiNeng(long id) => _resolve().ResumeKeBiNeng(id);
    internal bool ReturnKeBiNengNativeAttack() => _resolve().ReturnKeBiNengNativeAttack();
    internal bool ReturnKeBiNengNativeUse(global::CardGame.Core.CardUseFrame use) => _resolve().ReturnKeBiNengNativeUse(use);
    internal bool TryCancelKeBiNengPingduanBasicPayment(long programId) => _resolve().TryCancelKeBiNengPingduanBasicPayment(programId);
    internal bool TryReturnKeBiNengPingduanDeclaration(global::CardGame.Core.CardDeclarationFrame declaration, bool success) => _resolve().TryReturnKeBiNengPingduanDeclaration(declaration, success);
    internal bool ValidKeBiNeng(global::CardGame.Core.ProgramSkillFrame f) => _resolve().ValidKeBiNeng(f);
    internal bool HasKeBiNengCapability { get => _resolve().HasKeBiNengCapability; }
}
