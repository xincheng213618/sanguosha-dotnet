#nullable enable

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterCaoXiOlRuntime
{
    internal bool HasCaoXiGangshuCapability => _contentRegistryOperations._contentRegistry.ProgramDependencies.HasTriggerOperation(SkillProgramEffectOp.ResolveGangshuAttributeIncrement);
    internal bool HasCaoXiCapability => HasCaoXiGangshuCapability || _contentRegistryOperations._contentRegistry.ProgramDependencies.HasTriggerOperation(SkillProgramEffectOp.ResolveJianxuanRepeatedDraw);

    internal IReadOnlyList<CaoXiGangshuIncreasedEvent> ActiveCaoXiGangshuUnits()
    {
        if (!HasCaoXiGangshuCapability)
            return[];
        var history = _coreAlternatingBenefitsOperations.CompleteProgramEventHistory().ToArray();
        var tail = history.LastOrDefault();
        if (_characterCaoXiOlCaoXiActiveUnitsCaoXiHistoryCountCaoXiHistoryTailAccess._caoXiHistoryCount == history.Length && ReferenceEquals(_characterCaoXiOlCaoXiActiveUnitsCaoXiHistoryCountCaoXiHistoryTailAccess._caoXiHistoryTail, tail))
            return _characterCaoXiOlCaoXiActiveUnitsCaoXiHistoryCountCaoXiHistoryTailAccess._caoXiActiveUnits;
        var expired = history.OfType<CaoXiGangshuUnitExpiredEvent>().Select(e => e.GrantFrameId).ToHashSet();
        _characterCaoXiOlCaoXiActiveUnitsCaoXiHistoryCountCaoXiHistoryTailAccess._caoXiActiveUnits = Array.AsReadOnly(history.OfType<CaoXiGangshuIncreasedEvent>().Where(e => !expired.Contains(e.FrameId)).ToArray());
        _characterCaoXiOlCaoXiActiveUnitsCaoXiHistoryCountCaoXiHistoryTailAccess._caoXiHistoryTail = tail;
        _characterCaoXiOlCaoXiActiveUnitsCaoXiHistoryCountCaoXiHistoryTailAccess._caoXiHistoryCount = history.Length;
        return _characterCaoXiOlCaoXiActiveUnitsCaoXiHistoryCountCaoXiHistoryTailAccess._caoXiActiveUnits;
    }

    internal bool IsCaoXiDrawUnitEligible(CaoXiGangshuIncreasedEvent unit, ActualDiscardRecoveryPhaseKey? phase)
    {
        if (unit.Query != SkillRuleQuery.DrawCount)
            return false;
        if (phase is not { Kind: ActualDiscardRecoveryPhaseKind.Draw } draw)
            return true;
        return unit.GrantedAfterActualPhaseToken < draw.Token && unit.GrantedInDrawPhaseToken != draw.Token && !_coreAlternatingBenefitsOperations.CompleteProgramEventHistory().OfType<CaoXiGangshuDrawUnitIssuedEvent>().Any(e => e.GrantFrameId == unit.FrameId && e.Phase.Token != draw.Token);
    }

    internal int CaoXiGangshuBonus(CharacterState player, SkillRuleQuery query)
    {
        if (!HasCaoXiGangshuCapability || !player.IsAlive || query is not (SkillRuleQuery.AttackRange or SkillRuleQuery.DrawCount or SkillRuleQuery.SlashLimit))
            return 0;
        var phase = _coreActualDiscardRecoveryPhasesOperations.CurrentActualDiscardRecoveryPhase();
        return ActiveCaoXiGangshuUnits().Count(e => e.Source.OwnerSeat == player.Seat && e.Query == query && (query != SkillRuleQuery.DrawCount || IsCaoXiDrawUnitEligible(e, phase)));
    }

    internal CaoXiNumbers CurrentCaoXiNumbers(int seat)
    {
        var player = _coreMatchParticipantsPlayersAccess._players[seat];
        var draw = _coreActualTurnsOperations.GetTurnDrawCount(player);
        // The recorded next-draw number includes future units acquired during an
        // already running Draw, while that phase's native count excludes them.
        var allDrawUnits = player.IsAlive ? ActiveCaoXiGangshuUnits().Count(e => e.Source.OwnerSeat == seat && e.Query == SkillRuleQuery.DrawCount) : 0;
        return new(_coreNativeRuleQueriesOperations.GetAttackRange(seat), checked(draw + allDrawUnits - CaoXiGangshuBonus(player, SkillRuleQuery.DrawCount)), _coreRuleQueriesOperations.GetSlashUseLimit(player));
    }

    internal IReadOnlyList<CaoXiGangshuIncreasedEvent> FreezeCaoXiGangshuDrawUse(CharacterState player)
    {
        if (!HasCaoXiGangshuCapability || !player.IsAlive || _coreActualDiscardRecoveryPhasesOperations.CurrentActualDiscardRecoveryPhase()is not { Kind: ActualDiscardRecoveryPhaseKind.Draw } phase || phase.ActorSeat != player.Seat || !_coreActualDiscardRecoveryPhasesOperations.IsRecordedActualDiscardRecoveryPhase(phase))
            return[];
        return Array.AsReadOnly(ActiveCaoXiGangshuUnits().Where(e => e.Source.OwnerSeat == player.Seat && IsCaoXiDrawUnitEligible(e, phase)).ToArray());
    }

    internal void CaptureCaoXiGangshuDrawUse(CharacterState player, IReadOnlyList<CaoXiGangshuIncreasedEvent> frozen)
    {
        if (!HasCaoXiGangshuCapability || frozen.Count == 0)
            return;
        if (_coreActualDiscardRecoveryPhasesOperations.CurrentActualDiscardRecoveryPhase()is not { Kind: ActualDiscardRecoveryPhaseKind.Draw } phase || phase.ActorSeat != player.Seat || !player.IsAlive || !_coreActualDiscardRecoveryPhasesOperations.IsRecordedActualDiscardRecoveryPhase(phase) || frozen.Select(e => e.FrameId).Distinct().Count() != frozen.Count || frozen.Any(e => e.Source.OwnerSeat != player.Seat || e.Query != SkillRuleQuery.DrawCount || !ActiveCaoXiGangshuUnits().Contains(e) || !IsCaoXiDrawUnitEligible(e, phase)))
            throw new InvalidOperationException("Gangshu draw usage requires the exact live actual-phase token and frozen attributable units.");
        var old = _coreAlternatingBenefitsOperations.CompleteProgramEventHistory().OfType<CaoXiGangshuDrawUnitIssuedEvent>().Where(e => e.Phase == phase).ToArray();
        if (old.Length != 0)
        {
            if (!old.Select(e => e.GrantFrameId).Order().SequenceEqual(frozen.Select(e => e.FrameId).Order()))
                throw new InvalidOperationException("A frozen actual draw cannot replace its original Gangshu units.");
            return;
        }

        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new CaoXiGangshuDrawCaptureEvent(phase, player.Seat));
        foreach (var unit in frozen)
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new CaoXiGangshuDrawUnitIssuedEvent(phase, unit.FrameId, unit.Source));
    }

    internal void ExpireCaoXiGangshuUnits(int seat, string reason, long nativeId, Func<CaoXiGangshuIncreasedEvent, bool>? select = null)
    {
        foreach (var unit in ActiveCaoXiGangshuUnits().Where(e => e.Source.OwnerSeat == seat && (select is null || select(e))).ToArray())
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new CaoXiGangshuUnitExpiredEvent(unit.FrameId, seat, reason, nativeId));
    }

    internal void CaptureCaoXiGangshuSlashOffset(NativeOwners.CardAttackHandle attack)
    {
        if (!HasCaoXiGangshuCapability || attack.EffectiveCardKind is not { } kind || !CoreNativeCardClassificationsRuntime.IsSlashCard(kind) || attack.SuccessfulDodgeResponses != attack.RequiredDodgeResponses || _coreSlashSuitDiscardOperations.Value.IsSlashDodgeCancellationPrevented(attack) || !_coreTableSeatQueriesOperations.IsValidPlayerSeat(attack.TargetSeat))
            return;
        var cursor = _coreCardUseLifecycleOperations.LifecycleCardUse(attack.ResolutionId)?.TargetIndex ?? 0;
        if (_coreAlternatingBenefitsOperations.CompleteProgramEventHistory().OfType<CaoXiGangshuActualOffsetEvent>().Any(e => e.NativeId == attack.ResolutionId && e.OwnerSeat == attack.TargetSeat && e.Kind == "slash" && e.Cursor == cursor))
            return;
        // This helper is called only from the real successful native Slash-Dodge
        // return, never from a generic response or an armor immunity event.
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new CaoXiGangshuActualOffsetEvent(attack.ResolutionId, attack.TargetSeat, "slash", cursor));
        ExpireCaoXiGangshuUnits(attack.TargetSeat, "actual-slash-offset", attack.ResolutionId);
    }

    internal void CaptureCaoXiNativeCounterspellOffset(NullificationWindowFrame pending, CardActionContext action, bool invalidated)
    {
        if (!HasCaoXiGangshuCapability)
            return;
        if (_resolutionStackOperations._resolutionStack.LastOrDefault()is not NullificationWindowFrame current || current.Id != pending.Id || current.ParentFrameId != pending.ParentFrameId || current.ChainDepth != pending.ChainDepth || current.EffectNullified != pending.EffectNullified || pending.ChainDepth < 1 || pending.NamedAmbushResponse is not null || action.Type != CardActionType.Response || action.EffectiveKind != CardKind.Nullification || action.ActorSeat != action.ProviderSeat || action.ResponderSeat != action.ActorSeat || action.RequesterSeat is not null || action.OpponentSeat != pending.SourceSeat || action.ParentActionId != _coreDelayedNullificationOperations.Value.NullificationParentActionId(pending) || !_coreAlternatingBenefitsOperations.CompleteProgramEventHistory().OfType<CardActionAcceptedEvent>().Any(e => e.Action.ActionId == action.ActionId && CoreGameEngineOverflowTargetCancellationObserversRuntime.SameOverflowAction(e.Action, action)))
            throw new InvalidOperationException("Gangshu offset requires the exact accepted native counterspell node after its real invalidation decision.");
        if (invalidated || pending.ChainDepth == 1 && _coreCurrentCardEnhancementsOperations.HasCurrentCardEnhancement(pending.ParentFrameId, CurrentCardEnhancement.Uncancelable) || _coreAlternatingBenefitsOperations.CompleteProgramEventHistory().OfType<CaoXiGangshuActualOffsetEvent>().Any(e => e.NativeId == pending.Id && e.OwnerSeat == action.ActorSeat && e.Kind == "nullification" && e.Cursor == pending.ChainDepth))
            return;
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new CaoXiGangshuActualOffsetEvent(pending.Id, action.ActorSeat, "nullification", pending.ChainDepth));
        ExpireCaoXiGangshuUnits(action.ActorSeat, "actual-nullification-offset", pending.Id);
    }

    [ProgramRuleObserver(ProgramRulePhase.BeforeEvent, 26810)]
    internal void ObserveCaoXiGangshuExpiry(IGameEvent payload)
    {
        if (!HasCaoXiGangshuCapability)
            return;
        if (payload is ActualDrawPhaseCompletedEvent draw && !draw.Skipped && _coreActualDiscardRecoveryPhasesOperations.CurrentActualDiscardRecoveryPhase()is { Kind: ActualDiscardRecoveryPhaseKind.Draw } phase && phase.ActorSeat == draw.OwnerSeat && phase.TurnNumber == draw.ActualTurnNumber)
        {
            var used = _coreAlternatingBenefitsOperations.CompleteProgramEventHistory().OfType<CaoXiGangshuDrawUnitIssuedEvent>().Where(e => e.Phase == phase).Select(e => e.GrantFrameId).ToHashSet();
            ExpireCaoXiGangshuUnits(draw.OwnerSeat, "actual-draw-completed", draw.FrameId, e => e.Query == SkillRuleQuery.DrawCount && used.Contains(e.FrameId));
        }
        else if (payload is PlayerDiedEvent death)
            ExpireCaoXiGangshuUnits(death.VictimSeat, "owner-death", death.VictimSeat);
    }

    [ProgramDistanceRule(ProgramDistanceStage.AttackRangeContributions, 20)]
    internal void AddCaoXiGangshuBonusAttackRangeContribution(CharacterState player, List<RuleQueryContribution> contributions) => CoreGameEngineRuleQueriesRuntime.AddFiniteContribution(contributions, $"state:{(player.Seat)}:cao-xi:gangshu:attack-range", CaoXiGangshuBonus(player, SkillRuleQuery.AttackRange));
}
