#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterDuanWeiOlRuntime
{
    // Only the two Taohuai bindings need this relation. Normal draw, discard and
    // public conversion-state instructions retain their shared owning frames.
    [ProgramTriggerLegality(ProgramTriggerLegalityPhase.BeforeCondition, 1220)]
    internal bool AllowsDuanWeiTaohuai(ProgramTriggerLegalityContext gate)
    {
        if (gate.SkillId != "ol:taohuai")
            return true;
        if (gate.Context is not { Window: SkillProgramTriggerWindow.CardUseCommitted, CardUse: { } cardUse } || gate.Trigger.Id is not ("extreme-hand-use-draw" or "non-extreme-hand-use-discard"))
            return false;
        var action = _resolutionStackOperations._resolutionStack.OfType<ProgramCardTriggerWindowFrame>().LastOrDefault(window => window.Id == gate.Context.ParentFrameId && window.Action.ActionId == cardUse.CardActionId)?.Action;
        if (action is null || action.ActorSeat != gate.OwnerSeat || action.ProviderSeat != gate.OwnerSeat || action.PhysicalCards.Count == 0 || action.PhysicalCards.Any(c => c.From != CardLocation.Hand(gate.OwnerSeat)) || action.Type == CardActionType.Response && !_coreAlternativeViewAsCostsOperations.IsProgramResponseCardUse(gate.Owner, action.EffectiveKind))
            return false;
        var history = _coreAlternatingBenefitsOperations.CompleteProgramEventHistory().ToArray();
        var nativeUse = action.Type == CardActionType.Use ? _resolutionStackOperations._resolutionStack.OfType<CardUseFrame>().SingleOrDefault(use => use.Action?.ActionId == action.ActionId) : null;
        // Native Slash commitment precedes the later finalized-target Accepted
        // event. Its owning Use and declaration are already authoritative here.
        var boundaryIndex = nativeUse is not null ? Array.FindIndex(history, e => e is CardUseDeclaredEvent declared && declared.ResolutionId == nativeUse.Id) : Array.FindIndex(history, e => e is CardActionAcceptedEvent accepted && accepted.Action.ActionId == action.ActionId);
        if (boundaryIndex < 0)
            return false;
        var paymentIndexes = action.PhysicalCards.Select(cost =>
        {
            bool IsPayment(IGameEvent fact) => fact is CardMovedEvent moved && moved.CardId == cost.CardId && moved.From == cost.From;
            var afterDeclaration = nativeUse is not null ? Array.FindIndex(history, boundaryIndex, IsPayment) : -1;
            // A truthful accepted declaration may have paid into Processing
            // before its native Use was created. Response Uses are also paid
            // before their Accepted event. Keep that exact earlier source.
            return afterDeclaration >= 0 ? afterDeclaration : Array.FindLastIndex(history, boundaryIndex, IsPayment);
        }).ToArray();
        if (paymentIndexes.Any(i => i < 0))
            return false;
        var paymentStart = paymentIndexes.Min();
        // Undo the authoritative movement facts to the instant immediately before
        // this Use paid its first material. Earlier target/use observers may have
        // changed the hand before this committed window; the live hand is too late.
        // The reconstructed private set stays inside this trusted query.
        var originalHand = _corePhysicalOwnedCardQueriesOperations.GetHand(gate.Owner).Select(c => c.Id).ToHashSet();
        for (var i = history.Length - 1; i >= paymentStart; i--)
        {
            if (history[i] is not CardMovedEvent moved)
                continue;
            if (moved.To == CardLocation.Hand(gate.OwnerSeat))
                originalHand.Remove(moved.CardId);
            if (moved.From == CardLocation.Hand(gate.OwnerSeat))
                originalHand.Add(moved.CardId);
        }

        if (action.PhysicalCards.Any(cost => !originalHand.Contains(cost.CardId)))
            return false;
        // Read the mode at the issued action, rather than a later draw/other
        // binding changing it while candidates from this same action are pending.
        var yin = history.Take(boundaryIndex).OfType<ProgramBooleanStateChangedEvent>().LastOrDefault(e => e.OwnerSeat == gate.OwnerSeat && e.SkillId == gate.SkillId && e.SkillInstanceId == gate.Candidate.SkillInstanceId && e.StateId == "yin")?.Value ?? false;
        var rank = action.EffectiveRank ?? (action.PhysicalCards.Count == 1 ? _coreAlcoholIdentityPoliciesOperations.EffectiveOwnedCardRank(gate.Owner, _coreCardAttackStateOperations.GetAttackCard(action.PhysicalCards[0].CardId)) : 0);
        var extreme = rank > 0 && originalHand.All(id => yin ? _coreAlcoholIdentityPoliciesOperations.EffectiveOwnedCardRank(gate.Owner, _coreCardAttackStateOperations.GetAttackCard(id)) >= rank : _coreAlcoholIdentityPoliciesOperations.EffectiveOwnedCardRank(gate.Owner, _coreCardAttackStateOperations.GetAttackCard(id)) <= rank);
        return gate.Trigger.Id == "extreme-hand-use-draw" ? extreme : !extreme && _corePhysicalOwnedCardQueriesOperations.GetHand(gate.Owner).Count + _corePhysicalOwnedCardQueriesOperations.GetEquipment(gate.Owner).Count > 0;
    }
}
