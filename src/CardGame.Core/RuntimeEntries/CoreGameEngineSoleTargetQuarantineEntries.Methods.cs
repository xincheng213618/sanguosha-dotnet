#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineSoleTargetQuarantineEntries
{
    internal bool ContinueSoleTargetQuarantineReturns(DeferredTurnEndFrame parent) => _resolve().ContinueSoleTargetQuarantineReturns(parent);
    internal bool HasExactSoleTargetQuarantine(CardUseFrame use) => _resolve().HasExactSoleTargetQuarantine(use);
    internal bool IsGameEndedSoleTargetMaterial(CardUseFrame use, int cardId) => _resolve().IsGameEndedSoleTargetMaterial(use, cardId);
    internal bool IsSoleTargetQuarantinedMaterial(long id, int cardId) => _resolve().IsSoleTargetQuarantinedMaterial(id, cardId);
    internal bool IsSoleTargetReplacementFact(CardUseFrame use, SoleTargetReplacedEvent fact, IReadOnlyList<int> before) => _resolve().IsSoleTargetReplacementFact(use, fact, before);
    internal bool IsValidSoleTargetProtectionPaidFact(SoleTargetProtectionReceipt r) => _resolve().IsValidSoleTargetProtectionPaidFact(r);
    internal bool MatchesSoleTargetProtectionOriginalTargets(long id, IReadOnlyList<int> originalTargets) => _resolve().MatchesSoleTargetProtectionOriginalTargets(id, originalTargets);
    internal bool MatchesSoleTargetQuarantineReturn(DeferredTurnEndFrame parent, CardsMovedTriggerWindowFrame child) => _resolve().MatchesSoleTargetQuarantineReturn(parent, child);
    internal SoleTargetQuarantineReceipt[] OutstandingSoleTargetQuarantines(int? turn = default, int? owner = default) => _resolve().OutstandingSoleTargetQuarantines(turn, owner);
    internal void RefreshSoleProtectionObservers(CardUseFrame use) => _resolve().RefreshSoleProtectionObservers(use);
    internal void ReturnSoleTargetQuarantineMovement(CardsMovedTriggerWindowFrame child) => _resolve().ReturnSoleTargetQuarantineMovement(child);
    internal void SettleGameEndedSoleTargetQuarantines() => _resolve().SettleGameEndedSoleTargetQuarantines();
    internal void SettleSoleTargetQuarantine(SoleTargetQuarantineReceipt r, bool gameEnded) => _resolve().SettleSoleTargetQuarantine(r, gameEnded);
    internal bool TryCompleteSoleTargetQuarantinedUse(long id) => _resolve().TryCompleteSoleTargetQuarantinedUse(id);
}
