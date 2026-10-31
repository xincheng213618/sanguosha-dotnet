#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineWushuangJinzhiEntries
{
    internal global::CardGame.Core.CardActionContext CaptureWushuangJinzhiAppearance(global::CardGame.Core.CardActionContext action) => _resolve().CaptureWushuangJinzhiAppearance(action);
    internal void ContinueWushuangJinzhi(global::CardGame.Core.ProgramSkillFrame f) => _resolve().ContinueWushuangJinzhi(f);
    internal bool HasWushuangJinzhiPaymentAction(global::CardGame.Core.CardActionContext action, long paymentFrameId) => _resolve().HasWushuangJinzhiPaymentAction(action, paymentFrameId);
    internal global::CardGame.Core.WushuangJinzhiMaterialSelection? HumanWushuangJinzhiMaterials() => _resolve().HumanWushuangJinzhiMaterials();
    internal bool IsWushuangJinzhiPaidMaterial(global::CardGame.Core.CardActionContext? action, int card) => _resolve().IsWushuangJinzhiPaidMaterial(action, card);
    internal bool IsWushuangJinzhiSelection(global::CardGame.Core.ProgramMultiCardViewAsSelection selection) => _resolve().IsWushuangJinzhiSelection(selection);
    internal bool IsWushuangJinzhiSource(global::CardGame.Core.CardConversionSource source) => _resolve().IsWushuangJinzhiSource(source);
    internal bool TryGetWushuangJinzhiResponseMovements(global::CardGame.Core.CardActionContext action, int card, scoped out global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.CardMovementRecord> movements) => _resolve().TryGetWushuangJinzhiResponseMovements(action, card, out movements);
    internal bool TryResolveWushuangJinzhiDyingAlcohol(global::CardGame.Core.CharacterState owner, global::CardGame.Core.ProgramMultiCardViewAsSelection selection) => _resolve().TryResolveWushuangJinzhiDyingAlcohol(owner, selection);
    internal bool WushuangJinzhiConversionForbidden(global::CardGame.Core.ProgramViewAsLegalityQuery query) => _resolve().WushuangJinzhiConversionForbidden(query);
    internal global::CardGame.Core.CardMovementRecord? WushuangJinzhiDestroyedPayment(global::CardGame.Core.CardActionContext action, global::CardGame.Core.CardActionCost cost, long paymentFrameId, int movementThrough) => _resolve().WushuangJinzhiDestroyedPayment(action, cost, paymentFrameId, movementThrough);
    internal global::CardGame.Core.CardLocation? WushuangJinzhiDiscardSource(global::CardGame.Core.CardMovementRecord movement) => _resolve().WushuangJinzhiDiscardSource(movement);
    internal global::System.Collections.Generic.IEnumerable<global::CardGame.Core.Card> WushuangJinzhiDiscardableCards(global::CardGame.Core.CharacterState owner) => _resolve().WushuangJinzhiDiscardableCards(owner);
    internal int WushuangJinzhiNextCount(int owner) => _resolve().WushuangJinzhiNextCount(owner);
    internal global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.ProgramMultiCardViewAsSelection> WushuangJinzhiSelections(global::CardGame.Core.CharacterState owner, global::CardGame.Core.CardKind kind, bool response) => _resolve().WushuangJinzhiSelections(owner, kind, response);
}
