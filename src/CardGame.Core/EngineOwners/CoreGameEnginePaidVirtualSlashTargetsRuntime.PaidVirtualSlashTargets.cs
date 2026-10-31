#nullable enable

namespace CardGame.Core.EngineOwners;
internal sealed partial class CoreGameEnginePaidVirtualSlashTargetsRuntime
{
    internal bool TryContinuePaidVirtualSlashTargets(NativeOwners.CardAttackHandle attack)
    {
        if (_characterDaQiaoLeOperations.IsTargetScopedZixiEffectRound(attack.ResolutionId))
            return false;
        if (!_coreForeignPublicPileSlashesOperations.IsForeignPublicPileSlashUse(attack.ResolutionId) || _coreCardUseLifecycleOperations.LifecycleCardUse(attack.ResolutionId)is not { } use || !CoreNativeCardClassificationsRuntime.IsSlashCard(use.CardKind) || attack.Card is not null)
            return false;
        var next = use.TargetIndex + 1;
        while (next < use.TargetSeats.Count && !_coreMatchParticipantsPlayersAccess._players[use.TargetSeats[next]].IsAlive)
            next++;
        if (_coreMatchOutcomeWinnerAccess._winner != Winner.None || next >= use.TargetSeats.Count)
        {
            _coreCardUseLifecycleOperations.UpdateLifecycleCardUse(attack.ResolutionId, frame => frame with { ForeignPublicPileSlashBaseDamage = null });
            return false;
        }

        if (!ReferenceEquals(use, _resolutionStackOperations._resolutionStack.LastOrDefault()))
            throw new InvalidOperationException("A paid virtual Slash must finish its child before the next target.");
        var targetSeat = use.TargetSeats[next];
        _coreNativeNullificationFramesOperations.SetCardUseTargetIndex(use.Id, next);
        _coreNativeCardUseFramesOperations.SetCardUseStep(use.Id, ResolutionFrameStep.ResolvingEffect);
        var continued = _kernelCoreMainOperations.CreateCardAttackHandle(use.Id, use.SourceSeat, targetSeat, card: null, damageAmount: _coreForeignPublicPileSlashesOperations.GetForeignPublicPileSlashBaseDamage(use.Id), playedCardKind: _coreCurrentSlashFireOffersOperations.IsCurrentSlashFireChangedUse(use) ? use.CardKind : CardKind.Slash, ignoresArmor: _coreDirectedCardPolicyQueriesOperations.HasCardArmorBypass(_coreMatchParticipantsPlayersAccess._players[use.SourceSeat], _coreMatchParticipantsPlayersAccess._players[targetSeat], _coreCurrentSlashFireOffersOperations.IsCurrentSlashFireChangedUse(use) ? use.CardKind : CardKind.Slash));
        continued.SetCardUseCausedDamage(attack.CardUseCausedDamage);
        _coreActiveNativeAttackQueriesOperations.ActiveCardAttack = continued;
        _coreNativeCardContinuationCursorsOperations.ActiveDuel = null;
        _coreNativePromptBoundaryOperations.ClearPendingDecision();
        _coreNativeSlashUseOperations.BeginSlashTargetResolution(continued);
        return true;
    }
}
