#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineWenyiPeachSuppliesEntries
{
    internal void ExecuteWenyiDrawFundedPeach(global::CardGame.Core.DrawFundedDistinctBasicFrame paid) => _resolve().ExecuteWenyiDrawFundedPeach(paid);
    internal void ExecuteWenyiRequestedDeckPeach(global::CardGame.Core.Card card, global::System.Collections.Generic.IReadOnlyList<int> targets) => _resolve().ExecuteWenyiRequestedDeckPeach(card, targets);
    internal void ExecuteWenyiWeijiePeach(global::CardGame.Core.WeijiePaidOrigin paid) => _resolve().ExecuteWenyiWeijiePeach(paid);
    internal bool IsReservedWenyiPeachSupply(global::CardGame.Core.ProgramSkillFrame f, int actor, int target, global::CardGame.Core.CardConversionSource source) => _resolve().IsReservedWenyiPeachSupply(f, actor, target, source);
    internal bool IsWenyiPeachSupplyQuery(global::CardGame.Core.CharacterState actor, int target) => _resolve().IsWenyiPeachSupplyQuery(actor, target);
    internal void RepublishWenyiAfterWeijie(global::CardGame.Core.WeijieReceipt receipt) => _resolve().RepublishWenyiAfterWeijie(receipt);
    internal void ReserveWenyiDrawFundedPeach(global::CardGame.Core.PendingDecision original, global::CardGame.Core.GameEngine.DrawFundedBasicNeed need, global::CardGame.Core.CardConversionSource source, global::CardGame.Core.CardKind kind) => _resolve().ReserveWenyiDrawFundedPeach(original, need, source, kind);
    internal void ResolveWenyiPeachSupply(global::CardGame.Core.PromptChoice choice) => _resolve().ResolveWenyiPeachSupply(choice);
    internal bool ResumeWenyiEquipmentSlotPeach(global::CardGame.Core.EquipmentSlotNamedUseDraft draft) => _resolve().ResumeWenyiEquipmentSlotPeach(draft);
    internal bool TryCancelWenyiPeachSupply(long? parentId) => _resolve().TryCancelWenyiPeachSupply(parentId);
    internal bool TryGetWenyiWeijiePeachNeed(global::CardGame.Core.ProgramSkillFrame f, int provider, scoped out int target) => _resolve().TryGetWenyiWeijiePeachNeed(f, provider, out target);
    internal bool TryResolveWenyiPeachSupply(global::CardGame.Core.PromptChoice choice) => _resolve().TryResolveWenyiPeachSupply(choice);
    internal bool WenyiDrawFundedNeedStillLegal(global::CardGame.Core.DrawFundedDistinctBasicFrame paid) => _resolve().WenyiDrawFundedNeedStillLegal(paid);
    internal bool WenyiDrawFundedParentMatches(global::CardGame.Core.ProgramSkillFrame f, global::CardGame.Core.DrawFundedDistinctBasicPayment p) => _resolve().WenyiDrawFundedParentMatches(f, p);
    internal bool WenyiEquipmentSlotPaymentRidesOn(global::CardGame.Core.ProgramSkillFrame f, global::CardGame.Core.ResolutionFrame child) => _resolve().WenyiEquipmentSlotPaymentRidesOn(f, child);
    internal global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.PromptChoice> WenyiPeachSupplyChoices(global::CardGame.Core.ProgramSkillFrame f) => _resolve().WenyiPeachSupplyChoices(f);
    internal bool WenyiPeachUseLegal(int actorSeat, int targetSeat, global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.Card> materials, global::CardGame.Core.CardConversionSource? source = default) => _resolve().WenyiPeachUseLegal(actorSeat, targetSeat, materials, source);
    internal int[] WenyiRequestedDeckPeachTargets(global::CardGame.Core.RequestedDeckBasicFrame view, global::CardGame.Core.Card card) => _resolve().WenyiRequestedDeckPeachTargets(view, card);
    internal bool WenyiWeijiePeachLegal(global::CardGame.Core.WeijieNeed need, int target) => _resolve().WenyiWeijiePeachLegal(need, target);
}
