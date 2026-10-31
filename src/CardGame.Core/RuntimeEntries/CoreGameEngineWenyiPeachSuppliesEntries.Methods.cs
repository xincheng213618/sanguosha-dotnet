#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineWenyiPeachSuppliesEntries
{
    internal void ExecuteWenyiDrawFundedPeach(DrawFundedDistinctBasicFrame paid) => _resolve().ExecuteWenyiDrawFundedPeach(paid);
    internal void ExecuteWenyiRequestedDeckPeach(Card card, IReadOnlyList<int> targets) => _resolve().ExecuteWenyiRequestedDeckPeach(card, targets);
    internal void ExecuteWenyiWeijiePeach(WeijiePaidOrigin paid) => _resolve().ExecuteWenyiWeijiePeach(paid);
    internal bool IsReservedWenyiPeachSupply(ProgramSkillFrame f, int actor, int target, CardConversionSource source) => _resolve().IsReservedWenyiPeachSupply(f, actor, target, source);
    internal bool IsWenyiPeachSupplyQuery(CharacterState actor, int target) => _resolve().IsWenyiPeachSupplyQuery(actor, target);
    internal void RepublishWenyiAfterWeijie(WeijieReceipt receipt) => _resolve().RepublishWenyiAfterWeijie(receipt);
    internal void ReserveWenyiDrawFundedPeach(PendingDecision original, GameEngine.DrawFundedBasicNeed need, CardConversionSource source, CardKind kind) => _resolve().ReserveWenyiDrawFundedPeach(original, need, source, kind);
    internal void ResolveWenyiPeachSupply(PromptChoice choice) => _resolve().ResolveWenyiPeachSupply(choice);
    internal bool ResumeWenyiEquipmentSlotPeach(EquipmentSlotNamedUseDraft draft) => _resolve().ResumeWenyiEquipmentSlotPeach(draft);
    internal bool TryCancelWenyiPeachSupply(long? parentId) => _resolve().TryCancelWenyiPeachSupply(parentId);
    internal bool TryGetWenyiWeijiePeachNeed(ProgramSkillFrame f, int provider, scoped out int target) => _resolve().TryGetWenyiWeijiePeachNeed(f, provider, out target);
    internal bool TryResolveWenyiPeachSupply(PromptChoice choice) => _resolve().TryResolveWenyiPeachSupply(choice);
    internal bool WenyiDrawFundedNeedStillLegal(DrawFundedDistinctBasicFrame paid) => _resolve().WenyiDrawFundedNeedStillLegal(paid);
    internal bool WenyiDrawFundedParentMatches(ProgramSkillFrame f, DrawFundedDistinctBasicPayment p) => _resolve().WenyiDrawFundedParentMatches(f, p);
    internal bool WenyiEquipmentSlotPaymentRidesOn(ProgramSkillFrame f, ResolutionFrame child) => _resolve().WenyiEquipmentSlotPaymentRidesOn(f, child);
    internal IReadOnlyList<PromptChoice> WenyiPeachSupplyChoices(ProgramSkillFrame f) => _resolve().WenyiPeachSupplyChoices(f);
    internal bool WenyiPeachUseLegal(int actorSeat, int targetSeat, IReadOnlyList<Card> materials, CardConversionSource? source = default) => _resolve().WenyiPeachUseLegal(actorSeat, targetSeat, materials, source);
    internal int[] WenyiRequestedDeckPeachTargets(RequestedDeckBasicFrame view, Card card) => _resolve().WenyiRequestedDeckPeachTargets(view, card);
    internal bool WenyiWeijiePeachLegal(WeijieNeed need, int target) => _resolve().WenyiWeijiePeachLegal(need, target);
}
