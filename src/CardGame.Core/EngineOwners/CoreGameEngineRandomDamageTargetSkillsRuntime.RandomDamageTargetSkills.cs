#nullable enable

namespace CardGame.Core.EngineOwners;
internal sealed partial class CoreGameEngineRandomDamageTargetSkillsRuntime
{
    [ProgramTriggerLegality(ProgramTriggerLegalityPhase.BeforeGainQualification, 1242)]
    internal bool CanOfferRandomDamageTargetSkill(SkillProgramTrigger trigger, ProgramSkillWindowContext context, int ownerSeat)
    {
        if (!AcquireRandomDamageTargetSkillDescriptor.IsTrigger(trigger))
            return true;
        if (context is not { Window: SkillProgramTriggerWindow.AfterDamageApplied, Amount: > 0, SourceSeat: { } source, TargetSeat: { } target } || source != ownerSeat || target == ownerSeat || _resolutionStackOperations._resolutionStack.OfType<DamageTriggerWindowFrame>().SingleOrDefault(w => w.Id == context.ParentFrameId)is not { } window)
            return false;
        return TryGetRandomSkillDirectSlashUse(_coreNativeDamageHandlesOperations.Value.GetDamageTriggerAttack(window), out _);
    }

    internal bool TryGetRandomSkillDirectSlashUse(GameEngine.IDamageAttempt attack, out CardUseFrame use)
    {
        use = null !;
        if (attack.IsSourceLess || attack.IsChainPropagation || !attack.DamageWasApplied || attack.DamageAmount <= 0 || attack.SourceSeat == attack.TargetSeat || attack.SourceSeat != attack.CardUserSeat || attack.EffectiveCardKind is not { } kind || !CoreNativeCardClassificationsRuntime.IsSlashCard(kind) || _coreCardUseLifecycleOperations.LifecycleCardUse(attack.ResolutionId)is not { Action: { Type: CardActionType.Use } action } original || original.SourceSeat != attack.SourceSeat || action.ActorSeat != attack.SourceSeat || original.CardKind != kind || action.EffectiveKind != kind || !original.TargetSeats.Contains(attack.TargetSeat))
            return false;
        use = original;
        return true;
    }

    internal HashSet<string> OwnedAndRestorableRandomSkillIds(CharacterState owner)
    {
        var ids = owner.SkillGrants.Grants.Select(g => g.SkillId).ToHashSet(StringComparer.Ordinal);
        // A temporarily removed grant is still owned; a later restoration must not create a duplicate instance.
        foreach (var lease in _coreGameEngineOrderedPrintedSkillLossOrderedPrintedSkillLossLeasesAccess._orderedPrintedSkillLossLeases.Where(l => l.Source.OwnerSeat == owner.Seat))
            foreach (var grant in lease.RemovedGrants)
            {
                var general = lease.TemplateSourceId == CharacterState.SecondarySkillSource ? owner.SecondaryGeneral : owner.General;
                var loss = _coreProgramEventHistoryOperations.Value.ProgramEventHistory<OrderedPrintedSkillGrantRemovedEvent>().Single(e => e.FrameId == lease.ProgramFrameId && e.Grant == grant);
                if (general?.Id == lease.GeneralId && !_coreAlcoholSlashSkillReplacementsOperations.WasGrantPermanentlyRevokedAfterLoss(owner.Seat, grant, loss) && !RandomDamageTargetSkillGrantExpired(grant))
                    ids.Add(grant.SkillId);
            }

        foreach (var loss in _coreHellSoulCardsOperations.HellSoulOutstandingLosses(owner.Seat))
            if (owner.General.Id == loss.GeneralId && owner.SecondaryGeneral?.Id == loss.SecondaryGeneralId && !_coreAlcoholSlashSkillReplacementsOperations.WasGrantPermanentlyRevokedAfterLoss(owner.Seat, loss.Grant, loss) && loss.Grant.PhaseExpiry is null && !RandomDamageTargetSkillGrantExpired(loss.Grant))
                ids.Add(loss.Grant.SkillId);
        return ids;
    }

