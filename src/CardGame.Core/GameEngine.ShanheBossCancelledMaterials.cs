namespace CardGame.Core;

public sealed partial class GameEngine
{
    private const string ShanheBossInterruptedProcessingReason = "campaign.shanhe.boss-interrupted-processing";

    private void CaptureShanheBossCancelledMaterials()
    {
        if (_options.ShanheBattle is not { StartingResourcesExpireOnUse: true, BossTransition: not null } ||
            ProgramEventHistory<ShanheBossStageStartedEvent>().Count != 1) return;
        // Capture every paid native Use, including a virtual program child's
        // physical materials, before any frame is retired. No cost is refunded.
        foreach (var entry in _cardMovements.GroupBy(m => m.CardId).Select(g => g.Last())
            .Where(m => m.To == CardLocation.Processing).ToArray())
        {
            if (!IsShanheStartingResource(entry.CardId) || ShanheStartingResourceWasAcquired(entry.CardId)) continue;
            bool Matches(CardActionContext action) => action.PhysicalCards.Any(c =>
                c.CardId == entry.CardId && c.CardKind == entry.CardKind && c.From == entry.From);
            var use = _resolutionStack.OfType<CardUseFrame>().LastOrDefault(f => f.Action is { Type: CardActionType.Use } a && Matches(a) &&
                ProgramEventHistory<ShanheStartingResourceUseEnteredEvent>().Any(e => e.CardId == entry.CardId &&
                    e.EntrySequence == entry.Sequence && e.UseFrameId == f.Id && e.ActionId == a.ActionId));
            if (use?.Action is { } action)
            {
                AdvanceEventRulesAndQueueFact(new ShanheBossMaterialCancelledEvent(entry.CardId, use.Id,
                    action.ActionId, entry.Sequence, CardActionType.Use));
                continue;
            }
            if (entry.Reason != CardMoveReasons.Respond && entry.Reason != CardMoveReasons.Nullification &&
                entry.Reason.Value != "conversion.declaration.pay") continue;
            var response = ProgramEventHistory<CardActionAcceptedEvent>().Select(e => e.Action)
                .LastOrDefault(a => a.Type == CardActionType.Response && Matches(a));
            ResolutionFrame? owner = response is null ? null : _resolutionStack.LastOrDefault(f => f switch
            {
                ProgramCardTriggerWindowFrame card => card.Action.ActionId == response.ActionId,
                CardUseFrame incoming => incoming.Action?.ActionId is { } id && response.ParentActionId == id,
                NullificationWindowFrame counter => response.ParentActionId == NullificationParentActionId(counter),
                ProgramSkillFrame program => IsResponseCompletionProgramDuelParent(program, ProgramCardContinuation.DuelSlash),
                _ => false
            });
            // A declared response must also retain its original claimed invoice.
            if (entry.Reason.Value == "conversion.declaration.pay" && !_resolutionStack.Any(f =>
                f.AcceptedDeclarationPayment is { Claimed: true } paid && paid.OwnerFrameId == f.Id &&
                paid.Cost.CardId == entry.CardId && paid.Cost.CardKind == entry.CardKind && paid.Cost.From == entry.From &&
                response?.ConversionChain.Contains(paid.Source) == true)) continue;
            if (owner is not null && response is not null)
                AdvanceEventRulesAndQueueFact(new ShanheBossMaterialCancelledEvent(entry.CardId, owner.Id,
                    response.ActionId, entry.Sequence, CardActionType.Response));
        }
    }

    private bool TryDescribeShanheBossCancelledMaterial(int cardId, CardMovementRecord entry,
        CardMoveReason reason, out ShanheBossMaterialCancelledEvent cancelled)
    {
        cancelled = null!;
        if (reason.Value != ShanheBossInterruptedProcessingReason || _options.ShanheBattle?.BossTransition is null ||
            ProgramEventHistory<ShanheBossStageStartedEvent>().Count != 1 ||
            ProgramEventHistory<ShanheBossMaterialCancelledEvent>().Where(e => e.CardId == cardId &&
                e.EntrySequence == entry.Sequence).ToArray() is not [var receipt] ||
            !ProgramEventHistory<ShanheBossResolutionCancelledEvent>().Any(e => e.FrameId == receipt.FrameId) ||
            _resolutionStack.Any(f => f.Id == receipt.FrameId) || entry.CardId != cardId || entry.To != CardLocation.Processing)
            return false;
        if (receipt.ActionType == CardActionType.Use && !ProgramEventHistory<ShanheStartingResourceUseEnteredEvent>().Any(e =>
            e.CardId == cardId && e.EntrySequence == entry.Sequence && e.UseFrameId == receipt.FrameId && e.ActionId == receipt.ActionId)) return false;
        if (receipt.ActionType == CardActionType.Response && !ProgramEventHistory<CardActionAcceptedEvent>().Any(e =>
            e.Action.ActionId == receipt.ActionId && e.Action.Type == CardActionType.Response && e.Action.PhysicalCards.Any(c =>
                c.CardId == cardId && c.CardKind == entry.CardKind && c.From == entry.From))) return false;
        cancelled = receipt;
        return true;
    }
}
