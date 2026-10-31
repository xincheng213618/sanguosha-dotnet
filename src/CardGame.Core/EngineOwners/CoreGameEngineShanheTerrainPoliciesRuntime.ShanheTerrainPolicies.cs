#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CoreGameEngineShanheTerrainPoliciesRuntime
{
    internal RuleQueryEvaluation ApplyMinimumHandLimitPolicy(CharacterState player, RuleQueryEvaluation ordinary)
    {
        if (!_contentRegistryOperations._contentRegistry.ProgramDependencies.HasCardPolicy(SkillProgramCardPolicyKind.MinimumHandLimit) || ordinary.Value is not FiniteRuleQueryValue finite)
            return ordinary;
        var policies = _coreProgramCardPoliciesOperations.CardPolicies(player, SkillProgramCardPolicyKind.MinimumHandLimit).ToArray();
        if (policies.Length == 0)
            return ordinary;
        var minimum = policies.Max(item => item.Policy.Value);
        if (finite.Value >= minimum)
            return ordinary;
        var contributions = policies.Where(item => item.Policy.Value == minimum).Select(item => (RuleQueryContribution)new FiniteRuleQueryContribution($"skill:{(item.Source.SkillId)}:{(item.Source.SkillInstanceId)}:policy:{(item.Policy.Id)}:minimum", SkillRuleOperation.Set, item.Policy.Value, 5410)).ToArray();
        var frozen = Array.AsReadOnly(ordinary.Value.Contributions.Concat(contributions).OrderBy(item => item.SourceId, StringComparer.Ordinal).ToArray());
        var stage = new RuleQueryStageSnapshot("minimum-hand-limit", finite.Value, false, minimum, Array.AsReadOnly(contributions));
        return new(new FiniteRuleQueryValue(minimum, frozen), ordinary.BaseTerms, Array.AsReadOnly(ordinary.Stages.Append(stage).ToArray()));
    }

    [ProgramCardLegality(ProgramCardLegalityStage.BeforeTurnUseBan, 5411)]
    internal bool IsMaximumOwnPlayCardUseForbidden(ProgramCardUseQuery query)
    {
        if (!_contentRegistryOperations._contentRegistry.ProgramDependencies.HasCardPolicy(SkillProgramCardPolicyKind.MaximumOwnPlayCardUses) || _coreActualTurnProgressCurrentSeatPhaseAccess._phase != TurnPhase.Play || query.ActorSeat != _coreActualTurnProgressCurrentSeatPhaseAccess._currentSeat || !_coreMatchParticipantsPlayersAccess._players[query.ActorSeat].IsAlive || query.ActionType != CardActionType.Use && !(query.ActionType == CardActionType.Response && (query.Kind == CardKind.Nullification || _coreAlternativeViewAsCostsOperations.IsProgramResponseCardUse(_coreMatchParticipantsPlayersAccess._players[query.ActorSeat], query.Kind))))
            return false;
        var caps = _coreProgramCardPoliciesOperations.CardPolicies(_coreMatchParticipantsPlayersAccess._players[query.ActorSeat], SkillProgramCardPolicyKind.MaximumOwnPlayCardUses).Select(item => item.Policy.Value).ToArray();
        if (caps.Length == 0 || _coreActualPlayPhaseCardUsesOperations.GetActualPlayPhaseUseCount(query.ActorSeat) < caps.Min())
            return false;
        // A paid tenth use keeps its existing target/material continuation. The
        // cap stops a new use and does not retroactively cancel the accepted one.
        if (query.ExistingUseFrameId is { } id && _resolutionStackOperations._resolutionStack.OfType<CardUseFrame>().Any(frame => frame.Id == id && frame.Action is { } action && action.ActorSeat == query.ActorSeat && (_coreGameEngineActualPlayPhaseCardUsesActualPlayPhaseUseActionsAccess._actualPlayPhaseUseActions.Contains(action.ActionId) || _coreProgramEventHistoryOperations.ProgramEventHistory<CardActionAcceptedEvent>().Any(accepted => accepted.Action.ActionId == action.ActionId && accepted.Action.ActorSeat == query.ActorSeat))))
            return false;
        return true;
    }
}
