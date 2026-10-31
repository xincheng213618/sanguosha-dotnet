#nullable enable

namespace CardGame.Core.EngineOwners;
internal sealed partial class CoreGameEngineGroupTargetNullificationRuntime
{
    internal static bool UsesGroupTargetNullification(LegalActionKind kind) => kind is LegalActionKind.BarbarianAssault or LegalActionKind.ArrowBarrage or LegalActionKind.FiveGrains or LegalActionKind.PeachGarden;
    internal static LegalActionKind GroupNullificationAction(CardKind kind) => kind switch
    {
        CardKind.BarbarianAssault => LegalActionKind.BarbarianAssault,
        CardKind.ArrowBarrage => LegalActionKind.ArrowBarrage,
        CardKind.FiveGrains => LegalActionKind.FiveGrains,
        CardKind.PeachGarden => LegalActionKind.PeachGarden,
        _ => throw new InvalidOperationException("Unknown group counterspell effect.")};
    internal CardKind GroupNullificationCardKind(NativeOwners.GroupCardHandle group) => _characterZhugeGuoLeOperations.IsLeChengyanEffectUse(group.ResolutionId) ? _coreCardUseLifecycleOperations.LifecycleCardUse(group.ResolutionId)!.CardKind : group.Card.Kind;
    internal bool IsGroupTargetNullification(NullificationWindowFrame window) => window.GroupTargetReturn is { } target && _coreNativeCardContinuationCursorsOperations.ActiveGroupCard is { } group && group.ResolutionId == window.ParentFrameId && group.Card.Id == window.EffectCardId && GroupNullificationCardKind(group) == window.EffectCardKind && group.SourceSeat == window.SourceSeat && group.TargetIndex == target.TargetIndex && target.TargetIndex >= 0 && target.TargetIndex < group.TargetSeats.Count && group.TargetSeats[target.TargetIndex] == target.TargetSeat && window.TargetSeats.SequenceEqual([target.TargetSeat]) && group.CurrentAttack is null && group.NullificationCompletedTargetIndex != target.TargetIndex && window.ActionKind == GroupNullificationAction(group.Card.Kind) && window.RequiredCardKind == group.RequiredCardKind;
    internal bool TryBeginGroupTargetNullification(NativeOwners.GroupCardHandle group)
    {
        if (group.NullificationCompletedTargetIndex == group.TargetIndex)
            return false;
        if (CoreNativeCardClassificationsRuntime.IsSlashCard(GroupNullificationCardKind(group)) || _coreUnnullifiableOrdinaryTricksOperations.IsOriginalOrdinaryTrickCannotNullify(group.ResolutionId) || _coreOrderedSelectedActorDuelsOperations.IsOrderedSelectedActorDuelCannotNullify(group.ResolutionId) || _characterLuMengMouOperations.IsYingboUnrespondable(group.ResolutionId) || _coreIssuedPlayUsePoliciesOperations.IsIssuedCardUnrespondable(group.ResolutionId))
        {
            group.NullificationCompletedTargetIndex = group.TargetIndex;
            return false;
        }

        if (_coreNativeCardContinuationCursorsOperations.ActiveNullificationWindow is not null || _resolutionStackOperations._resolutionStack.LastOrDefault()?.Id != group.ResolutionId)
            throw new InvalidOperationException("A group target counterspell must start on its owning card frame.");
        var kind = GroupNullificationAction(group.Card.Kind);
        var targetSeat = group.TargetSeats[group.TargetIndex];
        _coreNativeCardUseFramesOperations.SetCardUseStep(group.ResolutionId, ResolutionFrameStep.AwaitingResponse);
        var window = new NullificationWindowFrame(++_coreResolutionIdentityBoundaryOperations._resolutionSequence, group.ResolutionId, group.SourceSeat, Array.AsReadOnly(new[] { targetSeat }), group.Card.Id, GroupNullificationCardKind(group), kind, null, group.RequiredCardKind, _coreNativeNullificationOperations.BuildNullificationCandidateSeats(group.SourceSeat))
        {
            GroupTargetReturn = new(group.TargetIndex, targetSeat)
        };
        var recovery = group.Effect == GroupCardEffect.Recovery;
        var targetIndex = group.TargetIndex;
        _coreRuntimeOperations.PushRuntimeFrame(window);
        _coreNativeNullificationOperations.ContinueNullificationWindow(window);
        // A recovery step with no responders can apply its current target now.
        // A canceled target or a live response window has already consumed this step.
        return !recovery || _coreNativeCardContinuationCursorsOperations.ActiveNullificationWindow is not null || !CoreNativeDamageHandlesRuntime.SameContinuationOwner(_coreNativeCardContinuationCursorsOperations.ActiveGroupCard, group) || group.TargetIndex != targetIndex;
    }

    internal void CompleteGroupTargetNullification(NullificationWindowFrame window)
    {
        if (!IsGroupTargetNullification(window) || _resolutionStackOperations._resolutionStack.LastOrDefault()?.Id != window.ParentFrameId)
            throw new InvalidOperationException("A group counterspell lost its exact target return.");
        var group = _coreNativeCardContinuationCursorsOperations.ActiveGroupCard!;
        group.NullificationCompletedTargetIndex = group.TargetIndex;
        if (window.EffectNullified && !_coreCurrentCardEnhancementsOperations.HasCurrentCardEnhancement(group.ResolutionId, CurrentCardEnhancement.Uncancelable))
        {
            _coreMatchLogOperations.AddLog("CardEffect", $"【{(CardCatalog.Get(window.EffectCardKind).DisplayName)}】对{(_coreMatchParticipantsPlayersAccess._players[window.TargetSeats[0]].Name)}的效果被无懈。", group.SourceSeat, window.TargetSeats[0]);
            group.TargetIndex++;
        }

        if (_coreMatchOutcomeWinnerAccess._winner != Winner.None)
            group.TargetIndex = group.TargetSeats.Count;
        switch (group.Effect)
        {
            case GroupCardEffect.ResponseAttack:
                _coreNativeGroupAttackOperations.BeginGroupAttackResponse(group);
                break;
            case GroupCardEffect.PublicDraft:
                _coreNativeHarvestOperations.BeginHarvestSelection(group);
                break;
            case GroupCardEffect.Recovery:
                if (group.TargetIndex >= group.TargetSeats.Count)
                    _coreNativeGroupCompletionOperations.FinishGroupRecovery(group);
                break;
        }
    }

    internal string NullificationEffectDescription(NullificationWindowFrame window) => window.GroupTargetReturn is { } target ? $"【{(CardCatalog.Get(window.EffectCardKind).DisplayName)}】对{(_coreMatchParticipantsPlayersAccess._players[target.TargetSeat].Name)}的效果" : $"【{(CardCatalog.Get(window.EffectCardKind).DisplayName)}】效果";
}
