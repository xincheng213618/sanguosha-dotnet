#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CoreGameEngineShanheAlliedTricksRuntime
{
    internal bool ShanheAlliedHarvestProhibited(global::CardGame.Core.NativeOwners.GroupCardHandle group, int pickerSeat) => _coreGroupTargetNullificationOperations.GroupNullificationCardKind(group) == CardKind.FiveGrains && _coreProgramCardPoliciesOperations.HasCardPolicy(_coreMatchParticipantsPlayersAccess._players[group.SourceSeat], SkillProgramCardPolicyKind.AlliedHarvestOnly, CardKind.FiveGrains) && !_coreShanheGuideSkillsOperations.IsShanheFriendlySeat(group.SourceSeat, pickerSeat);
    internal int ShanheAlliedPeachGardenRecovery(global::CardGame.Core.NativeOwners.GroupCardHandle group, int targetSeat, int original) => _coreGroupTargetNullificationOperations.GroupNullificationCardKind(group) == CardKind.PeachGarden && _coreProgramCardPoliciesOperations.HasCardPolicy(_coreMatchParticipantsPlayersAccess._players[group.SourceSeat], SkillProgramCardPolicyKind.AlliedPeachGardenDoubleRecovery, CardKind.PeachGarden) && _coreShanheGuideSkillsOperations.IsShanheFriendlySeat(group.SourceSeat, targetSeat) ? checked(original * 2) : original;
    internal bool RefreshShanheAlliedHarvestSelection()
    {
        if (_corePendingDecisionPolicyPipelineOperations._pendingDecision is not { Kind: DecisionKind.SelectHarvestCard } pending || _coreNativeCardContinuationCursorsOperations.ActiveGroupCard is not { Effect: GroupCardEffect.PublicDraft } group || _resolutionStackOperations._resolutionStack.LastOrDefault()?.Id != group.ResolutionId || group.TargetIndex < 0 || group.TargetIndex >= group.TargetSeats.Count || group.TargetSeats[group.TargetIndex] != pending.PlayerSeat || !ShanheAlliedHarvestProhibited(group, pending.PlayerSeat))
            return false;
        _coreNativePromptBoundaryOperations.ClearPendingDecision();
        _coreMatchOutcomeStatusAccess._status = EngineStatus.Running;
        _coreNativeHarvestOperations.BeginHarvestSelection(group);
        return true;
    }
}
