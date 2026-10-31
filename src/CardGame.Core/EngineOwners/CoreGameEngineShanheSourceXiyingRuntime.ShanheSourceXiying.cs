#nullable enable

using System.Globalization;
using CardGame.Core.Campaigns;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CoreGameEngineShanheSourceXiyingRuntime
{
    internal const string SourceXiyingDiscard = "skill-program.source-xiying-1180.discard", SourceXiyingReward = "skill-program.source-xiying-1180.reward";
    internal static CardConversionSource SourceXiyingSource(ProgramSkillFrame f) => new(f.SkillId, CoreGameEngineProgramLifecycleRuntime.GetProgramBindingId(f), f.OwnerSeat, f.SkillInstanceId);
    internal bool SourceXiyingMode => _coreMatchDefinitionModeDefinitionOptionsAccess._modeDefinition.Id == ShanheBattleSetup.BattleModeId && _coreMatchDefinitionModeDefinitionOptionsAccess._options.ShanheBattle is not null;

    SkillProgramStepOutcome IShanheSourceXiyingHost.Resolve(ProgramSkillFrame f) => BeginSourceXiying(f);
    internal bool SourceXiyingPlayParent(int owner, ProgramTriggerCandidate candidate, ProgramSkillWindowContext c) => SourceXiyingMode && c.Window == SkillProgramTriggerWindow.PlayPhaseStarting && c.OwnerSeat == owner && c.SourceSeat == owner && c.TargetSeat == owner && owner == _coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess._currentSeat && _coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess._phase == TurnPhase.Play && _coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess._turnNumber > 0 && _coreGameEngineCardUseDebitsCardUseDebitPhaseInstanceIdAccess._cardUseDebitPhaseInstanceId > 0 && _resolutionStackOperations._resolutionStack.OfType<PlayPhaseStartingBoundaryFrame>().SingleOrDefault(p => p.Id == c.ParentFrameId)is { } p && p.OwnerSeat == owner && p.ItemIndex >= 0 && p.ItemIndex < p.Items.Count && p.Items[p.ItemIndex].Candidate == candidate;
    internal IReadOnlyList<PromptChoice> SourceXiyingPaymentChoices(ProgramSkillFrame f, int actor, bool initial) => _coreProgramCardPaymentsOperations.BuildOwnedCardPaymentChoices(f.Id, actor, actor, initial ? [CardZoneKind.Hand] : [CardZoneKind.Hand, CardZoneKind.Equipment], OwnedCardMoveIntent.Discard).Where(c => c.Cards.Count == 1 && (!initial || CardCatalog.Get(_coreAdvancedLifecycleOperations.GetAdvancedCard(c.Cards.Single()).Kind).CategoryName != "基本牌")).ToArray();
    internal SkillProgramStepOutcome BeginSourceXiying(ProgramSkillFrame supplied)
    {
        var f = _coreProgramLifecycleOperations.GetActiveProgramFrame(supplied.Id);
        if (f.SkillId != ShanheSourceXiyingContract.Skill || f.InstructionIndex != 1 || f.TriggerId is null || f.ActivationId != f.TriggerId || f.ShanheSourceXiying is not null || f.SelectedCardIds.Count != 0 || f.SelectedTargetSeats.Count != 0 || f.WindowContext is not { } c)
            throw new InvalidOperationException("Source1180 requires its original zero-input native phase binding.");
        if (f.TriggerId == ShanheSourceXiyingContract.Ending)
            return _coreShanheSourceXiyingEndingOperations.Value.BeginSourceXiyingReward(f);
        var candidate = new ProgramTriggerCandidate(f.OwnerSeat, f.SkillId, f.TriggerId, f.SkillInstanceId, f.GameplayHash, _coreProgramLifecycleOperations.GetProgramTrigger(f).Priority, c.OccurrenceIndex);
        if (!SourceXiyingPlayParent(f.OwnerSeat, candidate, c) || !_coreMatchParticipantsPlayersAccess._players[f.OwnerSeat].IsAlive || SourceXiyingPaymentChoices(f, f.OwnerSeat, true).Count == 0)
            throw new InvalidOperationException("Source1180 must discard one actual non-basic hand entity at its real own Play start.");
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { ShanheSourceXiying = new(c.ParentFrameId, _coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess._turnNumber, _coreGameEngineCardUseDebitsCardUseDebitPhaseInstanceIdAccess._cardUseDebitPhaseInstanceId, ShanheSourceXiyingStage.Cost) { Seats = _coreMatchParticipantsPlayersAccess._players.Where(p => p.IsAlive && p.Seat != f.OwnerSeat).OrderBy(p => _coreTableOrderOperations.RelativeTableOrder(f.OwnerSeat, p.Seat)).Select(p => p.Seat).ToArray() } });
        ContinueSourceXiying(f.Id);
        return SkillProgramStepOutcome.AwaitChoice;
    }

    internal IReadOnlyList<PromptChoice> SourceXiyingChoices(ProgramSkillFrame f)
    {
        var r = f.ShanheSourceXiying!;
        var actor = r.Stage == ShanheSourceXiyingStage.Cost ? f.OwnerSeat : r.Seats[r.Index];
        PromptChoice Make(string branch, string label, IReadOnlyList<int> cards) => new(new($"source-xiying.{(f.Id)}.{(r.Stage)}.{(r.Index)}.{(branch)}.{(string.Join('-', cards))}"), label, cards, [], new Dictionary<string, string> { ["program-action"] = "source-xiying", ["frame-id"] = f.Id.ToString(CultureInfo.InvariantCulture), ["index"] = r.Index.ToString(CultureInfo.InvariantCulture), ["branch"] = branch });
        var choices = SourceXiyingPaymentChoices(f, actor, r.Stage == ShanheSourceXiyingStage.Cost).Select(c => Make("discard", c.Label, c.Cards)).ToList();
        if (r.Stage == ShanheSourceXiyingStage.Targets)
            choices.Add(Make("ban", "本回合不能使用或打出牌", []));
        return Array.AsReadOnly(choices.Select(PromptChoiceProjection.Freeze).ToArray());
    }

    internal void PublishSourceXiying(ProgramSkillFrame f)
    {
        var r = f.ShanheSourceXiying!;
        var choices = SourceXiyingChoices(f);
        if (choices.Count == 0)
        {
            FinishSourceXiying(f);
            return;
        }

        var actor = r.Stage == ShanheSourceXiyingStage.Cost ? f.OwnerSeat : r.Seats[r.Index];
        var skill = _contentRegistryOperations._contentRegistry.GetSkill(f.SkillId);
        _corePendingDecisionPolicyPipelineOperations._pendingDecision = new(DecisionKind.ProgramTrigger, actor, actor == f.OwnerSeat ? "袭营：弃置手中一张非基本牌。" : "袭营：弃置一张牌，或本回合不能使用、打出牌。", choices.SelectMany(c => c.Cards).Distinct().ToArray(), [], f.OwnerSeat)
        {
            PromptId = _coreNativePromptBoundaryOperations.CreatePromptId(),
            IsPrivate = true,
            TargetSeat = actor,
            Choices = choices,
            SkillPrompt = new(f.SkillId, skill.Name, skill.Name, skill.Description)
        };
        _coreMatchOutcomeStatusWinnerAccess._status = _coreMatchParticipantsPlayersAccess._players[actor].IsHuman ? EngineStatus.AwaitingHumanResponse : EngineStatus.Running;
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceRulesAndPublishState();
    }

    [ProgramAnswer("source-xiying")]
    internal void AnswerSourceXiying(PromptChoice selected)
    {
        if (_resolutionStackOperations._resolutionStack.LastOrDefault()is not ProgramSkillFrame { ShanheSourceXiying: { } r } f || r.Stage is not (ShanheSourceXiyingStage.Cost or ShanheSourceXiyingStage.Targets) || selected.Parameters.GetValueOrDefault("frame-id") != f.Id.ToString(CultureInfo.InvariantCulture) || !SourceXiyingChoices(f).Any(c => CoreGameEngineAssistedPhysicalCardsRuntime.AssistedChoicesEqual([c], [selected])))
            throw new InvalidOperationException("Source1180 lost its exact current owner/other-character payment choice.");
        _coreNativePromptBoundaryOperations.ClearPendingDecision();
        var target = r.Stage == ShanheSourceXiyingStage.Cost ? f.OwnerSeat : r.Seats[r.Index];
        if (selected.Parameters["branch"] == "ban")
        {
            if (r.Stage != ShanheSourceXiyingStage.Targets || target == f.OwnerSeat)
                throw new InvalidOperationException("Only the current other character can accept this exact ban.");
            _coreCardUseModulesOperations.ValidateProgramTurnEffectGrant(f);
            var ban = _turnCardUseEffectsOperations._turnCardUseEffects.GrantTargetRestriction(_coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess._turnNumber, _coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess._currentSeat, f.Id, -r.Index - 1, CoreGameEngineCardUseModulesRuntime.CreateProgramTurnEffectSource(f), SkillProgramCardTargetRestriction.CannotUseOrPlayForTurn, target);
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new CardTargetRestrictionGrantedEvent(ban));
            _coreRuntimeOperations.ReplaceRuntimeTop(f with { ShanheSourceXiying = r with { Index = r.Index + 1 } });
        }
        else
        {
            var id = selected.Cards.Single();
            var before = _coreCardMovementJournalMovementSequenceAccess._movementSequence;
            var initial = r.Stage == ShanheSourceXiyingStage.Cost;
            _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { ShanheSourceXiying = initial ? r with { Stage = ShanheSourceXiyingStage.CostChildren, CostCardId = id, CostBefore = before, CostAfter = before } : r with { Stage = ShanheSourceXiyingStage.TargetChildren, TargetSeat = target, TargetCardId = id, TargetBefore = before, TargetAfter = before } });
            _coreNativeLogicalDiscardsOperations.MoveProgramLogicalDiscard(f, target, [id], new(SourceXiyingDiscard), (_, moves) =>
            {
                var active = _coreProgramLifecycleOperations.GetActiveProgramFrame(f.Id);
                var current = active.ShanheSourceXiying!;
                if (moves.Count != 1 || moves[0].CardId != id || moves[0].To != CardLocation.DiscardPile && moves[0].To != CardLocation.OutsideGame)
                    throw new InvalidOperationException("Source1180 must really discard its exact selected physical card once.");
                _coreRuntimeOperations.ReplaceRuntimeTop(active with { ShanheSourceXiying = initial ? current with { CostAfter = _coreCardMovementJournalMovementSequenceAccess._movementSequence } : current with { TargetAfter = _coreCardMovementJournalMovementSequenceAccess._movementSequence } });
                if (initial)
                    _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new ShanheSourceXiyingPaidEvent(f.Id, SourceXiyingSource(active), active.GameplayHash, current.ActualTurn, current.PlayPhaseInstance, id, before, _coreCardMovementJournalMovementSequenceAccess._movementSequence));
                else
                    _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new ShanheSourceXiyingTargetPaidEvent(f.Id, target, id, before, _coreCardMovementJournalMovementSequenceAccess._movementSequence));
            });
        }

        ContinueSourceXiying(f.Id);
    }

    internal bool DrainSourceXiying(long id) => _coreRecoveryReplacementOperations.TryBeginQueuedRecoveryReplacement(id, PostEventContinuation.Program) || _coreCharacterStateProgramsOperations.TryBeginCharacterStateProgramWindow(id, CharacterStateContinuation.Program) || _coreHpChangeProgramsOperations.TryBeginHpChangedProgramWindow(id, PostEventContinuation.Program) || _coreCardMovementProgramsOperations.TryBeginCardsMovedProgramWindow(id) || _coreSkillOwnershipWindowsOperations.TryBeginAdvancedSkillsChanged(id);
    internal void ContinueSourceXiying(long id)
    {
        if (_resolutionStackOperations._resolutionStack.LastOrDefault()is not ProgramSkillFrame { ShanheSourceXiying: { } r } f || f.Id != id)
            return;
        if (r.Stage is ShanheSourceXiyingStage.CostChildren or ShanheSourceXiyingStage.TargetChildren or ShanheSourceXiyingStage.RewardChildren)
        {
            if (DrainSourceXiying(id))
                return;
            f = _coreProgramLifecycleOperations.GetActiveProgramFrame(id);
            r = f.ShanheSourceXiying!;
            if (r.Stage == ShanheSourceXiyingStage.RewardChildren)
            {
                FinishSourceXiying(f);
                return;
            }

            _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { ShanheSourceXiying = r = r with { Stage = ShanheSourceXiyingStage.Targets, Index = r.Stage == ShanheSourceXiyingStage.TargetChildren ? r.Index + 1 : r.Index } });
        }

        if (_coreMatchOutcomeStatusWinnerAccess._winner != Winner.None)
        {
            FinishSourceXiying(f);
            return;
        }

        if (r.Stage == ShanheSourceXiyingStage.Targets)
        {
            while (r.Index < r.Seats.Count && !_coreMatchParticipantsPlayersAccess._players[r.Seats[r.Index]].IsAlive)
                _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { ShanheSourceXiying = r = r with { Index = r.Index + 1 } });
            if (r.Index == r.Seats.Count)
            {
                FinishSourceXiying(f);
                return;
            }
        }

        if (_corePendingDecisionPolicyPipelineOperations._pendingDecision is null)
            PublishSourceXiying(f);
    }

    internal void FinishSourceXiying(ProgramSkillFrame f)
    {
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { ShanheSourceXiying = null });
        _coreSkillProgramsOperations.FinishProgramSkill(f, true);
    }

    [ProgramContinuation(ProgramContinuationKind.Resume, 62063)]
    internal bool ResumeSourceXiying(long id)
    {
        if (_resolutionStackOperations._resolutionStack.LastOrDefault()is not ProgramSkillFrame { ShanheSourceXiying: not null } f || f.Id != id)
            return false;
        ContinueSourceXiying(id);
        return true;
    }

    [ProgramAiChoice(SkillProgramEffectOp.ResolveShanheSourceXiying)]
    internal PromptChoice SelectSourceXiyingAi(PendingDecision p, ProgramSkillFrame f) => p.Choices.FirstOrDefault(c => c.Cards.Count == 1) ?? p.Choices.Single();
}
