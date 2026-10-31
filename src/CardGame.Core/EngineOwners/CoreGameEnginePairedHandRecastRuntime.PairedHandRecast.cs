#nullable enable

using System.Globalization;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CoreGameEnginePairedHandRecastRuntime
{
    internal long PairedHandRecastSequence => _coreCardMovementJournalCardMovementsAccess._cardMovements.LastOrDefault()?.Sequence ?? 0;

    internal static PairedHandRecastParticipantReceipt CurrentPairedRecast(ProgramPairedHandRecastReceipt r) => RuntimeSkills.RuntimeRules_Core_PairedHandRecast.CurrentPairedRecast(r);
    internal static ProgramPairedHandRecastReceipt WithCurrentPairedRecast(ProgramPairedHandRecastReceipt r, PairedHandRecastParticipantReceipt participant) => RuntimeSkills.RuntimeRules_Core_PairedHandRecast.WithCurrentPairedRecast(r, participant);
    internal PairedHandRecastMaterial[] PairedHandMaterials(int seat) => _corePhysicalOwnedCardQueriesOperations.GetHand(_coreMatchParticipantsPlayersAccess._players[seat]).Select(card => new PairedHandRecastMaterial(card.Id, card.Kind, CardLocation.Hand(seat))).ToArray();
    internal bool PairedRecastMaterialStillOwned(PairedHandRecastParticipantReceipt participant) => participant.Material is { } m && m.From == CardLocation.Hand(participant.Seat) && _cardZonesOperations._cardZones.GetLocation(m.CardId) == m.From && _corePhysicalOwnedCardQueriesOperations.GetHand(_coreMatchParticipantsPlayersAccess._players[participant.Seat]).Any(card => card.Id == m.CardId && card.Kind == m.PrintedKind);
    internal SkillProgramStepOutcome BeginPairedHandRecast(ProgramSkillFrame supplied, int partnerSeat)
    {
        var f = _coreProgramLifecycleOperations.GetActiveProgramFrame(supplied.Id);
        var plan = ProgramInstructionResolver.Default.Resolve(f, _contentRegistryOperations._contentRegistry.GetSkill(f.SkillId).Program!);
        if (plan.Activation is not { } activation || f.TriggerId is not null || f.WindowContext is not null || f.InstructionIndex != 1 || f.PairedHandRecast is not null || f.SelectedCardIds is not [var ownerCard] || f.SelectedTargetSeats is not [var target] || target != partnerSeat || target == f.OwnerSeat || !_coreTableSeatQueriesOperations.IsValidPlayerSeat(target) || _coreMatchOutcomeWinnerAccess._winner != Winner.None || !_coreMatchParticipantsPlayersAccess._players[f.OwnerSeat].IsAlive || !_coreMatchParticipantsPlayersAccess._players[target].IsAlive || !_coreProgramLifecycleOperations.CanInvokeProgramSkill(f) || _coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess._phase != TurnPhase.Play || _coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess._currentSeat != f.OwnerSeat || _coreGameEngineCardUseDebitsCardUseDebitPhaseInstanceIdAccess._cardUseDebitPhaseInstanceId <= 0 || _cardZonesOperations._cardZones.GetLocation(ownerCard) != CardLocation.Hand(f.OwnerSeat) || _corePhysicalOwnedCardQueriesOperations.GetHand(_coreMatchParticipantsPlayersAccess._players[target]).Count == 0 || _coreGameEngineSkillProgramsProgramPhaseUsesAccess._programPhaseUses.GetValueOrDefault((f.OwnerSeat, f.SkillId, activation.UsageGroup)) != 1 || _resolutionStackOperations._resolutionStack.Count != 1)
            throw new InvalidOperationException("Paired hand recast lost its original live hand entity, partner, phase debit or sole activation.");
        PairedHandRecastComposition.ValidateActivation(f.SkillId, activation);
        var r = new ProgramPairedHandRecastReceipt
        {
            InstructionIndex = f.InstructionIndex,
            Source = new(f.SkillId, CoreGameEngineProgramLifecycleRuntime.GetProgramBindingId(f), f.OwnerSeat, f.SkillInstanceId),
            GameplayHash = f.GameplayHash,
            UsageGroup = activation.UsageGroup,
            ActualTurnNumber = _coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess._turnNumber,
            ActualTurnOwnerSeat = _coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess._currentSeat,
            PhaseInstanceId = _coreGameEngineCardUseDebitsCardUseDebitPhaseInstanceIdAccess._cardUseDebitPhaseInstanceId,
            Stage = PairedHandRecastStage.ChoosingPartner,
            Owner = new(f.OwnerSeat, PairedHandMaterials(f.OwnerSeat).Single(m => m.CardId == ownerCard)),
            Partner = new(target, null),
            EligiblePartnerMaterials = PairedHandMaterials(target)
        };
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { PairedHandRecast = r });
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new PairedHandRecastStartedEvent(f.Id, r.Source, r.GameplayHash, r.UsageGroup, target, r.ActualTurnNumber, r.ActualTurnOwnerSeat, r.PhaseInstanceId));
        PublishPairedHandRecastChoice(f);
        return SkillProgramStepOutcome.AwaitChoice;
    }

    internal IReadOnlyList<PromptChoice> PairedHandRecastChoices(ProgramSkillFrame f) => Array.AsReadOnly(f.PairedHandRecast!.EligiblePartnerMaterials.Select(m => new PromptChoice(new($"paired-hand-recast.{(f.Id)}.{(m.CardId)}"), $"重铸【{(_coreAdvancedLifecycleOperations.GetAdvancedCard(m.CardId).DisplayName)}】", [m.CardId], [], new Dictionary<string, string> { ["program-action"] = "paired-hand-recast", ["frame-id"] = f.Id.ToString(CultureInfo.InvariantCulture) })).ToArray());
    internal void PublishPairedHandRecastChoice(ProgramSkillFrame f) => _coreParticipantHandProgramsOperations.PublishParticipantHandChoice(f, f.PairedHandRecast!.Partner.Seat, PairedHandRecastChoices(f), "选择一张自己的手牌，与发动者各重铸一张牌。", f.PairedHandRecast.Partner.Seat);
    [ProgramAnswer("paired-hand-recast")]
    internal void ResolvePairedHandRecastChoice(PromptChoice choice)
    {
        var f = _resolutionStackOperations._resolutionStack.LastOrDefault() as ProgramSkillFrame ?? throw new InvalidOperationException("Paired hand recast lost its private choice owner.");
        var r = f.PairedHandRecast!;
        if (r.Stage != PairedHandRecastStage.ChoosingPartner || r.Cursor != 0 || r.Partner.Material is not null || _corePendingDecisionPolicyPipelineOperations._pendingDecision is not { Kind: DecisionKind.ProgramTrigger, IsPrivate: true } prompt || prompt.PlayerSeat != r.Partner.Seat || choice.Parameters.GetValueOrDefault("program-action") != "paired-hand-recast" || choice.Parameters.GetValueOrDefault("frame-id") != f.Id.ToString(CultureInfo.InvariantCulture) || choice.Cards is not [var id] || choice.Targets.Count != 0 || !PairedHandRecastChoices(f).Any(c => c.Id == choice.Id && c.Cards.SequenceEqual(choice.Cards) && c.Targets.SequenceEqual(choice.Targets) && c.Parameters.OrderBy(x => x.Key).SequenceEqual(choice.Parameters.OrderBy(x => x.Key))))
            throw new InvalidOperationException("Paired recast changed its published private chooser, physical hand entity or frame.");
        _coreNativePromptBoundaryOperations.ClearPendingDecision();
        if (_coreMatchOutcomeWinnerAccess._winner != Winner.None || !_coreMatchParticipantsPlayersAccess._players[f.OwnerSeat].IsAlive || !_coreMatchParticipantsPlayersAccess._players[r.Partner.Seat].IsAlive || !_coreProgramLifecycleOperations.CanInvokeProgramSkill(f) || !PairedRecastMaterialStillOwned(r.Owner))
        {
            _coreProgramLifecycleOperations.CancelProgramBindingAndCleanup(f, "双方重铸在选牌完成前失去原发动资格或真实参与者，未支付任何牌。");
            return;
        }

        var material = r.EligiblePartnerMaterials.Single(m => m.CardId == id);
        if (!PairedRecastMaterialStillOwned(r.Partner with { Material = material }))
            throw new InvalidOperationException("The partner must select its original still-owned hand entity.");
        // Both private choices are now frozen. Subsequent source loss cannot
        // revoke this issued operation or exchange an unpaid partner's card.
        _coreRuntimeOperations.ReplaceRuntimeTop(f with { PairedHandRecast = r with { Stage = PairedHandRecastStage.Ready, Partner = r.Partner with { Material = material } } });
        _coreRuntimeOperations.AdvanceRuntimeProgram(f.Id);
    }

    internal void IssuePairedHandRecastCost(ProgramSkillFrame f)
    {
        var r = f.PairedHandRecast!;
        var p = CurrentPairedRecast(r);
        var m = p.Material!;
        if (r.Stage != PairedHandRecastStage.Ready || p.CostIssued || p.Skipped || !PairedRecastMaterialStillOwned(p))
            throw new InvalidOperationException("A pair may pay only its current frozen original hand entity once.");
        var before = PairedHandRecastSequence;
        f = _coreNativeRecastInvocationsOperations.BeginFrameNativeRecastInvocation(f, p.Seat, [m.CardId], r.Cursor);
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { PairedHandRecast = WithCurrentPairedRecast(r, p with { CostIssued = true, CostBefore = before, CostAfter = before })with { Stage = PairedHandRecastStage.CostChildren }, PendingMovementContinuation = new(p.Seat, 0, null) });
        // This is a recast move. Do not route it through discard eligibility or
        // discard-origin payments: an un-discardable hand card can be recast.
        _coreProgramAtomicCardMovesOperations.MoveProgramCardsFromMultipleSources([m.CardId], CardLocation.DiscardPile, CardMoveReasons.RecastDiscard, (batch, records) =>
        {
            if (records is not [var paid] || paid.CardId != m.CardId || paid.CardKind != m.PrintedKind || paid.From != m.From || paid.To != CardLocation.DiscardPile || paid.Reason != CardMoveReasons.RecastDiscard)
                throw new InvalidOperationException("The paired recast did not move its exact physical hand cost.");
            var current = _coreProgramLifecycleOperations.GetActiveProgramFrame(f.Id);
            var receipt = current.PairedHandRecast!;
            _coreRuntimeOperations.ReplaceRuntimeTop(current with { PairedHandRecast = WithCurrentPairedRecast(receipt, CurrentPairedRecast(receipt)with { CostAfter = paid.Sequence, CostBatchId = batch }) });
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new PairedHandRecastPaidEvent(f.Id, receipt.Cursor, p.Seat, m.CardId, m.PrintedKind, m.From, batch, before, paid.Sequence));
        });
        _coreRuntimeOperations.AdvanceRuntimeProgram(f.Id);
    }

    internal void IssuePairedHandRecastDraw(ProgramSkillFrame f)
    {
        var r = f.PairedHandRecast!;
        var p = CurrentPairedRecast(r);
        if (r.Stage != PairedHandRecastStage.CostChildren || !p.CostIssued || p.DrawIssued || f.PendingMovementContinuation is not null)
            throw new InvalidOperationException("A paid paired recast must drain its exact cost before issuing one owed draw.");
        var before = PairedHandRecastSequence;
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { PairedHandRecast = WithCurrentPairedRecast(r, p with { DrawIssued = true, DrawBefore = before, DrawAfter = before })with { Stage = PairedHandRecastStage.DrawChildren }, PendingMovementContinuation = new(p.Seat, 0, null) });
        // A completed cost owes this draw even after the granting skill becomes
        // unqualified. Death or an ended match settles it with actual zero;
        // neither circumstance restores the cost or selects a replacement.
        var actual = _coreMatchOutcomeWinnerAccess._winner == Winner.None && _coreMatchParticipantsPlayersAccess._players[p.Seat].IsAlive ? _corePhysicalDrawFacadesOperations.DrawCards(_coreMatchParticipantsPlayersAccess._players[p.Seat], 1, true, CardMoveReasons.RecastDraw).Count : 0;
        var current = _coreProgramLifecycleOperations.GetActiveProgramFrame(f.Id);
        var receipt = current.PairedHandRecast!;
        _coreRuntimeOperations.ReplaceRuntimeTop(current with { PairedHandRecast = WithCurrentPairedRecast(receipt, CurrentPairedRecast(receipt)with { ActualDrawCount = actual, DrawAfter = PairedHandRecastSequence }) });
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new PairedHandRecastDrawIssuedEvent(f.Id, receipt.Cursor, p.Seat, actual, before, PairedHandRecastSequence));
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new CardRecastEvent(p.Seat, p.Material!.CardId, p.Material.PrintedKind, actual));
        _coreNativeRecastInvocationsOperations.CompleteFrameNativeRecastInvocation(_coreProgramLifecycleOperations.GetActiveProgramFrame(f.Id), actual);
        _coreRuntimeOperations.AdvanceRuntimeProgram(f.Id);
    }

    internal bool DrainPairedHandRecastChildren(ProgramSkillFrame f) => _coreRecoveryReplacementOperations.TryBeginQueuedRecoveryReplacement(f.Id, PostEventContinuation.AwaitedProgramMovement) || _coreCharacterStateProgramsOperations.TryBeginCharacterStateProgramWindow(f.Id, CharacterStateContinuation.Program) || _coreHpChangeProgramsOperations.TryBeginHpChangedProgramWindow(f.Id, PostEventContinuation.AwaitedProgramMovement) || _coreCardMovementProgramsOperations.TryBeginCardsMovedProgramWindow(f.Id) || _coreSkillOwnershipWindowsOperations.TryBeginAdvancedSkillsChanged(f.Id);
    [ProgramContinuation(ProgramContinuationKind.Resume, 70)]
    internal bool ResumePairedHandRecast(long frameId)
    {
        if (_resolutionStackOperations._resolutionStack.LastOrDefault()is not ProgramSkillFrame f || f.Id != frameId || f.PairedHandRecast is not { } r)
            return false;
        if (r.Stage == PairedHandRecastStage.ChoosingPartner)
        {
            if (_corePendingDecisionPolicyPipelineOperations._pendingDecision is null)
                PublishPairedHandRecastChoice(f);
            return true;
        }

        if (r.Stage is PairedHandRecastStage.CostChildren or PairedHandRecastStage.DrawChildren)
        {
            if (DrainPairedHandRecastChildren(f))
                return true;
            _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { PendingMovementContinuation = null });
            if (r.Stage == PairedHandRecastStage.CostChildren)
            {
                IssuePairedHandRecastDraw(f);
                return true;
            }

            r = r with
            {
                Cursor = r.Cursor + 1,
                Stage = PairedHandRecastStage.Ready
            };
            _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { PairedHandRecast = r });
        }

        while (r.Stage == PairedHandRecastStage.Ready && r.Cursor < 2)
        {
            var p = CurrentPairedRecast(r);
            PairedHandRecastSkipReason? skip = _coreMatchOutcomeWinnerAccess._winner != Winner.None ? PairedHandRecastSkipReason.MatchEnded : !_coreMatchParticipantsPlayersAccess._players[p.Seat].IsAlive ? PairedHandRecastSkipReason.ParticipantUnavailable : !PairedRecastMaterialStillOwned(p) ? PairedHandRecastSkipReason.MaterialUnavailable : null;
            if (skip is null)
            {
                IssuePairedHandRecastCost(f);
                return true;
            }

            r = WithCurrentPairedRecast(r, p with { Skipped = true, SkipReason = skip })with
            {
                Cursor = r.Cursor + 1
            };
            _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { PairedHandRecast = r });
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new PairedHandRecastSkippedEvent(f.Id, r.Cursor - 1, p.Seat, skip.Value));
        }

        if (r.Stage == PairedHandRecastStage.Ready && r.Cursor == 2)
        {
            _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { PairedHandRecast = r with { Stage = PairedHandRecastStage.Complete } });
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new PairedHandRecastCompletedEvent(f.Id, r.Owner.CostIssued, r.Partner.CostIssued, r.Owner.ActualDrawCount, r.Partner.ActualDrawCount));
        }

        _coreSkillProgramsOperations.FinishProgramSkill(f, true);
        return true;
    }

    [ProgramContinuation(ProgramContinuationKind.Movement, 50)]
    internal bool ReturnPairedHandRecastMovement(ProgramSkillFrame f)
    {
        if (f.PairedHandRecast is not { Stage: PairedHandRecastStage.CostChildren or PairedHandRecastStage.DrawChildren })
            return false;
        _coreRuntimeOperations.AdvanceRuntimeProgram(f.Id);
        return true;
    }
    [ProgramRuntimeHook(ProgramRuntimePhase.AfterOwnerDeath, 620)]
    [ProgramRuntimeHook(ProgramRuntimePhase.AfterSkillLoss, 640)]
    internal bool CanContinuePairedHandRecast(ProgramSkillFrame f) => f.PairedHandRecast is { Partner.Material: not null } && _corePairedHandRecastObserversOperations.ValidPairedHandRecastReceipt(f);
    [ProgramAiChoice(SkillProgramEffectOp.PairedHandRecast)]
    internal PromptChoice SelectAiPairedHandRecast(PendingDecision decision, ProgramSkillFrame f) => decision.Choices.OrderBy(c => CoreNativeAiDecisionsRuntime.GetKeepValue(_coreAdvancedLifecycleOperations.GetAdvancedCard(c.Cards.Single()), _coreMatchParticipantsPlayersAccess._players[f.PairedHandRecast!.Partner.Seat])).ThenBy(c => c.Cards.Single()).First();
    SkillProgramStepOutcome IPairedHandRecastProgramHost.PairedHandRecast(ProgramSkillFrame f, int partnerSeat) => BeginPairedHandRecast(f, partnerSeat);
}
