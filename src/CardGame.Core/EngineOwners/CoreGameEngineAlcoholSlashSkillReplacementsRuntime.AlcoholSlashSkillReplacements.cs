#nullable enable

namespace CardGame.Core.EngineOwners;
internal sealed partial class CoreGameEngineAlcoholSlashSkillReplacementsRuntime
{
    internal static bool IsAlcoholSlashSkillReplacementTrigger(SkillProgramTrigger trigger) => RuntimeSkills.RuntimeRules_Core_AlcoholSlashSkillReplacements.IsAlcoholSlashSkillReplacementTrigger(trigger);
    internal static string AlcoholSlashReplacementGrantSource(int owner, string skill, string instance) => RuntimeSkills.RuntimeRules_Core_AlcoholSlashSkillReplacements.AlcoholSlashReplacementGrantSource(owner, skill, instance);
    [ProgramTriggerLegality(ProgramTriggerLegalityPhase.BeforeGainQualification, 941)]
    internal bool CanOfferAlcoholSlashSkillReplacement(SkillProgramTrigger trigger, ProgramSkillWindowContext context, int ownerSeat)
    {
        if (!IsAlcoholSlashSkillReplacementTrigger(trigger))
            return true;
        if (!_coreTableSeatQueriesOperations.IsValidPlayerSeat(ownerSeat) || !_coreMatchParticipantsPlayersAccess._players[ownerSeat].IsAlive || context.OwnerSeat != ownerSeat || context is not { Window: SkillProgramTriggerWindow.AfterDamageApplied, Amount: > 0, SourceSeat: { } source } || source != ownerSeat || _resolutionStackOperations._resolutionStack.OfType<DamageTriggerWindowFrame>().SingleOrDefault(f => f.Id == context.ParentFrameId)is not { } window)
            return false;
        var attack = _coreNativeDamageHandlesOperations.GetDamageTriggerAttack(window);
        return TryGetPermanentAlcoholSlashUse(attack, out _);
    }

    internal bool TryGetPermanentAlcoholSlashUse(GameEngine.IDamageAttempt attack, out CardUseFrame use)
    {
        use = null !;
        if (attack.IsSourceLess || !attack.DamageWasApplied || attack.DamageAmount <= 0 || attack.SourceSeat != attack.CardUserSeat || attack.EffectiveCardKind is not { } kind || !CoreNativeCardClassificationsRuntime.IsSlashCard(kind) || _coreCardUseLifecycleOperations.LifecycleCardUse(attack.ResolutionId)is not { ConsumedAlcoholBoost: true, ConsumedAlcoholBonus: > 0, Action: { Type: CardActionType.Use } action } original || action.ActorSeat != attack.SourceSeat || original.SourceSeat != attack.SourceSeat || original.CardKind != kind || action.EffectiveKind != kind)
            return false;
        if (_coreProgramEventHistoryOperations.ProgramEventHistory<CardUseAlcoholConsumptionCapturedEvent>().Where(e => e.CardUseFrameId == original.Id).ToArray()is not [var fact] || fact.ActionId != action.ActionId || fact.ActorSeat != attack.SourceSeat || fact.AlcoholBonus != original.ConsumedAlcoholBonus)
            return false;
        use = original;
        return true;
    }

