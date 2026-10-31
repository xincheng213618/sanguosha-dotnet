#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterLiFengOlRuntime
{
    internal bool IsShuliangGrainQuotaSpent(int providerSeat, int actualTurn, int turnOwnerSeat) => _coreProgramEventHistoryOperations.ProgramEventHistory<ShuliangGrainPaidEvent>().Any(e => e.ProviderSeat == providerSeat && e.ActualTurn == actualTurn && e.TurnOwner == turnOwnerSeat);
    internal IReadOnlyList<NativePublicPileCardSelection> ShuliangOriginalGrainChoices(NativePublicPileCardRequest request)
    {
        if (!HasLiFengCapability || !_coreTableSeatQueriesOperations.IsValidPlayerSeat(request.ProviderSeat) || !_coreTableSeatQueriesOperations.IsValidPlayerSeat(request.ActorSeat) || !_coreMatchParticipantsPlayersAccess._players[request.ProviderSeat].IsAlive || !_coreMatchParticipantsPlayersAccess._players[request.ActorSeat].IsAlive || !Enum.IsDefined(request.Direction) || IsShuliangGrainQuotaSpent(request.ProviderSeat, _coreActualTurnProgressTurnNumberTurnProgressionAccess._turnNumber, _coreActualTurnProgressTurnNumberTurnProgressionAccess._turnProgression.OwnerSeat) || _resolutionStackOperations._resolutionStack.Any(f => f.NativePublicPilePayment is { } p && p.Request.ProviderSeat == request.ProviderSeat && p.ActualTurn == _coreActualTurnProgressTurnNumberTurnProgressionAccess._turnNumber && p.TurnOwnerSeat == _coreActualTurnProgressTurnNumberTurnProgressionAccess._turnProgression.OwnerSeat))
            return[];
        var sources = _coreNationalSkillsOperations.GetSkillBindingShard(_coreMatchParticipantsPlayersAccess._players[request.ProviderSeat]).ProgramInstances.Where(i => i.SkillId == "ol:shuliang" && i.Program.Triggers.Any(t => t.Effects is [{ Op: SkillProgramEffectOp.DistributeShuliangEndingGrain }])).OrderBy(i => i.SkillInstanceId, StringComparer.Ordinal);
        var result = new List<NativePublicPileCardSelection>();
        foreach (var source in sources)
            foreach (var pile in _corePublicPileSourcesOperations.ReferencedPublicPileSources(request.ProviderSeat, "ol:jiyun", source.SkillInstanceId).Where(IsLiveJiyunPile))
                foreach (var card in _corePublicPileSourcesOperations.PublicPileCards(pile))
                    if (request.RequiredKind is null || global::CardGame.Core.EngineOwners.CoreNativeCardClassificationsRuntime.MatchesRequiredCard(card.Kind, request.RequiredKind.Value))
                        result.Add(new(pile, new("ol:shuliang", "original-grain", request.ProviderSeat, source.SkillInstanceId), card, request));
        return Array.AsReadOnly(result.DistinctBy(c => (c.Card.Id, c.AccessSource)).ToArray());
    }

    internal bool IsShuliangOriginalGrainChoice(NativePublicPileCardRequest request, NativePublicPileCardSelection selection) => selection.Request == request && ShuliangOriginalGrainChoices(request).Contains(selection);
    internal void RecordShuliangGrainPayment(NativePublicPileCardPayment payment)
    {
        if (payment.Source.SkillId != "ol:shuliang" || payment.Source.BindingId != "original-grain" || payment.Source.OwnerSeat != payment.Request.ProviderSeat || payment.Pile.OwnerSeat != payment.Request.ProviderSeat || payment.Pile.SkillId != "ol:jiyun" || payment.Pile.Capacity != 3 || payment.ActualTurn != _coreActualTurnProgressTurnNumberTurnProgressionAccess._turnNumber || payment.TurnOwnerSeat != _coreActualTurnProgressTurnNumberTurnProgressionAccess._turnProgression.OwnerSeat || payment.SequenceBefore < 0 || payment.SequenceAfter <= payment.SequenceBefore || payment.SequenceAfter > _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence || _resolutionStackOperations._resolutionStack.SingleOrDefault(f => f.Id == payment.OwnerFrameId)?.NativePublicPilePayment != payment || _coreCardMovementJournalCardMovementsMovementSequenceAccess._cardMovements.SingleOrDefault(m => m.Sequence == payment.SequenceAfter && m.CardId == payment.Card.Id && m.CardKind == payment.Card.Kind && m.From == payment.Pile.Location && m.To != m.From)is not { } movement)
            throw new InvalidOperationException("Shuliang requires one actual authorized original-grain payment.");
        var previous = _coreProgramEventHistoryOperations.ProgramEventHistory<ShuliangGrainPaidEvent>().Where(e => e.ParentFrameId == payment.OwnerFrameId && e.MovementSequence == movement.Sequence).ToArray();
        if (previous.Length != 0)
        {
            if (previous is not [var fact] || fact.Source != payment.Source || fact.CardId != payment.Card.Id || fact.From != payment.Pile.Location)
                throw new InvalidOperationException("Shuliang cannot rebind a different paid grain.");
            return;
        }

        if (IsShuliangGrainQuotaSpent(payment.Request.ProviderSeat, payment.ActualTurn, payment.TurnOwnerSeat))
            throw new InvalidOperationException("Shuliang shares one actual-turn payment between Use and Played.");
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new ShuliangGrainPaidEvent(payment.OwnerFrameId, payment.ActionId ?? 0, payment.Source, payment.Request.ProviderSeat, payment.ActualTurn, payment.TurnOwnerSeat, payment.Card.Id, payment.Pile.Location, movement.Sequence, payment.Request.Direction == NativePublicPileDirection.Played));
    }
}
