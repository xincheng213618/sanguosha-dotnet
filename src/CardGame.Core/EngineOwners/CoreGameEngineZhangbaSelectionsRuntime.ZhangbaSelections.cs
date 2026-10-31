#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CoreGameEngineZhangbaSelectionsRuntime
{
    internal IReadOnlyList<ActiveSkillCardGroupTarget> GetZhangbaSlashGroupTargets(CharacterState actor)
    {
        var choices = new List<ActiveSkillCardGroupTarget>();
        foreach (var pair in _coreNativeResponseMaterialQueriesOperations.GetZhangbaSlashPairs(actor))
        {
            var ids = Array.AsReadOnly(pair.Select(card => card.Id).ToArray());
            if (_coreProgramParticipantPhaseOperationsOperations.IsTurnPhysicalUseForbidden(actor.Seat, ids))
                continue;
            foreach (var target in _coreMatchParticipantsPlayersAccess._players)
                if (_coreNativeTargetLegalityOperations.CanUseVirtualSlashTarget(actor, target, CardKind.Slash, _coreAlternatingSuitAndCompletedTopOperations.PhysicalGroupSuit(actor, pair), _coreTurnRedSlashBenefitsOperations.PhysicalGroupColor(actor, pair), _coreFinalTargetSlashOperations.ZhangbaSpecificSlashRank(actor, pair), ids))
                    choices.Add(new(ids, target.Seat));
        }

        return choices.AsReadOnly();
    }

    internal static bool MatchesPublishedCardGroupTarget(LegalAction action, IReadOnlyList<int> cardIds, IReadOnlyList<int> targets) => action.SelectableCardGroupTargets is not { } groups || targets.Count >= action.MinTargetCount && targets.Count <= action.MaxTargetCount && targets.All(target => groups.Any(group => group.TargetSeat == target && group.CardIds.Count == cardIds.Count && group.CardIds.Order().SequenceEqual(cardIds.Order())));
}
