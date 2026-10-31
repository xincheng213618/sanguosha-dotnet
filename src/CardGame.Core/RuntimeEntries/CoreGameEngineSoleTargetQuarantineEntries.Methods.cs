#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineSoleTargetQuarantineEntries
{
    internal bool ContinueSoleTargetQuarantineReturns(global::CardGame.Core.DeferredTurnEndFrame parent) => _resolve().ContinueSoleTargetQuarantineReturns(parent);
    internal bool HasExactSoleTargetQuarantine(global::CardGame.Core.CardUseFrame use) => _resolve().HasExactSoleTargetQuarantine(use);
    internal bool IsGameEndedSoleTargetMaterial(global::CardGame.Core.CardUseFrame use, int cardId) => _resolve().IsGameEndedSoleTargetMaterial(use, cardId);
    internal bool IsSoleTargetQuarantinedMaterial(long id, int cardId) => _resolve().IsSoleTargetQuarantinedMaterial(id, cardId);
    internal bool IsSoleTargetReplacementFact(global::CardGame.Core.CardUseFrame use, global::CardGame.Core.SoleTargetReplacedEvent fact, global::System.Collections.Generic.IReadOnlyList<int> before) => _resolve().IsSoleTargetReplacementFact(use, fact, before);
    internal bool IsValidSoleTargetProtectionPaidFact(global::CardGame.Core.SoleTargetProtectionReceipt r) => _resolve().IsValidSoleTargetProtectionPaidFact(r);
    internal bool MatchesSoleTargetProtectionOriginalTargets(long id, global::System.Collections.Generic.IReadOnlyList<int> originalTargets) => _resolve().MatchesSoleTargetProtectionOriginalTargets(id, originalTargets);
    internal bool MatchesSoleTargetQuarantineReturn(global::CardGame.Core.DeferredTurnEndFrame parent, global::CardGame.Core.CardsMovedTriggerWindowFrame child) => _resolve().MatchesSoleTargetQuarantineReturn(parent, child);
    internal global::CardGame.Core.SoleTargetQuarantineReceipt[] OutstandingSoleTargetQuarantines(int? turn = default, int? owner = default) => _resolve().OutstandingSoleTargetQuarantines(turn, owner);
    internal void RefreshSoleProtectionObservers(global::CardGame.Core.CardUseFrame use) => _resolve().RefreshSoleProtectionObservers(use);
    internal void ReturnSoleTargetQuarantineMovement(global::CardGame.Core.CardsMovedTriggerWindowFrame child) => _resolve().ReturnSoleTargetQuarantineMovement(child);
    internal void SettleGameEndedSoleTargetQuarantines() => _resolve().SettleGameEndedSoleTargetQuarantines();
    internal void SettleSoleTargetQuarantine(global::CardGame.Core.SoleTargetQuarantineReceipt r, bool gameEnded) => _resolve().SettleSoleTargetQuarantine(r, gameEnded);
    internal bool TryCompleteSoleTargetQuarantinedUse(long id) => _resolve().TryCompleteSoleTargetQuarantinedUse(id);
}
