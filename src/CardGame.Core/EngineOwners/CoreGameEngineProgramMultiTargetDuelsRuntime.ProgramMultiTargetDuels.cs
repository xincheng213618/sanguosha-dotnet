#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CoreGameEngineProgramMultiTargetDuelsRuntime
{
    internal bool HasProgramIronChainTargetExpansion(int sourceSeat) => _coreRuleQueriesOperations.EvaluateCardTargetCount(_coreMatchParticipantsPlayersAccess._players[sourceSeat], CardKind.IronChain).Value is FiniteRuleQueryValue { Value: > 2 };
    internal void AddProgramMultiTargetDuelActions(ICollection<LegalAction> actions, CharacterState actor)
    {
        // Existing single-target actions retain all card, conversion and target qualifications.
        // Only their already legal targets can be combined; the physical card is still paid once.
        if (_coreRuleQueriesOperations.EvaluateCardTargetCount(actor, CardKind.Duel).Value is not FiniteRuleQueryValue { Value: > 1 })
            return;
        var groups = actions.Where(action => action.Kind == LegalActionKind.Duel && action.TargetSeats.Count == 1).GroupBy(action => (action.CardId, action.PlayedCardKind, action.ConversionSource)).ToArray();
        foreach (var group in groups)
        {
            var original = group.First();
            var candidates = group.SelectMany(action => action.TargetSeats).Distinct().Where(seat => !_coreDirectedTurnCardPoliciesOperations.IsDirectedCardTargetProhibited(actor.Seat, seat, CardKind.Duel)).OrderBy(seat => _coreTableOrderOperations.RelativeTableOrder(actor.Seat, seat)).ToArray();
            foreach (var selected in EnumerateProgramDuelTargets(actor, candidates, minimum: 2))
                actions.Add(original with { TargetSeat = null, TargetSeats = selected, Description = "对 " + string.Join("、", selected.Select(seat => _coreMatchParticipantsPlayersAccess._players[seat].Name)) + " 使用【决斗】" });
        }
    }

    internal IEnumerable<IReadOnlyList<int>> EnumerateProgramDuelTargets(CharacterState actor, IReadOnlyList<int> candidates, int minimum = 1)
    {
        var maximum = ((FiniteRuleQueryValue)_coreRuleQueriesOperations.EvaluateCardTargetCount(actor, CardKind.Duel).Value).Value;
        for (var count = minimum; count <= Math.Min(maximum, candidates.Count); count++)
            foreach (var targets in Select(0, count, []))
                yield return targets;
        IEnumerable<IReadOnlyList<int>> Select(int start, int remaining, IReadOnlyList<int> selected)
        {
            if (remaining == 0)
            {
                yield return Array.AsReadOnly(selected.ToArray());
                yield break;
            }

            for (var index = start; index <= candidates.Count - remaining; index++)
                foreach (var targets in Select(index + 1, remaining - 1, [..selected, candidates[index]]))
                    yield return targets;
        }
    }

    internal void ResolveProgramMultiTargetDuel(CharacterState source, Card duel, LegalAction action)
    {
        var targets = action.TargetSeats.ToArray();
        if (targets.Length < 2 || !_coreNativeLegalActionsOperations.BuildLegalActions(source, selectedPhysicalCardId: duel.Id).Any(candidate => candidate.Kind == LegalActionKind.Duel && candidate.CardId == duel.Id && candidate.PlayedCardKind == action.PlayedCardKind && candidate.ConversionSource == action.ConversionSource && candidate.TargetSeats.SequenceEqual(targets)))
            throw new InvalidOperationException("The exact multi-target Duel became illegal before resolution.");
        var id = _coreNativeCardUseOperations.BeginCardUse(duel, source.Seat, targets, action.PlayedCardKind);
        _corePhysicalMovementFacadesOperations.MoveCard(duel, _coreNativeOwnedCardQueriesOperations.FindOwnedCardLocation(source, duel), CardLocation.Processing, CardMoveReasons.Use, disclosure: CardMovementDisclosure.FaceUp);
        _coreNativeNullificationOperations.BeginJizhiOrNullificationWindow(id, duel, source.Seat, targets, LegalActionKind.Duel, playedCardKind: action.PlayedCardKind);
    }
}
