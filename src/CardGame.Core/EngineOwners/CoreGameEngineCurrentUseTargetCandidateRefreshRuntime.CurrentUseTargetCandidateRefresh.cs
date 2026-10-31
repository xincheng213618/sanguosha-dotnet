#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CoreGameEngineCurrentUseTargetCandidateRefreshRuntime
{
    internal CardUseFrame NormalizeImplicitDrawTwoUseTarget(CardUseFrame use)
    {
        if (use.CardKind != CardKind.DrawTwo || use.TargetSeats.Count != 0 || use.Action is not { } old || !_coreProgramTargetWindowProtocolsOperations.TargetWindowProtocols.HasCardWindow(ProgramTargetWindowStage.NormalizeImplicitDrawTwo, new(use.CardKind)))
            return use;
        var targets = Array.AsReadOnly(new[] { use.SourceSeat });
        var action = _coreDesignatedExtraTargetsOperations.CloneDesignatedExtraTargetAction(old, targets);
        use = use with
        {
            TargetSeats = targets,
            Action = action
        };
        _coreProgramCardUseRolesOperations.UpdateProgramRoleCardUse(use, action);
        return use;
    }

    internal void RefreshCurrentUseAddedTargetCandidates(CardUseFrame use, IReadOnlyList<int> addedMainTargets)
    {
        if (use.Action is not { Type: CardActionType.Use } action || addedMainTargets.Count == 0)
            return;
        var window = _resolutionStackOperations._resolutionStack.OfType<ProgramCardTriggerWindowFrame>().LastOrDefault(w => w.ParentFrameId == use.Id && global::CardGame.Core.EngineOwners.CoreGameEngineCardActionsRuntime.GetCardActionWindow(w) == SkillProgramTriggerWindow.CardUseTargetsFinalized);
        if (window is null)
            throw new InvalidOperationException("A target addition lost its actual designation window.");
        var newCandidates = _coreCardActionsOperations.CollectSharedCardActionCandidates(action, SkillProgramTriggerWindow.CardUseTargetsFinalized, addedMainTargets, null, window.Continuation).Where(c => addedMainTargets.Contains(c.OwnerSeat) && _contentRegistryOperations._contentRegistry.GetSkill(c.SkillId).Program!.Triggers.Single(t => t.Id == c.TriggerId).OwnerRelation == SkillProgramCardActionOwnerRelation.Target && !window.Candidates.Any(old => old.OwnerSeat == c.OwnerSeat && old.SkillId == c.SkillId && old.TriggerId == c.TriggerId && old.SkillInstanceId == c.SkillInstanceId)).OrderByDescending(c => c.Priority).ThenBy(c => _coreTableOrderOperations.RelativeTableOrder(0, c.OwnerSeat)).ThenBy(c => c.SkillId, StringComparer.Ordinal).ThenBy(c => c.SkillInstanceId, StringComparer.Ordinal).Select(c => c with { FrozenContext = c.FrozenContext!with { ParentFrameId = window.Id, CardUse = c.FrozenContext.CardUse!with { ParentCardUseFrameId = use.Id } } }).ToArray();
        if (newCandidates.Length > 0)
            _coreRuntimeOperations.ReplaceRuntimeFrame(window.Id, window with { Candidates = Array.AsReadOnly(window.Candidates.Concat(newCandidates).ToArray()) });
    }
}
