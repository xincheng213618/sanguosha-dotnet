#nullable enable

using System.Globalization;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CoreGameEnginePhaseStartCardControlRuntime
{
    SkillProgramStepOutcome IPhaseStartCardControlHost.GrantOtherPlayUseRestriction(ProgramSkillFrame frame, string skillId) => BeginPhaseStartCardControl(frame, PhaseStartCardControlKind.OtherPlayRestriction, skillId);
    SkillProgramStepOutcome IPhaseStartCardControlHost.ChooseStartingEquipmentControl(ProgramSkillFrame frame) => BeginPhaseStartCardControl(frame, PhaseStartCardControlKind.StartingEquipment, null);
    internal static string PhaseStartControlReason(PhaseStartCardControlKind kind) => RuntimeSkills.RuntimeRules_Core_PhaseStartCardControl.PhaseStartControlReason(kind);
    internal CardConversionSource PhaseStartControlSource(ProgramSkillFrame f) => new(f.SkillId, CoreGameEngineProgramLifecycleRuntime.GetProgramBindingId(f), f.OwnerSeat, f.SkillInstanceId);
    internal Card[] OtherPlayRestrictionCosts(int owner) => _corePhysicalOwnedCardQueriesOperations.GetHand(_coreMatchParticipantsPlayersAccess._players[owner]).Concat(_corePhysicalOwnedCardQueriesOperations.GetEquipment(_coreMatchParticipantsPlayersAccess._players[owner])).Where(card => !_coreTurnHandCategoryRestrictionsOperations.IsSelfHandCategoryDiscardForbidden(owner, card, _cardZonesOperations._cardZones.GetLocation(card.Id), OwnedCardMoveIntent.Discard) && !_coreForeignEquipmentDiscardPoliciesOperations.IsForeignEquipmentDiscardPrevented(owner, card, _cardZonesOperations._cardZones.GetLocation(card.Id), OwnedCardMoveIntent.Discard)).ToArray();
    internal bool HasPhaseStartControlBoundary(int owner, ProgramSkillWindowContext? context, PhaseStartCardControlKind kind) => context is { Window: SkillProgramTriggerWindow.PlayPhaseStarting, SourceSeat: { } actor, TargetSeat: { } target } && actor == target && actor == _coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess._currentSeat && context.OwnerSeat == owner && _coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess._phase == TurnPhase.Play && _coreMatchParticipantsPlayersAccess._players[owner].IsAlive && _coreMatchParticipantsPlayersAccess._players[actor].IsAlive && (kind == PhaseStartCardControlKind.OtherPlayRestriction ? actor != owner : actor == owner) && _resolutionStackOperations._resolutionStack.OfType<PlayPhaseStartingBoundaryFrame>().Any(p => p.Id == context.ParentFrameId && p.OwnerSeat == actor);
    [ProgramTriggerLegality(ProgramTriggerLegalityPhase.BeforeGainQualification, 20)]
    internal bool CanOfferPhaseStartCardControl(ProgramTriggerCandidate candidate, SkillProgramTrigger trigger, ProgramSkillWindowContext context)
    {
        if (trigger.Effects is [{ Op: SkillProgramEffectOp.GrantOtherPlayUseRestriction }])
            return HasPhaseStartControlBoundary(candidate.OwnerSeat, context, PhaseStartCardControlKind.OtherPlayRestriction) && _coreProgramDodgeResponsesOperations.IsWithinAttackRange(_coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess._currentSeat, candidate.OwnerSeat) && OtherPlayRestrictionCosts(candidate.OwnerSeat).Length > 0;
        if (trigger.Effects is [{ Op: SkillProgramEffectOp.ChooseStartingEquipmentControl }])
            return HasPhaseStartControlBoundary(candidate.OwnerSeat, context, PhaseStartCardControlKind.StartingEquipment) && StartingEquipmentControlChoices(candidate.OwnerSeat).Any();
        return true;
    }

    internal IEnumerable<(Card Card, int Seat, bool Obtain)> StartingEquipmentControlChoices(int owner)
    {
        foreach (var target in _coreMatchParticipantsPlayersAccess._players.Where(p => p.IsAlive))
            foreach (var card in _corePhysicalOwnedCardQueriesOperations.GetEquipment(target))
            {
                if (target.Seat != owner && !_coreForeignEquipmentDiscardPoliciesOperations.IsForeignEquipmentDiscardPrevented(owner, card, CardLocation.Equipment(target.Seat), OwnedCardMoveIntent.Discard))
                    yield return (card, target.Seat, false);
                if (_coreFieldEquipmentOperations.FieldEquipmentSlot(card) == EquipmentSlot.Armor)
                    yield return (card, target.Seat, true);
            }
    }

    internal SkillProgramStepOutcome BeginPhaseStartCardControl(ProgramSkillFrame supplied, PhaseStartCardControlKind kind, string? grantedSkill)
    {
        var f = _coreProgramLifecycleOperations.GetActiveProgramFrame(supplied.Id);
        if (f.PhaseStartCardControl is not null || f.InstructionIndex != 1 || !HasPhaseStartControlBoundary(f.OwnerSeat, f.WindowContext, kind) || !CanOfferPhaseStartCardControl(new(f.OwnerSeat, f.SkillId, f.TriggerId!, f.SkillInstanceId, f.GameplayHash, 0), _coreProgramLifecycleOperations.GetProgramTrigger(f), f.WindowContext!))
            throw new InvalidOperationException("Phase-starting card control requires its exact eligible owner and actual boundary.");
        var r = new PhaseStartCardControlReceipt(PhaseStartControlSource(f), f.GameplayHash, f.Id, f.InstructionIndex, f.WindowContext!.ParentFrameId, _coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess._turnNumber, _coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess._currentSeat, _coreGameEngineCardUseDebitsCardUseDebitPhaseInstanceIdAccess._cardUseDebitPhaseInstanceId, kind, PhaseStartCardControlStage.Choosing, grantedSkill);
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { PhaseStartCardControl = r });
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new PhaseStartCardControlIssuedEvent(r));
        PublishPhaseStartCardControlChoice(f);
        return SkillProgramStepOutcome.AwaitChoice;
    }

    internal IReadOnlyList<PromptChoice> PhaseStartCardControlChoices(ProgramSkillFrame f)
    {
        var r = f.PhaseStartCardControl!;
        var choices = new List<PromptChoice>();
        PromptChoice Choice(string branch, string label, int? card = null, int? seat = null) => new(new($"phase-start-control.{(f.Id)}.{(branch)}.{(card ?? -1)}.{(seat ?? -1)}"), label, card is { } id ? [id] : [], seat is { } target ? [target] : [], new Dictionary<string, string> { ["program-action"] = "phase-start-card-control", ["frame-id"] = f.Id.ToString(CultureInfo.InvariantCulture), ["branch"] = branch });
        if (r.Stage != PhaseStartCardControlStage.Choosing)
            return[];
        if (r.Kind == PhaseStartCardControlKind.OtherPlayRestriction)
            choices.AddRange(OtherPlayRestrictionCosts(f.OwnerSeat).Select(card => Choice("pay", $"弃置【{(card.DisplayName)}】", card.Id, f.OwnerSeat)));
        else
            choices.AddRange(StartingEquipmentControlChoices(f.OwnerSeat).Select(option => Choice(option.Obtain ? "obtain" : "discard", $"{((option.Obtain ? "获得" : "弃置"))} {(_coreMatchParticipantsPlayersAccess._players[option.Seat].Name)}的【{(option.Card.DisplayName)}】", option.Card.Id, option.Seat)));
        choices.Add(Choice("decline", "不执行"));
        return Array.AsReadOnly(choices.Select(PromptChoiceProjection.Freeze).ToArray());
    }

    internal void PublishPhaseStartCardControlChoice(ProgramSkillFrame f)
    {
        var choices = PhaseStartCardControlChoices(f);
        var skill = _contentRegistryOperations._contentRegistry.GetSkill(f.SkillId);
        _corePendingDecisionPolicyPipelineOperations._pendingDecision = new(DecisionKind.ProgramTrigger, f.OwnerSeat, $"【{(skill.Name)}】请选择。", Array.AsReadOnly(choices.SelectMany(c => c.Cards).Distinct().ToArray()), Array.AsReadOnly(choices.SelectMany(c => c.Targets).Distinct().ToArray()), f.OwnerSeat)
        {
            PromptId = _coreNativePromptBoundaryOperations.CreatePromptId(),
            IsPrivate = f.PhaseStartCardControl!.Kind == PhaseStartCardControlKind.OtherPlayRestriction,
            Choices = choices,
            SkillPrompt = new(f.SkillId, skill.Name, skill.Name, skill.Description)
        };
        _coreMatchOutcomeStatusWinnerAccess._status = _coreMatchParticipantsPlayersAccess._players[f.OwnerSeat].IsHuman ? EngineStatus.AwaitingHumanResponse : EngineStatus.Running;
    }

    [ProgramAnswer("phase-start-card-control")]
    internal void ResolvePhaseStartCardControlChoice(PromptChoice selected)
    {
        var f = _resolutionStackOperations._resolutionStack.LastOrDefault() as ProgramSkillFrame ?? throw new InvalidOperationException("No phase-starting control frame.");
        if (!HasExactPhaseStartCardControl(f) || f.PhaseStartCardControl!.Stage != PhaseStartCardControlStage.Choosing || _corePendingDecisionPolicyPipelineOperations._pendingDecision?.PlayerSeat != f.OwnerSeat || !_coreNationalSkillsOperations.HasRuntimeSkillInstance(_coreMatchParticipantsPlayersAccess._players[f.OwnerSeat], f.SkillId, f.SkillInstanceId) || !CoreGameEngineAssistedPhysicalCardsRuntime.AssistedChoicesEqual([selected], PhaseStartCardControlChoices(f).Where(c => c.Id == selected.Id).ToArray()))
            throw new InvalidOperationException("Phase-starting control lost its exact unpaid choice and source.");
        var r = f.PhaseStartCardControl;
        var branch = selected.Parameters["branch"];
        _coreNativePromptBoundaryOperations.ClearPendingDecision();
        if (branch == "decline")
        {
            CompletePhaseStartCardControl(f);
            return;
        }

        var card = _coreCardAttackStateOperations.GetAttackCard(selected.Cards.Single());
        var from = _cardZonesOperations._cardZones.GetLocation(card.Id);
        var obtain = r.Kind == PhaseStartCardControlKind.StartingEquipment && branch == "obtain";
        var to = _coreEphemeralCardMaterialsOperations.NormalizeEphemeralCardDestination(card, from, obtain ? CardLocation.Hand(f.OwnerSeat) : from.Zone == CardZoneKind.Equipment && card.IsGeneralWeapon ? CardLocation.OutsideGame : CardLocation.DiscardPile);
        var payment = new PhaseStartCardControlPayment(card.Id, card.Kind, _coreNativeCardAppearanceOperations.EffectiveSuit(_coreMatchParticipantsPlayersAccess._players[from.OwnerSeat!.Value], card), from, to, _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence, _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence);
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { PhaseStartCardControl = r with { Stage = PhaseStartCardControlStage.MovementChildren, Payment = payment, ArmorAcquired = obtain }, PendingMovementContinuation = new(from.OwnerSeat.Value, 0, null) });
        var logicalDiscard = obtain ? null : _coreNativeLogicalDiscardsOperations.CaptureNativeLogicalDiscardInvocation(f.OwnerSeat, f.OwnerSeat, [card.Id], new(PhaseStartControlReason(r.Kind)), f.Id);
        _coreProgramAtomicCardMovesOperations.MoveProgramCardsFromMultipleSources([card.Id], to, new(PhaseStartControlReason(r.Kind)), (_, moves) =>
        {
            foreach (var movement in moves)
                _characterBianFuRenOlOperations.RecordBianFuRenForeignDiscardProducer(movement, f.OwnerSeat, f.Id);
            var current = _coreProgramLifecycleOperations.GetActiveProgramFrame(f.Id);
            _coreRuntimeOperations.ReplaceRuntimeTop(current with { PhaseStartCardControl = current.PhaseStartCardControl!with { Payment = payment with { SequenceAfter = moves.Max(m => m.Sequence) } } });
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new PhaseStartCardControlPaidEvent(_coreProgramLifecycleOperations.GetActiveProgramFrame(f.Id).PhaseStartCardControl!));
        });
        _coreNativeLogicalDiscardsOperations.CompleteNativeLogicalDiscardInvocation(logicalDiscard);
        _coreRuntimeOperations.AdvanceRuntimeProgram(f.Id);
    }

    internal bool HasExactPhaseStartCardControl(ProgramSkillFrame f)
    {
        if (f.PhaseStartCardControl is not { } r || r.ProgramFrameId != f.Id || r.InstructionIndex != 1 || f.InstructionIndex != 1 || r.Source != PhaseStartControlSource(f) || r.GameplayHash != f.GameplayHash || _contentRegistryOperations._contentRegistry.GetSkill(f.SkillId).Program?.GameplayHash != f.GameplayHash || f.WindowContext is not { Window: SkillProgramTriggerWindow.PlayPhaseStarting } context || context.ParentFrameId != r.PhaseParentFrameId || context.OwnerSeat != f.OwnerSeat || context.SourceSeat != r.ActualTurnOwnerSeat || context.TargetSeat != r.ActualTurnOwnerSeat || r.ActualTurnNumber != _coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess._turnNumber || r.ActualTurnOwnerSeat != _coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess._currentSeat || r.PhaseInstanceId != _coreGameEngineCardUseDebitsCardUseDebitPhaseInstanceIdAccess._cardUseDebitPhaseInstanceId || _coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess._phase != TurnPhase.Play || _resolutionStackOperations._resolutionStack.OfType<PlayPhaseStartingBoundaryFrame>().SingleOrDefault(p => p.Id == r.PhaseParentFrameId)is not { } parent || parent.ItemIndex < 0 || parent.ItemIndex >= parent.Items.Count || parent.Items[parent.ItemIndex].Candidate is not { } candidate || !CoreGameEngineDamageTargetMountObserverRuntime.MountObserverCandidateMatches(f, candidate) || _coreProgramLifecycleOperations.GetProgramTrigger(f).Effects is not [var effect] || effect.Op != (r.Kind == PhaseStartCardControlKind.OtherPlayRestriction ? SkillProgramEffectOp.GrantOtherPlayUseRestriction : SkillProgramEffectOp.ChooseStartingEquipmentControl) || (r.Kind == PhaseStartCardControlKind.OtherPlayRestriction ? effect.SkillIds is not [var granted] || granted != r.GrantedSkillId : r.GrantedSkillId is not null) || _coreAlternatingBenefitsOperations.CompleteProgramEventHistory().OfType<PhaseStartCardControlIssuedEvent>().Count(e => e.Receipt.ProgramFrameId == f.Id && e.Receipt.Source == r.Source && e.Receipt.PhaseParentFrameId == r.PhaseParentFrameId && e.Receipt.Kind == r.Kind) != 1)
            return false;
        if (r.Payment is not { } p)
            return r.Stage == PhaseStartCardControlStage.Choosing && !r.ArmorAcquired && f.PendingMovementContinuation is null;
        return r.Stage is PhaseStartCardControlStage.MovementChildren or PhaseStartCardControlStage.GrantChildren && p.SequenceAfter > p.SequenceBefore && p.From.Zone is CardZoneKind.Hand or CardZoneKind.Equipment && (r.Kind == PhaseStartCardControlKind.OtherPlayRestriction ? p.From.OwnerSeat == f.OwnerSeat && !r.ArmorAcquired && p.To == _coreEphemeralCardMaterialsOperations.NormalizeEphemeralCardDestination(_coreAdvancedLifecycleOperations.GetAdvancedCard(p.CardId), p.From, p.Kind == CardKind.GeneralWeapon && p.From.Zone == CardZoneKind.Equipment ? CardLocation.OutsideGame : CardLocation.DiscardPile) : p.From.Zone == CardZoneKind.Equipment && (r.ArmorAcquired ? p.To == _coreEphemeralCardMaterialsOperations.NormalizeEphemeralCardDestination(_coreAdvancedLifecycleOperations.GetAdvancedCard(p.CardId), p.From, CardLocation.Hand(f.OwnerSeat)) && _coreEquipmentContentOperations.EquipmentRules.Get(p.Kind).Slot == EquipmentSlot.Armor : p.From.OwnerSeat != f.OwnerSeat && p.To == _coreEphemeralCardMaterialsOperations.NormalizeEphemeralCardDestination(_coreAdvancedLifecycleOperations.GetAdvancedCard(p.CardId), p.From, p.Kind == CardKind.GeneralWeapon ? CardLocation.OutsideGame : CardLocation.DiscardPile))) && _coreCardMovementJournalCardMovementsMovementSequenceAccess._cardMovements.Count(m => m.CardId == p.CardId && m.CardKind == p.Kind && m.From == p.From && m.To == p.To && m.Sequence > p.SequenceBefore && m.Sequence <= p.SequenceAfter && m.Reason.Value == PhaseStartControlReason(r.Kind)) == 1 && _coreAlternatingBenefitsOperations.CompleteProgramEventHistory().OfType<PhaseStartCardControlPaidEvent>().Count(e => e.Receipt.ProgramFrameId == f.Id && e.Receipt.Source == r.Source && e.Receipt.Payment == p && e.Receipt.ArmorAcquired == r.ArmorAcquired) == 1;
    }

    [ProgramRuntimeHook(ProgramRuntimePhase.AfterOwnerDeath, 440)]
    [ProgramRuntimeHook(ProgramRuntimePhase.AfterSkillLoss, 470)]
    internal bool CanContinuePhaseStartCardControl(ProgramSkillFrame f) => f.PhaseStartCardControl is { Payment: not null } && _coreMatchOutcomeStatusWinnerAccess._winner == Winner.None && HasExactPhaseStartCardControl(f);
    internal bool DrainPhaseStartCardControlChildren(ProgramSkillFrame f) => _coreRecoveryReplacementOperations.TryBeginQueuedRecoveryReplacement(f.Id, PostEventContinuation.AwaitedProgramMovement) || _coreCharacterStateProgramsOperations.TryBeginCharacterStateProgramWindow(f.Id, CharacterStateContinuation.Program) || _coreHpChangeProgramsOperations.TryBeginHpChangedProgramWindow(f.Id, PostEventContinuation.AwaitedProgramMovement) || _coreCardMovementProgramsOperations.TryBeginCardsMovedProgramWindow(f.Id) || _coreSkillOwnershipWindowsOperations.TryBeginAdvancedSkillsChanged(f.Id);
    [ProgramContinuation(ProgramContinuationKind.Resume, 20600)]
    internal bool ResumePhaseStartCardControl(long id)
    {
        if (_resolutionStackOperations._resolutionStack.LastOrDefault()is not ProgramSkillFrame { PhaseStartCardControl: { } r } f || f.Id != id)
            return false;
        if (!HasExactPhaseStartCardControl(f))
            throw new InvalidOperationException("Phase-starting control lost its exact issuance or once-paid movement.");
        if (r.Stage == PhaseStartCardControlStage.Choosing)
        {
            if (_corePendingDecisionPolicyPipelineOperations._pendingDecision is null)
                PublishPhaseStartCardControlChoice(f);
            return true;
        }

        if (DrainPhaseStartCardControlChildren(f))
            return true;
        _coreRuntimeOperations.ReplaceRuntimeTop(f = _coreProgramLifecycleOperations.GetActiveProgramFrame(id)with { PendingMovementContinuation = null });
        if (_coreMatchOutcomeStatusWinnerAccess._winner != Winner.None || !_coreMatchParticipantsPlayersAccess._players[r.ActualTurnOwnerSeat].IsAlive || r.Kind == PhaseStartCardControlKind.StartingEquipment && !_coreMatchParticipantsPlayersAccess._players[f.OwnerSeat].IsAlive)
        {
            CompletePhaseStartCardControl(f);
            return true;
        }

        if (r.Stage == PhaseStartCardControlStage.MovementChildren)
        {
            _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { PhaseStartCardControl = r with { Stage = PhaseStartCardControlStage.GrantChildren } });
            if (r.Kind == PhaseStartCardControlKind.OtherPlayRestriction)
                _corePhaseStartControlPoliciesOperations.GrantOtherPlayRestriction(f);
            else if (r.ArmorAcquired && r.Payment?.To == CardLocation.Hand(f.OwnerSeat))
                _corePhaseStartControlPoliciesOperations.GrantStartingEquipmentSlashProhibition(f);
            _coreRuntimeOperations.AdvanceRuntimeProgram(id);
            return true;
        }

        CompletePhaseStartCardControl(f);
        return true;
    }

    [ProgramContinuation(ProgramContinuationKind.Movement, 20600)]
    internal bool ReturnPhaseStartCardControlMovement(ProgramSkillFrame frame)
    {
        if (frame.PhaseStartCardControl is null)
            return false;
        _coreRuntimeOperations.AdvanceRuntimeProgram(frame.Id);
        return true;
    }

    internal void CompletePhaseStartCardControl(ProgramSkillFrame f)
    {
        var r = f.PhaseStartCardControl!;
        if (r.Kind == PhaseStartCardControlKind.StartingEquipment && r.Payment is { } p)
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new StartingEquipmentControlCompletedEvent(f.Id, r.PhaseParentFrameId, r.Source, r.ActualTurnNumber, r.ActualTurnOwnerSeat, p.CardId, p.From, p.To, r.ArmorAcquired, p.SequenceBefore, p.SequenceAfter));
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { PhaseStartCardControl = null, PendingMovementContinuation = null });
        _coreSkillProgramsOperations.FinishProgramSkill(f, true);
    }

    [ProgramAiChoice(SkillProgramEffectOp.GrantOtherPlayUseRestriction, SkillProgramEffectOp.ChooseStartingEquipmentControl)]
    internal PromptChoice SelectAiPhaseStartCardControl(PendingDecision decision, ProgramSkillFrame f) => f.PhaseStartCardControl!.Kind == PhaseStartCardControlKind.OtherPlayRestriction ? decision.Choices.Where(c => c.Cards.Count == 1).OrderBy(c => CoreNativeAiDecisionsRuntime.GetKeepValue(_coreCardAttackStateOperations.GetAttackCard(c.Cards[0]), _coreMatchParticipantsPlayersAccess._players[f.OwnerSeat])).ThenBy(c => c.Cards[0]).FirstOrDefault() ?? decision.Choices[^1] : decision.Choices.FirstOrDefault(c => c.Parameters["branch"] == "obtain") ?? decision.Choices[0];
}
