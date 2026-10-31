#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineWushuangJinzhiEntries
{
    internal CardActionContext CaptureWushuangJinzhiAppearance(CardActionContext action) => _resolve().CaptureWushuangJinzhiAppearance(action);
    internal void ContinueWushuangJinzhi(ProgramSkillFrame f) => _resolve().ContinueWushuangJinzhi(f);
    internal bool HasWushuangJinzhiPaymentAction(CardActionContext action, long paymentFrameId) => _resolve().HasWushuangJinzhiPaymentAction(action, paymentFrameId);
    internal WushuangJinzhiMaterialSelection? HumanWushuangJinzhiMaterials() => _resolve().HumanWushuangJinzhiMaterials();
    internal bool IsWushuangJinzhiPaidMaterial(CardActionContext? action, int card) => _resolve().IsWushuangJinzhiPaidMaterial(action, card);
    internal bool IsWushuangJinzhiSelection(ProgramMultiCardViewAsSelection selection) => _resolve().IsWushuangJinzhiSelection(selection);
    internal bool IsWushuangJinzhiSource(CardConversionSource source) => _resolve().IsWushuangJinzhiSource(source);
    internal bool TryGetWushuangJinzhiResponseMovements(CardActionContext action, int card, scoped out IReadOnlyList<CardMovementRecord> movements) => _resolve().TryGetWushuangJinzhiResponseMovements(action, card, out movements);
    internal bool TryResolveWushuangJinzhiDyingAlcohol(CharacterState owner, ProgramMultiCardViewAsSelection selection) => _resolve().TryResolveWushuangJinzhiDyingAlcohol(owner, selection);
    internal bool WushuangJinzhiConversionForbidden(ProgramViewAsLegalityQuery query) => _resolve().WushuangJinzhiConversionForbidden(query);
    internal CardMovementRecord? WushuangJinzhiDestroyedPayment(CardActionContext action, CardActionCost cost, long paymentFrameId, int movementThrough) => _resolve().WushuangJinzhiDestroyedPayment(action, cost, paymentFrameId, movementThrough);
    internal CardLocation? WushuangJinzhiDiscardSource(CardMovementRecord movement) => _resolve().WushuangJinzhiDiscardSource(movement);
    internal IEnumerable<Card> WushuangJinzhiDiscardableCards(CharacterState owner) => _resolve().WushuangJinzhiDiscardableCards(owner);
    internal int WushuangJinzhiNextCount(int owner) => _resolve().WushuangJinzhiNextCount(owner);
    internal IReadOnlyList<ProgramMultiCardViewAsSelection> WushuangJinzhiSelections(CharacterState owner, CardKind kind, bool response) => _resolve().WushuangJinzhiSelections(owner, kind, response);
}
