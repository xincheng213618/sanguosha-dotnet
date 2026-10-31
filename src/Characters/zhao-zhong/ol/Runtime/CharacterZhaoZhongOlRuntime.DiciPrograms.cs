#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;
using System.Globalization;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterZhaoZhongOlRuntime
{
    internal void PayDiciGift(ProgramSkillFrame f, int cardId, bool next)
    {
        var r = f.ZhaoZhong!;
        var from = CardLocation.Hand(r.TargetSeat);
        var card = _cardZonesOperations._cardZones.CardsAt(from).Single(c => c.Id == cardId);
        if (!_coreMatchParticipantsPlayersAccess._players[r.TargetSeat].IsAlive || !_coreMatchParticipantsPlayersAccess._players[f.OwnerSeat].IsAlive || r.Stage != (next ? ZhaoZhongStage.NextChoice : ZhaoZhongStage.InitialGive))
            throw new InvalidOperationException("Dici cannot give another person's card or give to a dead recipient.");
        if (!next)
        {
            if (r.Paid || r.Phase is not { } phase || _coreProgramEventHistoryOperations.ProgramEventHistory<DiciAcceptedEvent>().Any(e => e.Source.OwnerSeat == f.OwnerSeat && e.Phase == phase))
                throw new InvalidOperationException("Dici repeated its actual Play-phase payment.");
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new DiciAcceptedEvent(f.Id, r.Source, r.GameplayHash, r.TargetSeat, phase));
        }
        else
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new DiciNextChoiceEvent(f.Id, r.LeaseId, "give"));
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { ZhaoZhong = r with { Paid = true, CardId = card.Id, CardKind = card.Kind, From = from, Before = _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence, After = _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence, Stage = next ? ZhaoZhongStage.NextGiftChildren : ZhaoZhongStage.GiftChildren }, PendingMovementContinuation = new(f.OwnerSeat, 0, null) });
        _corePhysicalMovementFacadesOperations.MoveCard(card, from, CardLocation.Hand(f.OwnerSeat), new(next ? DiciNextGiveReason : DiciGiveReason), _characterHuangQuanMouOperations.NativeGiveCompletion(f.Id, from.OwnerSeat!.Value, f.OwnerSeat, [card.Id], _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence, m => _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new DiciGiftEntityEvent(f.Id, r.LeaseId, m.CardId, m.CardKind, m.From, m.To, m.Sequence))));
        f = _coreProgramLifecycleOperations.GetActiveProgramFrame(f.Id);
        _coreRuntimeOperations.ReplaceRuntimeTop(f with { ZhaoZhong = f.ZhaoZhong!with { After = _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence } });
        ContinueZhaoZhong(f.Id);
    }

    internal int[] DiciNeighbors(int target)
    {
        var living = _coreTableOrderOperations.CurrentTableOrder().Where(seat => _coreMatchParticipantsPlayersAccess._players[seat].IsAlive).ToArray();
        var index = Array.IndexOf(living, target);
        return index < 0 || living.Length < 2 ? [] : new[]
        {
            living[(index + 1) % living.Length],
            living[(index + living.Length - 1) % living.Length]
        }.Distinct().ToArray();
    }
}
