#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;
using System.Globalization;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterTaoQianOlRuntime
{
    internal int[] ZongluanTargets(int actor) => !_coreMatchParticipantsPlayersAccess._players[actor].IsAlive ? [] : _coreMatchParticipantsPlayersAccess._players.Where(p => _coreNativeTargetLegalityOperations.CanUseVirtualSlashTarget(_coreMatchParticipantsPlayersAccess._players[actor], p, CardKind.Slash, Suit.None, countsTowardLimit: false, effectiveColor: null)).Select(p => p.Seat).ToArray();
    internal IReadOnlyList<PromptChoice> ZongluanDiscardCards(ProgramSkillFrame f) => _coreProgramCardPaymentsOperations.BuildOwnedCardPaymentChoices(f.Id, f.OwnerSeat, f.OwnerSeat, [CardZoneKind.Hand, CardZoneKind.Equipment], OwnedCardMoveIntent.Discard, canSelect: (_, card) => !f.TaoQian!.Selected.Contains(card.Id));
    internal void PayZongluanDiscard(ProgramSkillFrame f)
    {
        var r = f.TaoQian!;
        if (!_coreMatchParticipantsPlayersAccess._players[f.OwnerSeat].IsAlive)
        {
            FinishTaoQian(f);
            return;
        }

        var legal = _coreProgramCardPaymentsOperations.BuildOwnedCardPaymentChoices(f.Id, f.OwnerSeat, f.OwnerSeat, [CardZoneKind.Hand, CardZoneKind.Equipment], OwnedCardMoveIntent.Discard).SelectMany(c => c.Cards).ToHashSet();
        if (r.Selected.Count != r.DiscardRequired || r.Selected.Distinct().Count() != r.Selected.Count || r.Selected.Any(id => !legal.Contains(id)))
            throw new InvalidOperationException("Zongluan requires exactly its published real legal HE discard entities.");
        var materials = r.Selected.Select(id => new CardActionCost(id, _coreAdvancedLifecycleOperations.GetAdvancedCard(id).Kind, _cardZonesOperations._cardZones.GetLocation(id))).ToArray();
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { TaoQian = r with { Materials = materials, Before = _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence, After = _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence, Stage = TaoQianStage.DiscardChildren }, PendingMovementContinuation = new(f.OwnerSeat, 0, null) });
        _coreNativeLogicalDiscardsOperations.MoveProgramLogicalDiscard(f, f.OwnerSeat, r.Selected, new(ZongluanDiscardReason), (batch, records) =>
        {
            for (var i = 0; i < records.Count; i++)
            {
                var m = records[i];
                _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new ZongluanDiscardEntityEvent(f.Id, i, batch, m.CardId, m.CardKind, m.From, m.To, m.Sequence));
            }
        });
        f = _coreProgramLifecycleOperations.GetActiveProgramFrame(f.Id);
        _coreRuntimeOperations.ReplaceRuntimeTop(f with { TaoQian = f.TaoQian!with { After = _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence } });
        ContinueTaoQian(f.Id);
    }
}