    internal SkillProgramStepOutcome ReplaceOwnSkillsAfterAlcoholSlashDamage(ProgramSkillFrame supplied)
    {
        var f = _coreProgramLifecycleOperations.GetActiveProgramFrame(supplied.Id);
        if (_resolutionStackOperations._resolutionStack.LastOrDefault()?.Id != f.Id || f.AlcoholSlashSkillReplacement is not null || f.InstructionIndex != 1 || !IsAlcoholSlashSkillReplacementTrigger(_coreProgramLifecycleOperations.GetProgramTrigger(f)) || !_coreMatchParticipantsPlayersAccess._players[f.OwnerSeat].IsAlive || !_coreNationalSkillsOperations.HasRuntimeSkillInstance(_coreMatchParticipantsPlayersAccess._players[f.OwnerSeat], f.SkillId, f.SkillInstanceId) || !_coreOrderedPrintedSkillLossObserversOperations.ExactOrderedPrintedSkillLossParent(f, out var window, out var damage, out var attack) || attack.SourceSeat != f.OwnerSeat || !TryGetPermanentAlcoholSlashUse(attack, out var use))
            throw new InvalidOperationException("Permanent Alcohol Slash replacement requires the exact live source and already paid native damage segment.");
        var effect = _coreProgramLifecycleOperations.GetProgramTrigger(f).Effects.Single();
        var owner = _coreMatchParticipantsPlayersAccess._players[f.OwnerSeat];
        var sourceId = AlcoholSlashReplacementGrantSource(f.OwnerSeat, f.SkillId, f.SkillInstanceId);
        var grantId = sourceId + ":" + effect.SourceBind;
        var existing = owner.SkillGrants.Grants.SingleOrDefault(g => g.GrantId == grantId);
        var removed = owner.SkillGrants.Grants.Where(g => !g.IsModeRule && effect.SkillIds.Contains(g.SkillId, StringComparer.Ordinal)).ToArray();
        var pendingOrdered = _coreGameEngineOrderedPrintedSkillLossOrderedPrintedSkillLossLeasesAccess._orderedPrintedSkillLossLeases.Where(l => l.Source.OwnerSeat == f.OwnerSeat).SelectMany(lease => lease.RemovedGrants.Where(g => !g.IsModeRule && effect.SkillIds.Contains(g.SkillId, StringComparer.Ordinal)).Where(g => !WasGrantPermanentlyRevokedAfterLoss(f.OwnerSeat, g, _coreAlternatingBenefitsOperations.CompleteProgramEventHistory().OfType<OrderedPrintedSkillGrantRemovedEvent>().Single(e => e.FrameId == lease.ProgramFrameId && e.Grant == g))));
        var pendingHell = _coreHellSoulCardsOperations.HellSoulOutstandingLosses(f.OwnerSeat).Where(loss => !loss.Grant.IsModeRule && effect.SkillIds.Contains(loss.Grant.SkillId, StringComparer.Ordinal) && !WasGrantPermanentlyRevokedAfterLoss(f.OwnerSeat, loss.Grant, loss)).Select(loss => loss.Grant);
        var revoked = removed.Concat(pendingOrdered).Concat(pendingHell).DistinctBy(g => (g.GrantId, g.SkillId, g.SkillInstanceId, g.SourceId)).ToArray();
        // Later chain recipients share the original paid use but do not issue an already completed exchange again.
        if (existing is not null && revoked.Length == 0)
            return SkillProgramStepOutcome.Continue;
        var priorIssuance = _coreProgramEventHistoryOperations.ProgramEventHistory<ProgramAlcoholSlashSkillsReplacedEvent>().Select(e => e.Receipt).FirstOrDefault(r => r.Source.OwnerSeat == f.OwnerSeat && r.GrantAdded && r.GrantedGrant.GrantId == grantId);
        var shared = existing is null ? _coreNationalSkillsOperations.GetSkillBindingShard(owner).ActiveGrants.Where(g => g.SkillId == effect.SourceBind).OrderBy(g => g.SourceId.StartsWith("turn:", StringComparison.Ordinal) ? 1 : 0).ThenBy(g => g.SkillInstanceId, StringComparer.Ordinal).FirstOrDefault() : priorIssuance?.SharedInstanceGrant;
        var instance = shared?.SkillInstanceId ?? grantId;
        var granted = existing ?? new SkillGrant(grantId, effect.SourceBind!, instance, sourceId);
        if (granted.SkillId != effect.SourceBind || granted.SkillInstanceId != instance || granted.SourceId != sourceId || granted.IsModeRule || granted.TurnExpiry is not null || granted.PhaseExpiry is not null || granted.NextActualTurnStartExpiry is not null)
            throw new InvalidOperationException("A permanent Alcohol Slash grant changed its independently owned source.");
        var r = new AlcoholSlashSkillReplacementReceipt
        {
            ProgramFrameId = f.Id,
            InstructionIndex = f.InstructionIndex,
            Source = new(f.SkillId, CoreGameEngineProgramLifecycleRuntime.GetProgramBindingId(f), f.OwnerSeat, f.SkillInstanceId),
            GameplayHash = f.GameplayHash,
            DamageWindowId = window.Id,
            DamageFrameId = damage.Id,
            CardUseFrameId = use.Id,
            CardActionId = use.Action!.ActionId,
            TargetSeat = attack.TargetSeat,
            DamageAmount = damage.Amount,
            Nature = damage.Nature,
            AlcoholBonus = use.ConsumedAlcoholBonus!.Value,
            LostSkillIds = effect.SkillIds,
            RemovedGrants = removed,
            RevokedGrants = revoked,
            GrantedGrant = granted,
            SharedInstanceGrant = shared,
            GrantAdded = existing is null
        };
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { AlcoholSlashSkillReplacement = r });
        foreach (var player in _coreMatchParticipantsPlayersAccess._players)
            _observedSkillGrantRevisionsOperations._observedSkillGrantRevisions.TryAdd(player.Seat, player.SkillGrants.Revision);
        foreach (var grant in removed)
            if (!owner.SkillGrants.RemoveGrant(grant.GrantId))
                throw new InvalidOperationException("Permanent skill loss was paid twice or changed its grant.");
        if (existing is null)
            owner.SkillGrants.Grant(granted);
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new ProgramAlcoholSlashSkillsReplacedEvent(r));
        if (existing is null)
        {
            _coreConversionStatesOperations.RegisterTaggedConversionSkill(owner, granted.SkillId);
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new SkillsAcquiredEvent(owner.Seat, f.SkillId, Array.AsReadOnly(new[] { granted.SkillId })));
        }

        _coreRuntimeOperations.AdvanceRuntimeProgram(f.Id);
        return SkillProgramStepOutcome.AwaitChild;
    }

    internal bool ValidAlcoholSlashSkillReplacementReceipt(ProgramSkillFrame f)
    {
        if (f.AlcoholSlashSkillReplacement is not { } r || f.InstructionIndex != 1 || r.InstructionIndex != 1 || r.ProgramFrameId != f.Id || f.SelectedCardIds.Count != 0 || f.SelectedTargetSeats.Count != 0 || f.PendingMovementContinuation is not null || r.Source != new CardConversionSource(f.SkillId, CoreGameEngineProgramLifecycleRuntime.GetProgramBindingId(f), f.OwnerSeat, f.SkillInstanceId) || r.GameplayHash != f.GameplayHash || _contentRegistryOperations._contentRegistry.GetSkill(f.SkillId).Program?.GameplayHash != r.GameplayHash || !IsAlcoholSlashSkillReplacementTrigger(_coreProgramLifecycleOperations.GetProgramTrigger(f)) || !_coreOrderedPrintedSkillLossObserversOperations.ExactOrderedPrintedSkillLossParent(f, out var window, out var damage, out var attack) || attack.SourceSeat != f.OwnerSeat || !TryGetPermanentAlcoholSlashUse(attack, out var use) || r.DamageWindowId != window.Id || r.DamageFrameId != damage.Id || r.CardUseFrameId != use.Id || r.CardActionId != use.Action!.ActionId || r.TargetSeat != damage.TargetSeat || r.DamageAmount != damage.Amount || r.Nature != damage.Nature || r.AlcoholBonus != use.ConsumedAlcoholBonus)
            return false;
        var effect = _coreProgramLifecycleOperations.GetProgramTrigger(f).Effects.Single();
        var source = AlcoholSlashReplacementGrantSource(f.OwnerSeat, f.SkillId, f.SkillInstanceId);
        var expectedGrantId = source + ":" + effect.SourceBind;
        var facts = _coreProgramEventHistoryOperations.ProgramEventHistory<ProgramAlcoholSlashSkillsReplacedEvent>().Where(e => e.Receipt.ProgramFrameId == f.Id).ToArray();
        var expectedInstance = r.SharedInstanceGrant?.SkillInstanceId ?? expectedGrantId;
        var earlierIssuance = r.GrantAdded || _coreAlternatingBenefitsOperations.CompleteProgramEventHistory().TakeWhile(e => e is not ProgramAlcoholSlashSkillsReplacedEvent issued || issued.Receipt.ProgramFrameId != f.Id).OfType<ProgramAlcoholSlashSkillsReplacedEvent>().Any(e => e.Receipt.GrantAdded && e.Receipt.Source == r.Source && SamePermanentGrantIdentity(e.Receipt.GrantedGrant, r.GrantedGrant) && e.Receipt.SharedInstanceGrant == r.SharedInstanceGrant);
        return r.LostSkillIds.SequenceEqual(effect.SkillIds) && r.RemovedGrants.All(g => !g.IsModeRule && r.LostSkillIds.Contains(g.SkillId, StringComparer.Ordinal)) && r.RevokedGrants.All(g => !g.IsModeRule && r.LostSkillIds.Contains(g.SkillId, StringComparer.Ordinal)) && r.RemovedGrants.All(g => r.RevokedGrants.Any(revoked => SamePermanentGrantIdentity(g, revoked))) && r.RevokedGrants.Select(g => (g.GrantId, g.SkillId, g.SkillInstanceId, g.SourceId)).Distinct().Count() == r.RevokedGrants.Count && r.RemovedGrants.Select(g => g.GrantId).Distinct(StringComparer.Ordinal).Count() == r.RemovedGrants.Count && (r.SharedInstanceGrant is null || r.SharedInstanceGrant.IsEnabled && r.SharedInstanceGrant.SkillId == effect.SourceBind) && earlierIssuance && (r.GrantedGrant with
        {
            IsEnabled = true
        }

        ) == new SkillGrant(expectedGrantId, effect.SourceBind!, expectedInstance, source) && (!r.GrantAdded || r.GrantedGrant.IsEnabled) && facts is [var issued] && SameAlcoholSlashReplacementReceipt(r, issued.Receipt) && _coreProgramEventHistoryOperations.ProgramEventHistory<ProgramBindingStartedEvent>().Count(e => e.FrameId == f.Id && e.OwnerSeat == f.OwnerSeat && e.SkillId == f.SkillId && e.BindingId == f.TriggerId && e.SkillInstanceId == f.SkillInstanceId && e.Window == SkillProgramTriggerWindow.AfterDamageApplied) == 1;
    }

    internal static bool SamePermanentGrantIdentity(SkillGrant left, SkillGrant right) => RuntimeSkills.RuntimeRules_Core_AlcoholSlashSkillReplacements.SamePermanentGrantIdentity(left, right);
    internal static bool SameAlcoholSlashReplacementReceipt(AlcoholSlashSkillReplacementReceipt left, AlcoholSlashSkillReplacementReceipt right) => RuntimeSkills.RuntimeRules_Core_AlcoholSlashSkillReplacements.SameAlcoholSlashReplacementReceipt(left, right);
    internal bool WasGrantPermanentlyRevokedAfterLoss(int ownerSeat, SkillGrant grant, IGameEvent originalLoss)
    {
        if (!_contentRegistryOperations._contentRegistry.ProgramDependencies.HasTriggerOperation(SkillProgramEffectOp.ReplaceOwnSkillsAfterAlcoholSlashDamage) && !_contentRegistryOperations._contentRegistry.ProgramDependencies.HasTriggerOperation(SkillProgramEffectOp.ResolvePrepPoisonOutcomeByRound) && !_contentRegistryOperations._contentRegistry.ProgramDependencies.HasActivationOperation(SkillProgramEffectOp.ResolveShenHuaTuoHuandao) && !_contentRegistryOperations._contentRegistry.ProgramDependencies.HasActivationOperation(SkillProgramEffectOp.TeachMobileShenHuaTuoWuling) && !_contentRegistryOperations._contentRegistry.ProgramDependencies.HasActivationOperation(SkillProgramEffectOp.ResolveMobileShenLuBuWuqian))
            return false;
        var history = _coreAlternatingBenefitsOperations.CompleteProgramEventHistory().ToArray();
        var lossIndex = Array.FindIndex(history, e => ReferenceEquals(e, originalLoss));
        return history.Skip(lossIndex + 1).OfType<ProgramAlcoholSlashSkillsReplacedEvent>().Any(e => e.Receipt.Source.OwnerSeat == ownerSeat && e.Receipt.RevokedGrants.Any(revoked => SamePermanentGrantIdentity(grant, revoked))) || history.Skip(lossIndex + 1).OfType<PrepPoisonGrantRevokedEvent>().Any(e => e.OwnerSeat == ownerSeat && CoreGameEngineDamageTargetSkillRoundDrawRuntime.SameRoundSkillIdentity(grant, e.Grant)) || history.Skip(lossIndex + 1).OfType<ShenHuaTuoSkillRevokedEvent>().Any(e => e.TargetSeat == ownerSeat && e.GrantId == grant.GrantId && e.SkillId == grant.SkillId && e.SkillInstanceId == grant.SkillInstanceId && e.SourceId == grant.SourceId) || history.Skip(lossIndex + 1).OfType<MobileWulingGrantRevokedEvent>().Any(e => e.TargetSeat == ownerSeat && e.GrantId == grant.GrantId && e.SkillId == grant.SkillId && e.SkillInstanceId == grant.SkillInstanceId && e.SourceId == grant.SourceId) || history.Skip(lossIndex + 1).OfType<MobileLuBuWushuangRevokedEvent>().Any(e => e.TargetSeat == ownerSeat && e.GrantId == grant.GrantId && e.SkillId == grant.SkillId && e.SkillInstanceId == grant.SkillInstanceId && e.SourceId == grant.SourceId);
    }

    [ProgramContinuation(ProgramContinuationKind.Resume, 3902)]
    internal bool ResumeAlcoholSlashSkillReplacement(long id)
    {
        if (_resolutionStackOperations._resolutionStack.LastOrDefault()is not ProgramSkillFrame f || f.Id != id || f.AlcoholSlashSkillReplacement is null)
            return false;
        if (_coreRecoveryReplacementOperations.TryBeginQueuedRecoveryReplacement(id, PostEventContinuation.Program) || _coreCharacterStateProgramsOperations.TryBeginCharacterStateProgramWindow(id, CharacterStateContinuation.Program) || _coreHpChangeProgramsOperations.TryBeginHpChangedProgramWindow(id, PostEventContinuation.Program) || _coreCardMovementProgramsOperations.TryBeginCardsMovedProgramWindow(id) || _coreSkillOwnershipWindowsOperations.TryBeginAdvancedSkillsChanged(id))
            return true;
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new ProgramAlcoholSlashSkillReplacementCompletedEvent(id));
        _coreSkillProgramsOperations.FinishProgramSkill(f, true);
        return true;
    }

    [ProgramRuntimeHook(ProgramRuntimePhase.AfterOwnerDeath, 3902)]
    [ProgramRuntimeHook(ProgramRuntimePhase.AfterSkillLoss, 3902)]
    internal bool CanContinueAlcoholSlashSkillReplacement(ProgramSkillFrame f) => f.AlcoholSlashSkillReplacement is not null && ValidAlcoholSlashSkillReplacementReceipt(f);

    SkillProgramStepOutcome IAlcoholSlashSkillReplacementProgramHost.ReplaceOwnSkillsAfterAlcoholSlashDamage(ProgramSkillFrame frame) => ReplaceOwnSkillsAfterAlcoholSlashDamage(frame);
}
