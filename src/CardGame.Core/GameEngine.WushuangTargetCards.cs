using System.Text.Json.Serialization;

namespace CardGame.Core;

public sealed record WushuangTargetCardReceipt(long ActionId, int TargetSeat, int Remaining, bool ExtraSelection = false);
public sealed partial record CardUseFrame
{
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public WushuangTargetCardReceipt? WushuangTargetCards { get; init; }
}

public sealed partial class GameEngine
{
    private void BindWushuangTargetCards(NullificationWindowFrame pending, int targetSeat)
    {
        if (!IsWushuangMode || pending.ActionKind != LegalActionKind.Dismantlement ||
            LifecycleCardUse(pending.ParentFrameId) is not { Action: { } action, WushuangTargetCards: null } use) return;
        var extra = (HasWushuangTactic(pending.SourceSeat, 2239) ? 1 : 0) + (HasWushuangTactic(pending.SourceSeat, 2240) ? 2 : 0);
        if (extra > 0) UpdateLifecycleCardUse(use.Id, current => current with { WushuangTargetCards = new(action.ActionId, targetSeat, extra) });
    }

    private bool IsWushuangExtraForeignSelection(long useId) => LifecycleCardUse(useId)?.WushuangTargetCards is { ExtraSelection: true };

    private bool TryContinueWushuangTargetCards(long id, Card effectCard, CardKind effectiveKind, int source, int target, LegalActionKind kind)
    {
        if (LifecycleCardUse(id) is not { WushuangTargetCards: { } receipt } use) return false;
        if (use.Action?.ActionId != receipt.ActionId || source != use.SourceSeat || target != receipt.TargetSeat || kind != LegalActionKind.Dismantlement)
            throw new InvalidOperationException("Additional Dismantlement lost its owning accepted effect.");
        if (receipt.Remaining <= 0 || !_players[target].IsAlive ||
            GetHand(_players[target]).Count + GetEquipment(_players[target]).Count + GetJudgment(_players[target]).Count == 0)
        { UpdateLifecycleCardUse(id, current => current with { WushuangTargetCards = null }); return false; }
        UpdateLifecycleCardUse(id, current => current with { WushuangTargetCards = receipt with { Remaining = receipt.Remaining - 1, ExtraSelection = true } });
        var continuation = new NullificationWindowFrame(id, id, source, [target], effectCard.Id, effectiveKind, kind, null, null, []);
        if (!TryBeginExtraForeignSelection(continuation, _players[target]))
            throw new InvalidOperationException("Additional Dismantlement did not create its typed selection.");
        return true;
    }

    private void FinishWushuangTargetCards(CardUseFrame use)
    {
        if (use.WushuangTargetCards is not { ExtraSelection: true } receipt || use.Action?.ActionId != receipt.ActionId)
            throw new InvalidOperationException("Additional Dismantlement cannot decline another use's effect.");
        UpdateLifecycleCardUse(use.Id, current => current with { WushuangTargetCards = null });
        var card = GetTrickRepresentation(use.Id, use.CardId, true);
        MoveFinishedTrickCard(use.Id, card); FinishCardUse(use.Id, card, CardKind.Dismantlement);
    }

    private void WushuangClaimNullifiedTrick(NullificationWindowFrame window,
        IReadOnlyList<Card> finishedPhysicalCards, int beforeFinishMovement)
    {
        if (!IsWushuangMode || !window.EffectNullified) return;
        var response = ProgramEventHistory<NullificationRespondedEvent>().LastOrDefault(e =>
            e.ResolutionId == window.ParentFrameId && e.ChainDepth == window.ChainDepth && e.EffectNullified);
        if (response is null || !HasWushuangTactic(response.ResponderSeat, 2008) || !_players[response.ResponderSeat].IsAlive) return;
        if (LifecycleCardUse(window.ParentFrameId) is not { Action: { } action } use ||
            action.ActorSeat != window.SourceSeat ||
            !(use.PhysicalCardIds ?? [use.CardId]).SequenceEqual(finishedPhysicalCards.Select(card => card.Id)))
            throw new InvalidOperationException("A counterspell claim lost its exact native trick materials.");
        foreach (var card in finishedPhysicalCards.Where(c => _cardZones.GetLocation(c.Id) == CardLocation.DiscardPile &&
            _cardMovements.Any(move => move.Sequence > beforeFinishMovement && move.CardId == c.Id &&
                move.From == CardLocation.Processing && move.To == CardLocation.DiscardPile && move.Reason == CardMoveReasons.UseFinished)))
            MoveCard(card, CardLocation.DiscardPile, CardLocation.Hand(response.ResponderSeat), new("wushuang.counterspell-claim"));
    }
}
