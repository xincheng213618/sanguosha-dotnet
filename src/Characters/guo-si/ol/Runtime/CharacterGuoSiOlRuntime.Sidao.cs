#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterGuoSiOlRuntime
{
    internal IReadOnlyList<LegalAction> SidaoActions(int owner, GuoSiPair pair)
    {
        if (!_coreMatchParticipantsPlayersAccess._players[owner].IsAlive)
            return[];
        var result = new List<LegalAction>();
        var actor = _coreMatchParticipantsPlayersAccess._players[owner];
        foreach (var card in _corePhysicalOwnedCardQueriesOperations.GetHand(actor).Where(c => !_coreCardUseModulesOperations.IsTurnHandCardRestricted(actor, c) && !_coreProgramParticipantPhaseOperationsOperations.IsTurnPhysicalUseForbidden(owner, [c.Id])))
        {
            var suit = _coreProgramCardPoliciesOperations.GetProgramEffectiveSuit(actor, card);
            foreach (var o in _coreProgramOrdinaryTrickUsesOperations.BuildProgramOrdinaryTrickUseOptions(actor, CardKind.Snatch, suit, enforceUsePermission: true, beneficiaryShieldSuit: suit, actualEffectiveColor: global::CardGame.Core.EngineOwners.CoreGameEnginePublicPileColorsRuntime.SuitColor(suit), hasActualColor: true, physicalCardIds: [card.Id], includeNextActualUseAdjustment: false))
                if (o.TargetSeats.Count == 1 && pair.CommonTargets.Contains(o.TargetSeats[0]) && _coreMatchParticipantsPlayersAccess._players[o.TargetSeats[0]].IsAlive && !_coreDirectedTurnCardPoliciesOperations.IsDirectedCardTargetProhibited(owner, o.TargetSeats[0], CardKind.Snatch))
                    result.Add(new(o.ActionKind, card.Id, o.TargetSeats[0], $"将【{(card.DisplayName)}】当【顺手牵羊】对{(_coreMatchParticipantsPlayersAccess._players[o.TargetSeats[0]].Name)}使用", CardKind.Snatch, o.TargetCardId, Array.AsReadOnly(o.TargetSeats.ToArray())));
        }

        return Array.AsReadOnly(result.ToArray());
    }
}
