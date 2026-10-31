#nullable enable

using System.Globalization;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterLuKangWumiaoRuntime
{
    internal const string WuLuKangDixianDrawReason = "skill-program.wumiao:dixian.draw";
    internal const string WuLuKangDixianObtainReason = "skill-program.wumiao:dixian.obtain";
    internal bool HasWuLuKangDixianCapability => _contentRegistryOperations._contentRegistry.ProgramDependencies.HasActivationOperation(SkillProgramEffectOp.ResolveWuLuKangDixian);

    internal static CardConversionSource WuLuKangDixianSource(ProgramSkillFrame f) => RuntimeSkills.RuntimeRules_lu_kang_wumiao_WuLuKangDixian.WuLuKangDixianSource(f);
    internal bool HasWuLuKangDixianOriginalPayment(ProgramSkillFrame f)
    {
        if (f.SkillId != WuLuKangComposition.Dixian || f.ActivationId != "declare-rank" || f.TriggerId is not null || f.InstructionIndex != 1 || f.SelectedCardIds.Count != 0 || f.SelectedTargetSeats.Count != 0 || _contentRegistryOperations._contentRegistry.GetSkill(f.SkillId).Program is not { } p || p.GameplayHash != f.GameplayHash || !WuLuKangComposition.IsDixian(p))
            return false;
        var history = _coreAlternatingBenefitsOperations.CompleteProgramEventHistory().ToArray();
        var starts = history.Select((e, i) => (e, i)).Where(x => x.e is ProgramSkillStartedEvent s && s.FrameId == f.Id && s.OwnerSeat == f.OwnerSeat && s.SkillId == f.SkillId && s.ActivationId == f.ActivationId).ToArray();
        return starts is [var start] && history.Take(start.i).OfType<SkillUsageConsumedEvent>().LastOrDefault(e => e.SkillOwnerSeat == f.OwnerSeat && e.SkillId == f.SkillId && e.UsageId == f.ActivationId && e.Scope == SkillUsageScope.Game)is { Count: 1 };
    }

    internal SkillProgramStepOutcome BeginWuLuKangDixian(ProgramSkillFrame supplied)
    {
        var f = _coreProgramLifecycleOperations.GetActiveProgramFrame(supplied.Id);
        if (!HasWuLuKangDixianOriginalPayment(f) || f.WuLuKangDixian is not null || _coreActualDiscardRecoveryPhasesOperations.CurrentActualDiscardRecoveryPhase()is not { Kind: ActualDiscardRecoveryPhaseKind.Play } phase || phase.ActorSeat != f.OwnerSeat || _coreProgramEventHistoryOperations.Value.ProgramEventHistory<WuLuKangDixianStartedEvent>().Any(e => e.FrameId == f.Id))
            throw new InvalidOperationException("Dixian requires its exact once-paid native Play activation.");
        var r = new WuLuKangDixianReceipt(WuLuKangDixianSource(f), f.GameplayHash, phase, _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence)
        {
            Stage = WuLuKangDixianStage.Choosing
        };
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { WuLuKangDixian = r });
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new WuLuKangDixianStartedEvent(f.Id, r.Source, r.GameplayHash, phase, r.SequenceBefore));
        _coreParticipantHandProgramsOperations.PublishParticipantHandChoice(f, f.OwnerSeat, WuLuKangDixianChoices(f), "声明一个点数（A至K）。", f.OwnerSeat);
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceRulesAndPublishState();
        return SkillProgramStepOutcome.AwaitChild;
    }

    internal static IReadOnlyList<PromptChoice> WuLuKangDixianChoices(ProgramSkillFrame f) => RuntimeSkills.RuntimeRules_lu_kang_wumiao_WuLuKangDixian.WuLuKangDixianChoices(f);
    [ProgramAnswer("wu-lu-kang-dixian")]
    internal void AnswerWuLuKangDixian(PromptChoice choice)
    {
        if (_resolutionStackOperations._resolutionStack.LastOrDefault()is not ProgramSkillFrame { WuLuKangDixian: { Stage: WuLuKangDixianStage.Choosing } r } f || _corePendingDecisionPolicyPipelineOperations._pendingDecision?.PlayerSeat != f.OwnerSeat || !HasWuLuKangDixianOriginalPayment(f) || !_coreMatchParticipantsPlayersAccess._players[f.OwnerSeat].IsAlive || !_coreNationalSkillsOperations.HasRuntimeSkillInstance(_coreMatchParticipantsPlayersAccess._players[f.OwnerSeat], f.SkillId, f.SkillInstanceId) || !CoreGameEngineAssistedPhysicalCardsRuntime.AssistedChoicesEqual([choice], WuLuKangDixianChoices(f).Where(c => c.Id == choice.Id).ToArray()))
            throw new InvalidOperationException("Dixian accepts only its owner's current declared-rank choice.");
        RequireWuLuKangDixian(f);
        var rank = int.Parse(choice.Parameters["rank"], CultureInfo.InvariantCulture);
        var stock = _cardZonesOperations._cardZones.CardsAt(CardLocation.DrawPile).ToArray();
        var allAtLeast = stock.All(c => c.Rank >= rank);
        // Field cards are Equipment/Judgment. Hands and every held/private pile
        // are deliberately outside this frozen, printed-rank collection.
        var locations = _coreMatchParticipantsPlayersAccess._players.SelectMany(p => new[] { CardLocation.Equipment(p.Seat), CardLocation.Judgment(p.Seat) }).Concat([CardLocation.DrawPile, CardLocation.DiscardPile]);
        var materials = allAtLeast ? Array.Empty<WuLuKangDixianMaterial>() : locations.SelectMany(loc => _cardZonesOperations._cardZones.CardsAt(loc).Where(c => c.Rank == 13).Select(c => new WuLuKangDixianMaterial(c.Id, c.Kind, c.Rank, loc))).ToArray();
        _coreNativePromptBoundaryOperations.ClearPendingDecision();
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { WuLuKangDixian = r with { Rank = rank, AllAtLeast = allAtLeast, StockCount = stock.Length, Materials = materials, Stage = allAtLeast ? WuLuKangDixianStage.Drawing : WuLuKangDixianStage.Collecting } });
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new WuLuKangDixianRankChosenEvent(f.Id, rank, allAtLeast, stock.Length, materials.Length));
        for (var i = 0; i < materials.Length; i++)
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new WuLuKangDixianMaterialFrozenEvent(f.Id, i, materials[i]));
        if (allAtLeast)
            _coreProgramNativeDrawOperations.BeginOperationProgramDraw(f, f.OwnerSeat, rank, new(WuLuKangDixianDrawReason));
        else
            ContinueWuLuKangDixian(f.Id);
    }

    [ProgramAiChoice(SkillProgramEffectOp.ResolveWuLuKangDixian)]
    internal PromptChoice AiWuLuKangDixian(PendingDecision p, ProgramSkillFrame f) => p.Choices.Single(c => c.Parameters.GetValueOrDefault("rank") == "13");
    [ProgramNativeDrawReturn(SkillProgramEffectOp.ResolveWuLuKangDixian)]
    internal void ReturnWuLuKangDixianDraw(ProgramSkillFrame f, NativeDrawOutcome o)
    {
        if (f.WuLuKangDixian is not { Stage: WuLuKangDixianStage.Drawing, AllAtLeast: true } r || o.RecipientSeat != f.OwnerSeat || o.RequestedCount != r.Rank || _coreProgramEventHistoryOperations.Value.ProgramEventHistory<WuLuKangDixianDrawReturnedEvent>().Any(e => e.FrameId == f.Id))
            throw new InvalidOperationException("Dixian requires its exact once-returned requested native draw.");
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { WuLuKangDixian = r with { DrawOutcome = o, Stage = WuLuKangDixianStage.DrawChildren }, PendingMovementContinuation = new(f.OwnerSeat, 0, null) });
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new WuLuKangDixianDrawReturnedEvent(f.Id, o.RequestedCount, o.ActualDrawIds.Count, o.ReplacementGainedIds.Count, o.SequenceBefore, o.SequenceAfter, o.AttemptFrameId));
        ContinueWuLuKangDixian(f.Id);
    }

    internal void ContinueWuLuKangDixian(long id)
    {
        if (_corePendingDecisionPolicyPipelineOperations._pendingDecision is not null || _resolutionStackOperations._resolutionStack.LastOrDefault()is not ProgramSkillFrame { WuLuKangDixian: { } r } f || f.Id != id)
            return;
        RequireWuLuKangDixian(f);
        if (r.Stage is WuLuKangDixianStage.Choosing or WuLuKangDixianStage.Drawing)
            throw new InvalidOperationException("Dixian cannot resume before its accepted rank or native draw returns.");
        if (AwaitWuLuKangDixianChildren(id))
            return;
        f = _coreProgramLifecycleOperations.GetActiveProgramFrame(id);
        r = f.WuLuKangDixian!;
        if (r.AllAtLeast || !_coreMatchParticipantsPlayersAccess._players[f.OwnerSeat].IsAlive || _coreMatchOutcomeStatusWinnerAccess._winner != Winner.None)
        {
            FinishWuLuKangDixian(f);
            return;
        }

        while (r.MaterialIndex < r.Materials.Count)
        {
            var index = r.MaterialIndex;
            var material = r.Materials[index];
            if (_cardZonesOperations._cardZones.GetLocation(material.CardId) != material.From)
            {
                _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { WuLuKangDixian = r = r with { MaterialIndex = index + 1, Stage = WuLuKangDixianStage.Collecting } });
                _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new WuLuKangDixianEntitySkippedEvent(id, index, material.CardId, "left-frozen-source"));
                continue;
            }

            _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { WuLuKangDixian = r with { Stage = WuLuKangDixianStage.MovementChildren }, PendingMovementContinuation = new(f.OwnerSeat, 0, null) });
            _corePhysicalMovementFacadesOperations.MoveCard(_coreAdvancedLifecycleOperations.GetAdvancedCard(material.CardId), material.From, CardLocation.Hand(f.OwnerSeat), new(WuLuKangDixianObtainReason), move =>
            {
                var current = _coreProgramLifecycleOperations.GetActiveProgramFrame(id);
                var receipt = current.WuLuKangDixian!;
                if (receipt.MaterialIndex != index || move.CardId != material.CardId || move.From != material.From)
                    throw new InvalidOperationException("Dixian cannot replace or repeat its frozen K entity.");
                _coreRuntimeOperations.ReplaceRuntimeFrame(id, current with { WuLuKangDixian = receipt with { MaterialIndex = index + 1, LastMovementSequence = move.Sequence, ActualGained = receipt.ActualGained + (move.To == CardLocation.Hand(f.OwnerSeat) ? 1 : 0) } });
                _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new WuLuKangDixianEntityMovedEvent(id, index, move.CardId, move.From, move.To, move.Sequence));
            }, disclosure: material.From == CardLocation.DrawPile ? CardMovementDisclosure.FaceDown : CardMovementDisclosure.FaceUp);
            if (AwaitWuLuKangDixianChildren(id))
                return;
            f = _coreProgramLifecycleOperations.GetActiveProgramFrame(id);
            r = f.WuLuKangDixian!;
            if (!_coreMatchParticipantsPlayersAccess._players[f.OwnerSeat].IsAlive || _coreMatchOutcomeStatusWinnerAccess._winner != Winner.None)
            {
                FinishWuLuKangDixian(f);
                return;
            }
        }

        FinishWuLuKangDixian(f);
    }

    internal bool AwaitWuLuKangDixianChildren(long id)
    {
        var f = _coreProgramLifecycleOperations.GetActiveProgramFrame(id);
        _coreRuntimeOperations.ReplaceRuntimeTop(f with { PendingMovementContinuation = new(f.OwnerSeat, 0, null) });
        if (_coreRecoveryReplacementOperations.TryBeginQueuedRecoveryReplacement(id, PostEventContinuation.AwaitedProgramMovement) || _coreCharacterStateProgramsOperations.TryBeginCharacterStateProgramWindow(id, CharacterStateContinuation.Program) || _coreHpChangeProgramsOperations.TryBeginHpChangedProgramWindow(id, PostEventContinuation.AwaitedProgramMovement) || _coreCardMovementProgramsOperations.TryBeginCardsMovedProgramWindow(id) || _coreSkillOwnershipWindowsOperations.TryBeginAdvancedSkillsChanged(id))
            return true;
        _coreRuntimeOperations.ReplaceRuntimeTop(_coreProgramLifecycleOperations.GetActiveProgramFrame(id)with { PendingMovementContinuation = null });
        return false;
    }

    internal void FinishWuLuKangDixian(ProgramSkillFrame f)
    {
        RequireWuLuKangDixian(f);
        var r = f.WuLuKangDixian!;
        var grant = r.AllAtLeast && r.DrawOutcome is not null && _coreMatchParticipantsPlayersAccess._players[f.OwnerSeat].IsAlive && _coreMatchOutcomeStatusWinnerAccess._winner == Winner.None;
        if (grant)
        {
            if (WuLuKangDixianGrantFor(f.OwnerSeat)is not null)
                throw new InvalidOperationException("Dixian cannot issue its permanent privilege twice.");
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new WuLuKangDixianGrantedEvent(new(f.Id, r.Source, r.GameplayHash, r.Rank, r.Phase)));
        }

        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new WuLuKangDixianCompletedEvent(f.Id, r.Rank, r.AllAtLeast, r.ActualGained, grant));
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { WuLuKangDixian = null, PendingMovementContinuation = null });
        _coreSkillProgramsOperations.FinishProgramSkill(f, true);
    }

    [ProgramContinuation(ProgramContinuationKind.Resume, 29701)]
    internal bool ResumeWuLuKangDixian(long id)
    {
        if (_resolutionStackOperations._resolutionStack.LastOrDefault()is not ProgramSkillFrame { WuLuKangDixian: not null } f || f.Id != id)
            return false;
        ContinueWuLuKangDixian(id);
        return true;
    }

    [ProgramContinuation(ProgramContinuationKind.Movement, 29701)]
    internal bool ReturnWuLuKangDixianMovement(ProgramSkillFrame f)
    {
        if (f.WuLuKangDixian is null || f.PendingMovementContinuation is null)
            return false;
        ContinueWuLuKangDixian(f.Id);
        return true;
    }

    SkillProgramStepOutcome IWuLuKangDixianHost.Resolve(ProgramSkillFrame f) => BeginWuLuKangDixian(f);
}
