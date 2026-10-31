#nullable enable

using System.Globalization;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CoreGameEngineDamageZoneAcquisitionRuntime
{
    internal const string DamageAcquisitionReason = "skill-program.damage-zone-acquisition.obtain";
    internal CardConversionSource DamageAcquisitionSource(ProgramSkillFrame f) => new(f.SkillId, CoreGameEngineProgramLifecycleRuntime.GetProgramBindingId(f), f.OwnerSeat, f.SkillInstanceId);
    [ProgramTriggerLegality(ProgramTriggerLegalityPhase.BeforeGainQualification, 820)]
    internal bool CanOfferDamageZoneAndAdjacentTarget(ProgramTriggerCandidate candidate, SkillProgramTrigger trigger, ProgramSkillWindowContext context)
    {
        if (trigger.Effects is not [var effect])
            return true;
        if (effect.Op == SkillProgramEffectOp.JoinAdjacentRedCardTarget)
            return _coreAdjacentRedTargetsOperations.TryGetAdjacentRedTargetUse(candidate.OwnerSeat, context, out var use, out _) && _coreAdjacentRedTargetsOperations.AdjacentRedTargetEligible(use, candidate.OwnerSeat) && !_coreAlternatingBenefitsOperations.CompleteProgramEventHistory().OfType<AdjacentRedTargetAddedEvent>().Any(e => e.CardUseFrameId == use.Id && e.Source.OwnerSeat == candidate.OwnerSeat && e.Source.SkillId == candidate.SkillId);
        if (effect.Op != SkillProgramEffectOp.ObtainEachDamageTargetZoneThenSelfOnly)
            return true;
        return _coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess._phase == TurnPhase.Play && _coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess._currentSeat == candidate.OwnerSeat && context.Window == SkillProgramTriggerWindow.AfterDamageApplied && context.Amount > 0 && context.DamageFrameId is not null && context.SourceSeat == candidate.OwnerSeat && context.TargetSeat is { } victim && victim != candidate.OwnerSeat && _coreTableSeatQueriesOperations.IsValidPlayerSeat(victim) && !_coreAlternatingBenefitsOperations.CompleteProgramEventHistory().OfType<DamageZoneAcquisitionStartedEvent>().Any(e => e.Receipt.Source.OwnerSeat == candidate.OwnerSeat && e.Receipt.Source.SkillId == candidate.SkillId && e.Receipt.ActualTurn == _coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess._turnNumber);
    }

    internal bool ExactDamageAcquisitionParent(ProgramSkillFrame f)
    {
        if (f.TriggerId is null || f.InstructionIndex != 1 || f.WindowContext is not { Window: SkillProgramTriggerWindow.AfterDamageApplied, Amount: > 0, SourceSeat: { } source, TargetSeat: { } target, DamageFrameId: { } damageId } c || source != f.OwnerSeat || target == source || _coreProgramLifecycleOperations.GetProgramTrigger(f).Effects is not [{ Op: SkillProgramEffectOp.ObtainEachDamageTargetZoneThenSelfOnly }])
            return false;
        var i = _resolutionStackOperations._resolutionStack.FindIndex(x => x.Id == f.Id);
        return i > 0 && _resolutionStackOperations._resolutionStack[i - 1] is DamageTriggerWindowFrame w && w.Id == c.ParentFrameId && w.ParentFrameId == damageId && w.TriggerWindow == c.Window && w.SourceSeat == source && w.TargetSeat == target && w.CandidateIndex >= 0 && w.CandidateIndex < w.Candidates.Count && CoreGameEngineDamageTargetMountObserverRuntime.MountObserverCandidateMatches(f, w.Candidates[w.CandidateIndex].ToProgramCandidate()) && _resolutionStackOperations._resolutionStack.OfType<DamageFrame>().SingleOrDefault(d => d.Id == damageId)is { } damage && damage.SourceSeat == source && damage.TargetSeat == target && damage.Amount == c.Amount && DamageAcquisitionHasExactOwningAttack(damage, w);
    }

    internal bool DamageAcquisitionHasExactOwningAttack(DamageFrame damage, DamageTriggerWindowFrame window)
    {
        // A paid gain observer may issue its own damage and dying child. The
        // global current attack then belongs to that child; the original damage
        // remains suspended on its own typed frame and must be proved there.
        GameEngine.IDamageAttempt? attack = _resolutionStackOperations._resolutionStack.SingleOrDefault(x => x.Id == damage.ParentFrameId) switch
        {
            ProgramSkillFrame { AttackAttempt: not null } => _kernelCoreGameEngineProgramAttackAttemptOperations.CreateProgramAttackHandle(damage.ParentFrameId),
            CardUseFrame { CardAttack: not null } or ProgramSkillFrame { CardAttack: not null } or JudgmentFrame { CardAttack: not null } => _kernelCoreMainOperations.CreateCardAttackHandle(damage.ParentFrameId),
            _ => null
        };
        return attack is { IsSourceLess: false, DamageWasApplied: true } && attack.SourceSeat == damage.SourceSeat && attack.TargetSeat == damage.TargetSeat && attack.DamageAmount == damage.Amount && window.SourceCardId == attack.Card?.Id && window.SourceCard == attack.EffectiveCardKind;
    }

    internal SkillProgramStepOutcome BeginDamageZoneAcquisition(ProgramSkillFrame input)
    {
        var f = _coreProgramLifecycleOperations.GetActiveProgramFrame(input.Id);
        if (!ExactDamageAcquisitionParent(f) || f.DamageZoneAcquisition is not null)
            throw new InvalidOperationException("Damage-zone acquisition requires its exact actual damage candidate once.");
        var r = new DamageZoneAcquisitionReceipt
        {
            WindowId = f.WindowContext!.ParentFrameId,
            DamageId = f.WindowContext.DamageFrameId!.Value,
            Source = DamageAcquisitionSource(f),
            GameplayHash = f.GameplayHash,
            ActualTurn = _coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess._turnNumber,
            VictimSeat = f.WindowContext.TargetSeat!.Value,
            Stage = DamageZoneAcquisitionStage.Selecting
        };
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { DamageZoneAcquisition = r });
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new DamageZoneAcquisitionStartedEvent(f.Id, r));
        PublishDamageAcquisitionChoice(f);
        return SkillProgramStepOutcome.AwaitChoice;
    }

    internal IReadOnlyList<PromptChoice> DamageAcquisitionChoices(ProgramSkillFrame f) => f.DamageZoneAcquisition is { NextZone: < 3 } r && _coreMatchParticipantsPlayersAccess._players[r.VictimSeat].IsAlive ? Array.AsReadOnly(_coreProgramCardPaymentsOperations.BuildOwnedCardPaymentChoices(f.Id, f.OwnerSeat, r.VictimSeat, [CoreGameEngineDamageZoneAcquisitionDefinitions.DamageAcquisitionZones[r.NextZone]], OwnedCardMoveIntent.Obtain).Select(c => PromptChoiceProjection.Freeze(c with { Parameters = new Dictionary<string, string>(c.Parameters) { ["program-action"] = "damage-zone-acquisition" } })).ToArray()) : [];
    internal void PublishDamageAcquisitionChoice(ProgramSkillFrame f)
    {
        var r = f.DamageZoneAcquisition!;
        while (r.NextZone < 3)
        {
            var choices = DamageAcquisitionChoices(f);
            if (choices.Count > 0)
            {
                _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { DamageZoneAcquisition = r with { Choices = choices } });
                var skill = _contentRegistryOperations._contentRegistry.GetSkill(f.SkillId);
                _corePendingDecisionPolicyPipelineOperations._pendingDecision = new(DecisionKind.ProgramTrigger, f.OwnerSeat, "雉盗：从此区域选择一张牌获得。", choices.SelectMany(c => c.Cards).Distinct().ToArray(), [r.VictimSeat], f.OwnerSeat)
                {
                    PromptId = _coreNativePromptBoundaryOperations.CreatePromptId(),
                    IsPrivate = true,
                    TargetSeat = r.VictimSeat,
                    Choices = choices,
                    SkillPrompt = new(f.SkillId, skill.Name, skill.Name, skill.Description)
                };
                _coreMatchOutcomeStatusAccess._status = _coreMatchParticipantsPlayersAccess._players[f.OwnerSeat].IsHuman ? EngineStatus.AwaitingHumanResponse : EngineStatus.Running;
                _kernelCoreGameEngineRuleProgressionOperations.AdvanceRulesAndPublishState();
                return;
            }

            r = r with
            {
                NextZone = r.NextZone + 1
            };
            _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { DamageZoneAcquisition = r });
        }

        IssueDamageZoneAcquisition(f);
    }

    [ProgramAnswer("damage-zone-acquisition")]
    internal void ResolveDamageAcquisitionChoice(PromptChoice selected)
    {
        var f = _resolutionStackOperations._resolutionStack.LastOrDefault() as ProgramSkillFrame ?? throw new InvalidOperationException("Damage acquisition lost its owning program.");
        RequireDamageAcquisitionReceipt(f);
        var r = f.DamageZoneAcquisition!;
        if (r.Stage != DamageZoneAcquisitionStage.Selecting || _corePendingDecisionPolicyPipelineOperations._pendingDecision?.PlayerSeat != f.OwnerSeat || !CoreGameEngineAssistedPhysicalCardsRuntime.AssistedChoicesEqual([selected], r.Choices.Where(c => c.Id == selected.Id).ToArray()) || !CoreGameEngineAssistedPhysicalCardsRuntime.AssistedChoicesEqual([selected], DamageAcquisitionChoices(f).Where(c => c.Id == selected.Id).ToArray()))
            throw new InvalidOperationException("Damage acquisition must retain its exact published opaque/public slot.");
        if (!_coreMatchParticipantsPlayersAccess._players[f.OwnerSeat].IsAlive || !_coreProgramLifecycleOperations.CanInvokeProgramSkill(f))
        {
            _coreNativePromptBoundaryOperations.ClearPendingDecision();
            _coreProgramLifecycleOperations.CancelProgramBindingAndCleanup(f, "获得前技能来源已失效。");
            return;
        }

        var from = new CardLocation(CoreGameEngineDamageZoneAcquisitionDefinitions.DamageAcquisitionZones[r.NextZone], r.VictimSeat);
        var cards = _cardZonesOperations._cardZones.CardsAt(from);
        var slot = int.Parse(selected.Parameters["slot-index"], CultureInfo.InvariantCulture);
        var card = cards[slot];
        if (from.Zone == CardZoneKind.Hand ? selected.Cards.Count != 0 : !selected.Cards.SequenceEqual([card.Id]))
            throw new InvalidOperationException("Damage acquisition cannot expose foreign hand entities or replace its public card.");
        _coreNativePromptBoundaryOperations.ClearPendingDecision();
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { DamageZoneAcquisition = r with { Cards = r.Cards.Append(card.Id).ToArray(), From = r.From.Append(from).ToArray(), NextZone = r.NextZone + 1, Choices = [] } });
        PublishDamageAcquisitionChoice(f);
    }

    internal void IssueDamageZoneAcquisition(ProgramSkillFrame f)
    {
        var r = f.DamageZoneAcquisition!;
        if (r.Stage != DamageZoneAcquisitionStage.Selecting || r.NextZone != 3 || r.Cards.Count != r.From.Count || r.Cards.Select((id, i) => _cardZonesOperations._cardZones.GetLocation(id) != r.From[i]).Any(x => x))
            throw new InvalidOperationException("Damage acquisition lost its selected one-per-zone physical invoice.");
        var before = _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence;
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { DamageZoneAcquisition = r with { Stage = DamageZoneAcquisitionStage.MovementChildren, Choices = [], Before = before, After = before }, PendingMovementContinuation = new(f.OwnerSeat, 0, null) });
        foreach (var(id, i)in r.Cards.Select((id, i) => (id, i)))
        {
            var card = _coreCardAttackStateOperations.GetAttackCard(id);
            var to = card.IsGeneralWeapon && r.From[i].Zone == CardZoneKind.Equipment ? CardLocation.OutsideGame : CardLocation.Hand(f.OwnerSeat);
            _corePhysicalMovementFacadesOperations.MoveCard(card, r.From[i], to, new(DamageAcquisitionReason));
        }

        _coreRuntimeOperations.ReplaceRuntimeTop(f = _coreProgramLifecycleOperations.GetActiveProgramFrame(f.Id)with { DamageZoneAcquisition = f.DamageZoneAcquisition!with { After = _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence } });
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new DamageZoneAcquisitionPaidEvent(f.Id, f.DamageZoneAcquisition!));
        if (!DrainDamageAcquisitionChildren(f))
            ReturnDamageAcquisitionMovement(f);
    }

    internal bool DrainDamageAcquisitionChildren(ProgramSkillFrame f) => _coreRecoveryReplacementOperations.TryBeginQueuedRecoveryReplacement(f.Id, PostEventContinuation.AwaitedProgramMovement) || _coreCharacterStateProgramsOperations.TryBeginCharacterStateProgramWindow(f.Id, CharacterStateContinuation.Program) || _coreHpChangeProgramsOperations.TryBeginHpChangedProgramWindow(f.Id, PostEventContinuation.AwaitedProgramMovement) || _coreCardMovementProgramsOperations.TryBeginCardsMovedProgramWindow(f.Id) || _coreSkillOwnershipWindowsOperations.TryBeginAdvancedSkillsChanged(f.Id);
    [ProgramContinuation(ProgramContinuationKind.Movement, 21300)]
    internal bool ReturnDamageAcquisitionMovement(ProgramSkillFrame f)
    {
        if (f.DamageZoneAcquisition is not { Stage: DamageZoneAcquisitionStage.MovementChildren } r || f.PendingMovementContinuation is null)
            return false;
        RequireDamageAcquisitionReceipt(f);
        if (DrainDamageAcquisitionChildren(f))
            return true;
        _coreCardUseModulesOperations.GrantProgramTurnCardTargetRestriction(f, SkillProgramCardTargetRestriction.SelfOnly, f.OwnerSeat);
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new DamageZoneAcquisitionCompletedEvent(f.Id, r.Source, r.ActualTurn));
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { DamageZoneAcquisition = null, PendingMovementContinuation = null });
        _coreSkillProgramsOperations.FinishProgramSkill(f, true);
        return true;
    }

    [ProgramContinuation(ProgramContinuationKind.Resume, 21300)]
    internal bool ResumeDamageAcquisition(long id)
    {
        if (_resolutionStackOperations._resolutionStack.LastOrDefault()is not ProgramSkillFrame { DamageZoneAcquisition: { } r } f || f.Id != id)
            return false;
        if (r.Stage == DamageZoneAcquisitionStage.Selecting)
        {
            if (_corePendingDecisionPolicyPipelineOperations._pendingDecision is null)
                PublishDamageAcquisitionChoice(f);
            return true;
        }

        return ReturnDamageAcquisitionMovement(f);
    }

    internal void RequireDamageAcquisitionReceipt(ProgramSkillFrame f)
    {
        var r = f.DamageZoneAcquisition ?? throw new InvalidOperationException("Damage acquisition lost its receipt.");
        if (!ExactDamageAcquisitionParent(f) || r.WindowId != f.WindowContext!.ParentFrameId || r.DamageId != f.WindowContext.DamageFrameId || r.Source != DamageAcquisitionSource(f) || r.GameplayHash != f.GameplayHash || r.ActualTurn != _coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess._turnNumber || r.VictimSeat != f.WindowContext.TargetSeat || r.NextZone is < 0 or > 3 || r.Cards.Count != r.From.Count || r.Cards.Count > r.NextZone || r.Cards.Distinct().Count() != r.Cards.Count || r.From.Select(l => l.Zone).Distinct().Count() != r.From.Count || r.From.Any(l => l.OwnerSeat != r.VictimSeat || !CoreGameEngineDamageZoneAcquisitionDefinitions.DamageAcquisitionZones.Contains(l.Zone)) || !Enum.IsDefined(r.Stage) || r.Before < 0 || r.After < r.Before || r.After > _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence || _coreAlternatingBenefitsOperations.CompleteProgramEventHistory().OfType<DamageZoneAcquisitionStartedEvent>().Count(e => e.ProgramFrameId == f.Id && e.Receipt.WindowId == r.WindowId && e.Receipt.DamageId == r.DamageId && e.Receipt.Source == r.Source && e.Receipt.GameplayHash == r.GameplayHash && e.Receipt.ActualTurn == r.ActualTurn && e.Receipt.VictimSeat == r.VictimSeat) != 1)
            throw new InvalidOperationException("Damage acquisition lost its actual candidate, source or one-per-zone invoice.");
        if (r.Stage == DamageZoneAcquisitionStage.Selecting)
        {
            if (f.PendingMovementContinuation is not null || r.Before != 0 || r.After != 0 || r.Cards.Select((id, i) => _cardZonesOperations._cardZones.GetLocation(id) != r.From[i]).Any(x => x))
                throw new InvalidOperationException("Unpaid zone acquisition cannot retain a completed movement.");
            if (_resolutionStackOperations._resolutionStack.LastOrDefault()?.Id == f.Id && _corePendingDecisionPolicyPipelineOperations._pendingDecision is { } prompt && (prompt.Kind != DecisionKind.ProgramTrigger || prompt.PlayerSeat != f.OwnerSeat || !prompt.IsPrivate || prompt.TargetSeat != r.VictimSeat || !CoreGameEngineAssistedPhysicalCardsRuntime.AssistedChoicesEqual(prompt.Choices, r.Choices)))
                throw new InvalidOperationException("Zone acquisition lost its chooser or foreign hand privacy.");
            return;
        }

        if (f.PendingMovementContinuation is not { BeforeCount: 0, CoverageResultBind: null } pending || pending.SubjectSeat != f.OwnerSeat || r.NextZone != 3 || _coreAlternatingBenefitsOperations.CompleteProgramEventHistory().OfType<DamageZoneAcquisitionPaidEvent>().Where(e => e.ProgramFrameId == f.Id).ToArray()is not [var paid] || paid.Receipt.Source != r.Source || paid.Receipt.Before != r.Before || paid.Receipt.After != r.After || !paid.Receipt.Cards.SequenceEqual(r.Cards) || !paid.Receipt.From.SequenceEqual(r.From))
            throw new InvalidOperationException("Zone acquisition lost its once-issued movement parent.");
        foreach (var(id, i)in r.Cards.Select((id, i) => (id, i)))
            if (_coreCardMovementJournalCardMovementsMovementSequenceAccess._cardMovements.Count(m => m.Sequence > r.Before && m.Sequence <= r.After && m.CardId == id && m.From == r.From[i] && m.To == _coreEphemeralCardMaterialsOperations.NormalizeEphemeralCardDestination(_coreAdvancedLifecycleOperations.GetAdvancedCard(id), m.From, (m.CardKind == CardKind.GeneralWeapon && m.From.Zone == CardZoneKind.Equipment ? CardLocation.OutsideGame : CardLocation.Hand(f.OwnerSeat))) && m.Reason.Value == DamageAcquisitionReason) != 1)
                throw new InvalidOperationException("Zone acquisition must move each selected physical entity exactly once.");
    }

    [ProgramRuntimeHook(ProgramRuntimePhase.AfterOwnerDeath, 380)]
    [ProgramRuntimeHook(ProgramRuntimePhase.AfterSkillLoss, 380)]
    internal bool CanContinueDamageZoneAcquisition(ProgramSkillFrame f)
    {
        if (f.DamageZoneAcquisition?.Stage != DamageZoneAcquisitionStage.MovementChildren)
            return false;
        RequireDamageAcquisitionReceipt(f);
        return true;
    }

    [ProgramAiChoice(SkillProgramEffectOp.ObtainEachDamageTargetZoneThenSelfOnly)]
    internal PromptChoice SelectAiDamageAcquisition(PendingDecision p, ProgramSkillFrame f) => p.Choices.OrderByDescending(c => c.Cards is [var id] ? CoreNativeAiDecisionsRuntime.GetKeepValue(_coreCardAttackStateOperations.GetAttackCard(id), _coreMatchParticipantsPlayersAccess._players[f.OwnerSeat]) : 0).First();
    SkillProgramStepOutcome IDamageZoneAndAdjacentTargetHost.ObtainEachDamageTargetZoneThenSelfOnly(ProgramSkillFrame f) => BeginDamageZoneAcquisition(f);
    SkillProgramStepOutcome IDamageZoneAndAdjacentTargetHost.JoinAdjacentRedCardTarget(ProgramSkillFrame f) => _coreAdjacentRedTargetsOperations.BeginAdjacentRedTarget(f);
}