    internal bool RandomDamageTargetSkillGrantExpired(SkillGrant grant) => _coreNextActualTurnStartSkillGrantsOperations.HasNextActualTurnStartSkillGrantExpired(grant) || grant.TurnExpiry is { } turn && _coreProgramEventHistoryOperations.Value.ProgramEventHistory<TurnEndedEvent>().Any(e => e.TurnNumber == turn.TurnNumber && e.ActorSeat == turn.TurnOwnerSeat);
    internal SkillProgramStepOutcome AcquireRandomDamageTargetSkill(ProgramSkillFrame supplied)
    {
        var f = _coreProgramLifecycleOperations.GetActiveProgramFrame(supplied.Id);
        if (_resolutionStackOperations._resolutionStack.LastOrDefault()?.Id != f.Id || f.RandomDamageTargetSkill is not null || f.InstructionIndex != 1 || !AcquireRandomDamageTargetSkillDescriptor.IsTrigger(_coreProgramLifecycleOperations.GetProgramTrigger(f)) || !_coreOrderedPrintedSkillLossObserversOperations.ExactOrderedPrintedSkillLossParent(f, out var window, out var damage, out var attack) || attack.SourceSeat != f.OwnerSeat || !TryGetRandomSkillDirectSlashUse(attack, out var use))
            throw new InvalidOperationException("Random skill acquisition requires its exact already-paid direct native Slash damage candidate.");
        var owner = _coreMatchParticipantsPlayersAccess._players[f.OwnerSeat];
        if (!owner.IsAlive || !_coreNationalSkillsOperations.HasRuntimeSkillInstance(owner, f.SkillId, f.SkillInstanceId))
            return SkillProgramStepOutcome.Continue;
        var owned = OwnedAndRestorableRandomSkillIds(owner);
        var candidates = _coreNationalSkillsOperations.EnabledContentSkillIds(_coreMatchParticipantsPlayersAccess._players[damage.TargetSeat]).Where(id => !owned.Contains(id) && !_contentRegistryOperations._contentRegistry.GetSkill(id).Tags.HasFlag(SkillTag.Locked)).Distinct(StringComparer.Ordinal).Order(StringComparer.Ordinal).ToArray();
        if (candidates.Length == 0)
            return SkillProgramStepOutcome.Continue;
        var before = _randomOperations._random.State;
        var selected = candidates[_randomOperations._random.Next(candidates.Length)];
        var sourceId = RandomDamageTargetSkillGrantSource(f);
        var grantId = sourceId + ":" + selected;
        var receipt = new ProgramRandomDamageTargetSkillReceipt
        {
            ProgramFrameId = f.Id,
            InstructionIndex = f.InstructionIndex,
            Source = new(f.SkillId, CoreGameEngineProgramLifecycleRuntime.GetProgramBindingId(f), f.OwnerSeat, f.SkillInstanceId),
            GameplayHash = f.GameplayHash,
            DamageWindowId = window.Id,
            DamageFrameId = damage.Id,
            CardUseFrameId = use.Id,
            CardActionId = use.Action!.ActionId,
            TargetSeat = damage.TargetSeat,
            DamageAmount = damage.Amount,
            Nature = damage.Nature,
            Candidates = candidates,
            SelectedSkillId = selected,
            RandomBefore = before,
            RandomAfter = _randomOperations._random.State,
            GrantedGrant = new SkillGrant(grantId, selected, grantId, sourceId)
        };
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { RandomDamageTargetSkill = receipt });
        foreach (var player in _coreMatchParticipantsPlayersAccess._players)
            _observedSkillGrantRevisionsOperations._observedSkillGrantRevisions.TryAdd(player.Seat, player.SkillGrants.GrantRevision);
        if (!owner.SkillGrants.Grant(receipt.GrantedGrant))
            throw new InvalidOperationException("Random damage-target skill acquisition cannot issue the same grant twice.");
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new ProgramRandomDamageTargetSkillAcquiredEvent(receipt));
        _coreConversionStatesOperations.RegisterTaggedConversionSkill(owner, selected);
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new SkillsAcquiredEvent(owner.Seat, f.SkillId, Array.AsReadOnly(new[] { selected })));
        _coreRuntimeOperations.AdvanceRuntimeProgram(f.Id);
        return SkillProgramStepOutcome.AwaitChild;
    }

    internal static string RandomDamageTargetSkillGrantSource(ProgramSkillFrame f) => RuntimeSkills.RuntimeRules_Core_RandomDamageTargetSkills.RandomDamageTargetSkillGrantSource(f);
    internal bool ValidRandomDamageTargetSkillReceipt(ProgramSkillFrame f)
    {
        if (f.RandomDamageTargetSkill is not { } r || r.ProgramFrameId != f.Id || r.InstructionIndex != 1 || f.InstructionIndex != 1 || f.SelectedCardIds.Count != 0 || f.SelectedTargetSeats.Count != 0 || f.PendingMovementContinuation is not null || r.Source != new CardConversionSource(f.SkillId, CoreGameEngineProgramLifecycleRuntime.GetProgramBindingId(f), f.OwnerSeat, f.SkillInstanceId) || r.GameplayHash != f.GameplayHash || _contentRegistryOperations._contentRegistry.GetSkill(f.SkillId).Program?.GameplayHash != r.GameplayHash || !AcquireRandomDamageTargetSkillDescriptor.IsTrigger(_coreProgramLifecycleOperations.GetProgramTrigger(f)) || !_coreOrderedPrintedSkillLossObserversOperations.ExactOrderedPrintedSkillLossParent(f, out var window, out var damage, out var attack) || attack.SourceSeat != f.OwnerSeat || !TryGetRandomSkillDirectSlashUse(attack, out var use) || r.DamageWindowId != window.Id || r.DamageFrameId != damage.Id || r.CardUseFrameId != use.Id || r.CardActionId != use.Action!.ActionId || r.TargetSeat != damage.TargetSeat || r.DamageAmount != damage.Amount || r.Nature != damage.Nature || r.Candidates.Count == 0 || !r.Candidates.SequenceEqual(r.Candidates.Distinct(StringComparer.Ordinal).Order(StringComparer.Ordinal)) || r.Candidates.Any(id => !_contentRegistryOperations._contentRegistry.Skills.TryGetValue(id, out var skill) || skill.Tags.HasFlag(SkillTag.Locked)))
            return false;
        var replay = new DeterministicRandom(unchecked((int)r.RandomBefore));
        var selected = r.Candidates[replay.Next(r.Candidates.Count)];
        var source = RandomDamageTargetSkillGrantSource(f);
        var grantId = source + ":" + selected;
        return r.RandomBefore != 0 && r.SelectedSkillId == selected && r.RandomAfter == replay.State && r.GrantedGrant == new SkillGrant(grantId, selected, grantId, source) && _coreProgramEventHistoryOperations.Value.ProgramEventHistory<ProgramRandomDamageTargetSkillAcquiredEvent>().Where(e => e.Receipt.ProgramFrameId == f.Id).ToArray()is [var issued] && SameRandomDamageTargetSkillReceipt(r, issued.Receipt) && _coreProgramEventHistoryOperations.Value.ProgramEventHistory<ProgramBindingStartedEvent>().Count(e => e.FrameId == f.Id && e.OwnerSeat == f.OwnerSeat && e.SkillId == f.SkillId && e.BindingId == f.TriggerId && e.SkillInstanceId == f.SkillInstanceId && e.Window == SkillProgramTriggerWindow.AfterDamageApplied) == 1;
    }

    internal static bool SameRandomDamageTargetSkillReceipt(ProgramRandomDamageTargetSkillReceipt a, ProgramRandomDamageTargetSkillReceipt b) => RuntimeSkills.RuntimeRules_Core_RandomDamageTargetSkills.SameRandomDamageTargetSkillReceipt(a, b);
    [ProgramContinuation(ProgramContinuationKind.Resume, 1242)]
    internal bool ResumeRandomDamageTargetSkill(long id)
    {
        if (_resolutionStackOperations._resolutionStack.LastOrDefault()is not ProgramSkillFrame f || f.Id != id || f.RandomDamageTargetSkill is null)
            return false;
        if (_coreRecoveryReplacementOperations.TryBeginQueuedRecoveryReplacement(id, PostEventContinuation.Program) || _coreCharacterStateProgramsOperations.TryBeginCharacterStateProgramWindow(id, CharacterStateContinuation.Program) || _coreHpChangeProgramsOperations.TryBeginHpChangedProgramWindow(id, PostEventContinuation.Program) || _coreCardMovementProgramsOperations.TryBeginCardsMovedProgramWindow(id) || _coreSkillOwnershipWindowsOperations.TryBeginAdvancedSkillsChanged(id))
            return true;
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new ProgramRandomDamageTargetSkillCompletedEvent(id));
        _coreSkillProgramsOperations.FinishProgramSkill(f, true);
        return true;
    }

    [ProgramRuntimeHook(ProgramRuntimePhase.AfterOwnerDeath, 1242)]
    [ProgramRuntimeHook(ProgramRuntimePhase.AfterSkillLoss, 1242)]
    internal bool CanContinueRandomDamageTargetSkill(ProgramSkillFrame f) => f.RandomDamageTargetSkill is not null && ValidRandomDamageTargetSkillReceipt(f);

    SkillProgramStepOutcome IRandomDamageTargetSkillHost.AcquireRandomDamageTargetSkill(ProgramSkillFrame frame) => AcquireRandomDamageTargetSkill(frame);
}
