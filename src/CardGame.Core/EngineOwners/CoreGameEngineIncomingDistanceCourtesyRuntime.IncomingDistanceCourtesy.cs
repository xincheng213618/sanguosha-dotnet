#nullable enable

using System.Globalization;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CoreGameEngineIncomingDistanceCourtesyRuntime
{
    SkillProgramStepOutcome IIncomingDistanceCourtesyHost.BeginIncomingDistanceCourtesy(ProgramSkillFrame f, IncomingDistanceCourtesyKind kind) => BeginIncomingDistanceCourtesy(f, kind);
    internal static string IncomingDistanceCourtesyReason(ProgramSkillFrame f, int step) => RuntimeSkills.RuntimeRules_Core_IncomingDistanceCourtesy.IncomingDistanceCourtesyReason(f, step);
    internal bool ExactIncomingDistanceEndingParent(ProgramSkillFrame f) => f.TriggerId is not null && f.WindowContext is { Window: SkillProgramTriggerWindow.TurnEnding } c && c.OwnerSeat == f.OwnerSeat && c.SourceSeat == _coreActualTurnProgressCurrentSeatTurnNumberAccess._currentSeat && c.TargetSeat == _coreActualTurnProgressCurrentSeatTurnNumberAccess._currentSeat && _resolutionStackOperations._resolutionStack.OfType<TurnEndingBoundaryFrame>().SingleOrDefault(p => p.Id == c.ParentFrameId)is { } p && p.OwnerSeat == _coreActualTurnProgressCurrentSeatTurnNumberAccess._currentSeat && p.TurnNumber == _coreActualTurnProgressCurrentSeatTurnNumberAccess._turnNumber && p.ItemIndex >= 0 && p.ItemIndex < p.Items.Count && p.Items[p.ItemIndex].Candidate is { } candidate && CoreGameEngineDamageTargetMountObserverRuntime.MountObserverCandidateMatches(f, candidate) && c.OccurrenceIndex == candidate.OccurrenceIndex;
    internal Card[] IncomingDistanceCosts(int owner) => _corePhysicalOwnedCardQueriesOperations.GetHand(_coreMatchParticipantsPlayersAccess._players[owner]).Concat(_corePhysicalOwnedCardQueriesOperations.GetEquipment(_coreMatchParticipantsPlayersAccess._players[owner])).Where(card => !_coreTurnHandCategoryRestrictionsOperations.IsSelfHandCategoryDiscardForbidden(owner, card, _cardZonesOperations._cardZones.GetLocation(card.Id), OwnedCardMoveIntent.Discard) && !_coreForeignEquipmentDiscardPoliciesOperations.IsForeignEquipmentDiscardPrevented(owner, card, _cardZonesOperations._cardZones.GetLocation(card.Id), OwnedCardMoveIntent.Discard)).ToArray();
    [ProgramTriggerLegality(ProgramTriggerLegalityPhase.BeforeGainQualification, 40)]
    internal bool CanOfferIncomingDistanceCourtesy(ProgramTriggerCandidate candidate, SkillProgramTrigger t, ProgramSkillWindowContext c)
    {
        if (t.Effects is not [var effect] || !IncomingDistanceCourtesyContract.IsOperation(effect.Op))
            return true;
        if (c.Window != SkillProgramTriggerWindow.TurnEnding || c.SourceSeat != _coreActualTurnProgressCurrentSeatTurnNumberAccess._currentSeat || c.TargetSeat != _coreActualTurnProgressCurrentSeatTurnNumberAccess._currentSeat || !_coreTableSeatQueriesOperations.IsValidPlayerSeat(candidate.OwnerSeat) || !_coreMatchParticipantsPlayersAccess._players[candidate.OwnerSeat].IsAlive || !_coreMatchParticipantsPlayersAccess._players[_coreActualTurnProgressCurrentSeatTurnNumberAccess._currentSeat].IsAlive || _resolutionStackOperations._resolutionStack.OfType<TurnEndingBoundaryFrame>().SingleOrDefault(p => p.Id == c.ParentFrameId)is not { } parent || parent.OwnerSeat != _coreActualTurnProgressCurrentSeatTurnNumberAccess._currentSeat || parent.TurnNumber != _coreActualTurnProgressCurrentSeatTurnNumberAccess._turnNumber)
            return false;
        return effect.Op == SkillProgramEffectOp.AdjustIncomingDistanceByLivingFactions ? candidate.OwnerSeat == _coreActualTurnProgressCurrentSeatTurnNumberAccess._currentSeat && IncomingDistanceCosts(candidate.OwnerSeat).Length > 0 && _coreMatchParticipantsPlayersAccess._players.Any(p => p.IsAlive && p.Seat != candidate.OwnerSeat && _coreNativeRuleQueriesOperations.GetCombatDistance(p.Seat, candidate.OwnerSeat) == 1) : candidate.OwnerSeat != _coreActualTurnProgressCurrentSeatTurnNumberAccess._currentSeat && !_coreProgramDodgeResponsesOperations.IsWithinAttackRange(_coreActualTurnProgressCurrentSeatTurnNumberAccess._currentSeat, candidate.OwnerSeat);
    }

    internal SkillProgramStepOutcome BeginIncomingDistanceCourtesy(ProgramSkillFrame supplied, IncomingDistanceCourtesyKind kind)
    {
        var f = _coreProgramLifecycleOperations.GetActiveProgramFrame(supplied.Id);
        if (f.IncomingDistanceCourtesy is not null || f.InstructionIndex != 1 || !ExactIncomingDistanceEndingParent(f) || !CanOfferIncomingDistanceCourtesy(new(f.OwnerSeat, f.SkillId, f.TriggerId!, f.SkillInstanceId, f.GameplayHash, 0), _coreProgramLifecycleOperations.GetProgramTrigger(f), f.WindowContext!) || _corePrivateOfferReturnsOperations.GetPausedPrivateOfferEffect(f)?.Op != (kind == IncomingDistanceCourtesyKind.FactionDistance ? SkillProgramEffectOp.AdjustIncomingDistanceByLivingFactions : SkillProgramEffectOp.ChooseEndingCourtesyBenefits))
            throw new InvalidOperationException("Incoming distance requires its exact eligible actual Ending instruction.");
        var r = new IncomingDistanceCourtesyReceipt
        {
            WindowFrameId = f.WindowContext!.ParentFrameId,
            Source = CoreGameEngineCardUseModulesRuntime.CreateProgramTurnEffectSource(f),
            GameplayHash = f.GameplayHash,
            ActualTurnNumber = _coreActualTurnProgressCurrentSeatTurnNumberAccess._turnNumber,
            ActualTurnOwnerSeat = _coreActualTurnProgressCurrentSeatTurnNumberAccess._currentSeat,
            Kind = kind,
            Stage = IncomingDistanceCourtesyStage.Choosing
        };
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { IncomingDistanceCourtesy = r });
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new IncomingDistanceCourtesyOfferedEvent(f.Id, r));
        PublishIncomingDistanceCourtesyChoice(f);
        return SkillProgramStepOutcome.AwaitChoice;
    }

    internal IReadOnlyList<PromptChoice> IncomingDistanceCourtesyChoices(ProgramSkillFrame f)
    {
        var r = f.IncomingDistanceCourtesy!;
        var choices = new List<PromptChoice>();
        PromptChoice Choice(string key, string text, IReadOnlyList<int>? cards = null) => new(new($"incoming-distance.{(f.Id)}.{(key)}"), text, cards ?? [], [], new Dictionary<string, string> { ["program-action"] = "incoming-distance-courtesy", ["selection"] = key, ["frame-id"] = f.Id.ToString(CultureInfo.InvariantCulture) });
        if (r.Kind == IncomingDistanceCourtesyKind.FactionDistance)
        {
            choices.AddRange(IncomingDistanceCosts(f.OwnerSeat).Select(card => Choice($"pay.{(card.Id)}", $"弃置【{(card.DisplayName)}】，增加其他角色计算与你的距离", [card.Id])));
            choices.Add(Choice("decline", "不执行"));
        }
        else
        {
            var available = new List<EndingCourtesyBenefit>
            {
                EndingCourtesyBenefit.OwnerDrawOne,
                EndingCourtesyBenefit.TurnOwnerDrawTwo
            };
            if (_coreMatchParticipantsPlayersAccess._players[r.ActualTurnOwnerSeat].Hp < _coreMatchParticipantsPlayersAccess._players[r.ActualTurnOwnerSeat].MaxHp)
                available.Add(EndingCourtesyBenefit.TurnOwnerRecoverOne);
            string Name(EndingCourtesyBenefit b) => b switch
            {
                EndingCourtesyBenefit.OwnerDrawOne => "你摸一张牌",
                EndingCourtesyBenefit.TurnOwnerDrawTwo => $"{(_coreMatchParticipantsPlayersAccess._players[r.ActualTurnOwnerSeat].Name)} 摸两张牌",
                _ => $"{(_coreMatchParticipantsPlayersAccess._players[r.ActualTurnOwnerSeat].Name)} 回复1点体力"};
            foreach (var first in available)
            {
                choices.Add(Choice(((int)first).ToString(CultureInfo.InvariantCulture), Name(first) + "；其他角色计算与你的距离-1"));
                foreach (var second in available.Where(b => b > first))
                    choices.Add(Choice($"{((int)first)},{((int)second)}", Name(first) + "，然后" + Name(second) + "；其他角色计算与你的距离-2"));
            }
        }

        return Array.AsReadOnly(choices.Select(PromptChoiceProjection.Freeze).ToArray());
    }

    internal void PublishIncomingDistanceCourtesyChoice(ProgramSkillFrame f)
    {
        var choices = IncomingDistanceCourtesyChoices(f);
        var skill = _contentRegistryOperations._contentRegistry.GetSkill(f.SkillId);
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { Step = ResolutionFrameStep.AwaitingResponse, IncomingDistanceCourtesy = f.IncomingDistanceCourtesy!with { Choices = choices } });
        _corePendingDecisionPolicyPipelineOperations._pendingDecision = new(DecisionKind.ProgramTrigger, f.OwnerSeat, $"【{(skill.Name)}】请选择。", Array.AsReadOnly(choices.SelectMany(c => c.Cards).Distinct().ToArray()), [], f.OwnerSeat)
        {
            PromptId = _coreNativePromptBoundaryOperations.CreatePromptId(),
            IsPrivate = f.IncomingDistanceCourtesy!.Kind == IncomingDistanceCourtesyKind.FactionDistance,
            Choices = f.IncomingDistanceCourtesy.Choices,
            SkillPrompt = new(f.SkillId, skill.Name, skill.Name, skill.Description)
        };
        _coreMatchOutcomeStatusWinnerAccess._status = _coreMatchParticipantsPlayersAccess._players[f.OwnerSeat].IsHuman ? EngineStatus.AwaitingHumanResponse : EngineStatus.Running;
    }

    [ProgramAnswer("incoming-distance-courtesy")]
    internal void ResolveIncomingDistanceCourtesyChoice(PromptChoice choice)
    {
        var f = _resolutionStackOperations._resolutionStack.LastOrDefault() as ProgramSkillFrame ?? throw new InvalidOperationException("Incoming distance lost its owning program.");
        RequireIncomingDistanceCourtesyReceipt(f);
        var r = f.IncomingDistanceCourtesy!;
        if (r.Stage != IncomingDistanceCourtesyStage.Choosing || _corePendingDecisionPolicyPipelineOperations._pendingDecision?.PlayerSeat != f.OwnerSeat || !_coreNationalSkillsOperations.HasRuntimeSkillInstance(_coreMatchParticipantsPlayersAccess._players[f.OwnerSeat], f.SkillId, f.SkillInstanceId) || !CoreGameEngineAssistedPhysicalCardsRuntime.AssistedChoicesEqual([choice], r.Choices.Where(c => c.Id == choice.Id).ToArray()) || !CoreGameEngineAssistedPhysicalCardsRuntime.AssistedChoicesEqual([choice], IncomingDistanceCourtesyChoices(f).Where(c => c.Id == choice.Id).ToArray()))
            throw new InvalidOperationException("Incoming distance selected a stale choice or an already paid instruction.");
        _coreNativePromptBoundaryOperations.ClearPendingDecision();
        if (choice.Parameters["selection"] == "decline")
        {
            CompleteIncomingDistanceCourtesy(f);
            return;
        }

        r = r with
        {
            Stage = IncomingDistanceCourtesyStage.MovementChildren,
            Choices = [],
            Benefits = r.Kind == IncomingDistanceCourtesyKind.Courtesy ? choice.Parameters["selection"].Split(',').Select(s => (EndingCourtesyBenefit)int.Parse(s, CultureInfo.InvariantCulture)).ToArray() : []
        };
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { Step = ResolutionFrameStep.ResolvingEffect, IncomingDistanceCourtesy = r });
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new IncomingDistanceCourtesySelectedEvent(f.Id, r));
        if (r.Kind == IncomingDistanceCourtesyKind.FactionDistance)
        {
            var card = _coreCardAttackStateOperations.GetAttackCard(choice.Cards.Single());
            var from = _cardZonesOperations._cardZones.GetLocation(card.Id);
            var to = _coreEphemeralCardMaterialsOperations.NormalizeEphemeralCardDestination(card, from, card.IsGeneralWeapon && from.Zone == CardZoneKind.Equipment ? CardLocation.OutsideGame : CardLocation.DiscardPile);
            var movement = new IncomingDistanceCourtesyMovement(0, f.OwnerSeat, 1, 1, _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence, _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence, card.Id, from, to);
            _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { PendingMovementContinuation = new(f.OwnerSeat, 0, null) });
            var logicalDiscard = _coreNativeLogicalDiscardsOperations.CaptureNativeLogicalDiscardInvocation(f.OwnerSeat, f.OwnerSeat, [card.Id], new(IncomingDistanceCourtesyReason(f, 0)), f.Id);
            _coreProgramAtomicCardMovesOperations.MoveProgramCardsFromMultipleSources([card.Id], to, new(IncomingDistanceCourtesyReason(f, 0)), (_, moves) => CommitIncomingDistanceCourtesyMovement(f.Id, movement with { After = moves.Max(m => m.Sequence) }));
            _coreNativeLogicalDiscardsOperations.CompleteNativeLogicalDiscardInvocation(logicalDiscard);
            _coreRuntimeOperations.AdvanceRuntimeProgram(f.Id);
            return;
        }

        IssueNextEndingCourtesyBenefit(f);
    }

    internal void CommitIncomingDistanceCourtesyMovement(long id, IncomingDistanceCourtesyMovement movement)
    {
        var f = _coreProgramLifecycleOperations.GetActiveProgramFrame(id);
        var r = f.IncomingDistanceCourtesy!;
        _coreRuntimeOperations.ReplaceRuntimeTop(f with { IncomingDistanceCourtesy = r with { Movements = r.Movements.Append(movement).ToArray() } });
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new IncomingDistanceCourtesyMovementIssuedEvent(id, movement));
    }

    internal void IssueNextEndingCourtesyBenefit(ProgramSkillFrame supplied)
    {
        var f = _coreProgramLifecycleOperations.GetActiveProgramFrame(supplied.Id);
        var r = f.IncomingDistanceCourtesy!;
        if (r.NextBenefitIndex >= r.Benefits.Count || _coreMatchOutcomeStatusWinnerAccess._winner != Winner.None)
        {
            CompleteIncomingDistanceCourtesy(f);
            return;
        }

        var b = r.Benefits[r.NextBenefitIndex];
        var target = b == EndingCourtesyBenefit.OwnerDrawOne ? f.OwnerSeat : r.ActualTurnOwnerSeat;
        var requested = b == EndingCourtesyBenefit.TurnOwnerDrawTwo ? 2 : 1;
        var before = _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence;
        var actual = 0;
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { PendingMovementContinuation = new(target, 0, null) });
        if (_coreMatchParticipantsPlayersAccess._players[target].IsAlive)
        {
            if (b == EndingCourtesyBenefit.TurnOwnerRecoverOne)
                _kernelCoreGameEngineAdvancedLifecycleHostOperations.CreateProgramSkillHost().Recover(f.Id, f.OwnerSeat, target, 1, null, null);
            else
                actual = _corePhysicalDrawFacadesOperations.DrawCards(_coreMatchParticipantsPlayersAccess._players[target], requested, true, new(IncomingDistanceCourtesyReason(f, r.NextBenefitIndex))).Count;
        }

        CommitIncomingDistanceCourtesyMovement(f.Id, new(r.NextBenefitIndex, target, requested, actual, before, _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence));
        _coreRuntimeOperations.AdvanceRuntimeProgram(f.Id);
    }

    internal bool DrainIncomingDistanceCourtesyChildren(ProgramSkillFrame f) => _coreRecoveryReplacementOperations.TryBeginQueuedRecoveryReplacement(f.Id, PostEventContinuation.AwaitedProgramMovement) || _coreCharacterStateProgramsOperations.TryBeginCharacterStateProgramWindow(f.Id, CharacterStateContinuation.Program) || _coreHpChangeProgramsOperations.TryBeginHpChangedProgramWindow(f.Id, PostEventContinuation.AwaitedProgramMovement) || _coreCardMovementProgramsOperations.TryBeginCardsMovedProgramWindow(f.Id) || _coreSkillOwnershipWindowsOperations.TryBeginAdvancedSkillsChanged(f.Id);
    [ProgramContinuation(ProgramContinuationKind.Movement, 21000)]
    internal bool ReturnIncomingDistanceCourtesyMovement(ProgramSkillFrame f)
    {
        if (f.IncomingDistanceCourtesy is null)
            return false;
        _coreRuntimeOperations.AdvanceRuntimeProgram(f.Id);
        return true;
    }

    [ProgramContinuation(ProgramContinuationKind.Resume, 21000)]
    internal bool ResumeIncomingDistanceCourtesy(long id)
    {
        if (_resolutionStackOperations._resolutionStack.LastOrDefault()is not ProgramSkillFrame { IncomingDistanceCourtesy: { } r } f || f.Id != id)
            return false;
        RequireIncomingDistanceCourtesyReceipt(f);
        if (r.Stage == IncomingDistanceCourtesyStage.Choosing)
        {
            if (_corePendingDecisionPolicyPipelineOperations._pendingDecision is null)
                PublishIncomingDistanceCourtesyChoice(f);
            return true;
        }

        if (DrainIncomingDistanceCourtesyChildren(f))
            return true;
        _coreRuntimeOperations.ReplaceRuntimeTop(f = _coreProgramLifecycleOperations.GetActiveProgramFrame(id)with { PendingMovementContinuation = null });
        if (_coreMatchOutcomeStatusWinnerAccess._winner != Winner.None)
        {
            CompleteIncomingDistanceCourtesy(f);
            return true;
        }

        var amount = r.Kind == IncomingDistanceCourtesyKind.FactionDistance ? _coreRuleQueriesOperations.GetLivingFactionCount() : -1;
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new IncomingDistanceChangedEvent(f.Id, r.NextBenefitIndex, r.Source, r.GameplayHash, r.ActualTurnNumber, r.ActualTurnOwnerSeat, amount));
        if (r.Kind == IncomingDistanceCourtesyKind.FactionDistance)
        {
            CompleteIncomingDistanceCourtesy(f);
            return true;
        }

        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { IncomingDistanceCourtesy = r with { NextBenefitIndex = r.NextBenefitIndex + 1 } });
        IssueNextEndingCourtesyBenefit(f);
        return true;
    }

    internal void CompleteIncomingDistanceCourtesy(ProgramSkillFrame f)
    {
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new IncomingDistanceCourtesyCompletedEvent(f.Id));
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { IncomingDistanceCourtesy = null, PendingMovementContinuation = null });
        _coreSkillProgramsOperations.FinishProgramSkill(f, true);
    }

    [ProgramDistanceRule(ProgramDistanceStage.IncomingContributions, 10)]
    internal IEnumerable<RuleQueryContribution> IncomingDistanceCourtesyContributions(int target) => !_contentRegistryOperations._contentRegistry.ProgramDependencies.HasTriggerOperation(SkillProgramEffectOp.AdjustIncomingDistanceByLivingFactions) && !_contentRegistryOperations._contentRegistry.ProgramDependencies.HasTriggerOperation(SkillProgramEffectOp.ChooseEndingCourtesyBenefits) ? [] : _coreProgramEventHistoryOperations.ProgramEventHistory<IncomingDistanceChangedEvent>().Where(e => e.Source.OwnerSeat == target).Select(e => (RuleQueryContribution)new FiniteRuleQueryContribution($"incoming-distance:{(e.ProgramFrameId)}:{(e.StepIndex)}", SkillRuleOperation.Add, e.Amount));
    [ProgramRuntimeHook(ProgramRuntimePhase.AfterOwnerDeath, 460)]
    [ProgramRuntimeHook(ProgramRuntimePhase.AfterSkillLoss, 450)]
    internal bool CanContinueIncomingDistanceCourtesy(ProgramSkillFrame f)
    {
        if (f.IncomingDistanceCourtesy is not { Stage: IncomingDistanceCourtesyStage.MovementChildren })
            return false;
        RequireIncomingDistanceCourtesyReceipt(f);
        return true;
    }

    internal void RequireIncomingDistanceCourtesyReceipt(ProgramSkillFrame f)
    {
        var r = f.IncomingDistanceCourtesy ?? throw new InvalidOperationException("Incoming distance lost its typed receipt.");
        if (!Enum.IsDefined(r.Kind) || !Enum.IsDefined(r.Stage) || !ExactIncomingDistanceEndingParent(f) || f.InstructionIndex != 1 || r.WindowFrameId != f.WindowContext!.ParentFrameId || r.Source != CoreGameEngineCardUseModulesRuntime.CreateProgramTurnEffectSource(f) || r.GameplayHash != f.GameplayHash || _contentRegistryOperations._contentRegistry.GetSkill(f.SkillId).Program?.GameplayHash != f.GameplayHash || r.ActualTurnNumber != _coreActualTurnProgressCurrentSeatTurnNumberAccess._turnNumber || r.ActualTurnOwnerSeat != _coreActualTurnProgressCurrentSeatTurnNumberAccess._currentSeat || _corePrivateOfferReturnsOperations.GetPausedPrivateOfferEffect(f)?.Op != (r.Kind == IncomingDistanceCourtesyKind.FactionDistance ? SkillProgramEffectOp.AdjustIncomingDistanceByLivingFactions : SkillProgramEffectOp.ChooseEndingCourtesyBenefits) || (r.Kind == IncomingDistanceCourtesyKind.FactionDistance ? r.Source.OwnerSeat != _coreActualTurnProgressCurrentSeatTurnNumberAccess._currentSeat : r.Source.OwnerSeat == _coreActualTurnProgressCurrentSeatTurnNumberAccess._currentSeat) || _coreAlternatingBenefitsOperations.CompleteProgramEventHistory().OfType<IncomingDistanceCourtesyOfferedEvent>().Count(e => e.ProgramFrameId == f.Id && e.Receipt.WindowFrameId == r.WindowFrameId && e.Receipt.Source == r.Source && e.Receipt.GameplayHash == r.GameplayHash && e.Receipt.Kind == r.Kind) != 1)
            throw new InvalidOperationException("Incoming distance lost its actual Ending parent, source, kind or instruction.");
        var selected = _coreAlternatingBenefitsOperations.CompleteProgramEventHistory().OfType<IncomingDistanceCourtesySelectedEvent>().Where(e => e.ProgramFrameId == f.Id).ToArray();
        if (r.Stage == IncomingDistanceCourtesyStage.Choosing)
        {
            if (selected.Length != 0 || r.Movements.Count != 0 || r.Benefits.Count != 0 || r.NextBenefitIndex != 0 || f.PendingMovementContinuation is not null)
                throw new InvalidOperationException("Unpaid incoming distance contains an issued benefit or movement.");
            return;
        }

        if (selected is not [var s] || s.Receipt.Source != r.Source || s.Receipt.Kind != r.Kind || !s.Receipt.Benefits.SequenceEqual(r.Benefits) || r.Benefits.Count != r.Benefits.Distinct().Count() || r.Benefits.Any(b => !Enum.IsDefined(b)) || (r.Kind == IncomingDistanceCourtesyKind.FactionDistance ? r.Benefits.Count != 0 || r.NextBenefitIndex != 0 || r.Movements.Count != 1 : r.Benefits.Count is < 1 or > 2 || r.NextBenefitIndex < 0 || r.NextBenefitIndex >= r.Benefits.Count || r.Movements.Count != r.NextBenefitIndex + 1) || f.PendingMovementContinuation is not { BeforeCount: 0, CoverageResultBind: null } pending || pending.SubjectSeat != r.Movements[^1].SubjectSeat)
            throw new InvalidOperationException("Incoming distance lost its exact one-cost or distinct ordered benefit selection.");
        foreach (var m in r.Movements)
        {
            if (m.Before < 0 || m.After < m.Before || m.After > _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence || m.Actual < 0 || m.Actual > m.Requested || _coreAlternatingBenefitsOperations.CompleteProgramEventHistory().OfType<IncomingDistanceCourtesyMovementIssuedEvent>().Count(e => e.ProgramFrameId == f.Id && e.Movement == m) != 1)
                throw new InvalidOperationException("Incoming distance movement lacks its unique producer.");
            var actual = _coreCardMovementJournalCardMovementsMovementSequenceAccess._cardMovements.Where(move => move.Sequence > m.Before && move.Sequence <= m.After && move.Reason.Value == IncomingDistanceCourtesyReason(f, m.StepIndex)).ToArray();
            if (r.Kind == IncomingDistanceCourtesyKind.FactionDistance)
            {
                if (m.StepIndex != 0 || m.SubjectSeat != f.OwnerSeat || m.Requested != 1 || m.Actual != 1 || m.CardId is null || m.From is not { OwnerSeat: { } owner } from || owner != f.OwnerSeat || from.Zone is not (CardZoneKind.Hand or CardZoneKind.Equipment) || actual is not [var move] || move.CardId != m.CardId || move.From != m.From || move.To != m.To)
                    throw new InvalidOperationException("Incoming distance cannot reissue or substitute its exact physical discard.");
            }
            else if (m.StepIndex < 0 || m.StepIndex >= r.Benefits.Count || m.SubjectSeat != (r.Benefits[m.StepIndex] == EndingCourtesyBenefit.OwnerDrawOne ? f.OwnerSeat : r.ActualTurnOwnerSeat) || m.Requested != (r.Benefits[m.StepIndex] == EndingCourtesyBenefit.TurnOwnerDrawTwo ? 2 : 1) || m.CardId is not null || m.From is not null || m.To is not null || actual.Length != m.Actual || actual.Any(move => move.From != CardLocation.DrawPile || move.To != CardLocation.Hand(m.SubjectSeat)))
                throw new InvalidOperationException("Courtesy benefits lost their exact participant, count or private draw movement.");
        }

        var changes = _coreAlternatingBenefitsOperations.CompleteProgramEventHistory().OfType<IncomingDistanceChangedEvent>().Where(e => e.ProgramFrameId == f.Id).ToArray();
        if (changes.Length != r.NextBenefitIndex || changes.Where((e, i) => e.StepIndex != i || e.Source != r.Source || e.Amount != -1).Any())
            throw new InvalidOperationException("Courtesy returned to an already settled or missing distance step.");
    }

    [ProgramAiChoice(SkillProgramEffectOp.AdjustIncomingDistanceByLivingFactions, SkillProgramEffectOp.ChooseEndingCourtesyBenefits)]
    internal PromptChoice SelectAiIncomingDistanceCourtesy(PendingDecision p, ProgramSkillFrame f) => f.IncomingDistanceCourtesy!.Kind == IncomingDistanceCourtesyKind.FactionDistance ? p.Choices.Where(c => c.Cards.Count == 1).OrderBy(c => CoreNativeAiDecisionsRuntime.GetKeepValue(_coreCardAttackStateOperations.GetAttackCard(c.Cards[0]), _coreMatchParticipantsPlayersAccess._players[f.OwnerSeat])).FirstOrDefault() ?? p.Choices[^1] : p.Choices.First(c => c.Parameters["selection"] == "0");
}
