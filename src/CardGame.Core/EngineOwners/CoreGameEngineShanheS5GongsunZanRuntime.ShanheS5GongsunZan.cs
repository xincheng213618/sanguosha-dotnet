#nullable enable

using System.Globalization;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CoreGameEngineShanheS5GongsunZanRuntime
{
    internal const string ShanheS5QiaomengReason = "skill-program.shanhe-qiaomeng.discard";
    SkillProgramStepOutcome IShanheS5GongsunProgramHost.DiscardAttackRangeCardsFromSlashTarget(ProgramSkillFrame f) => BeginShanheS5Qiaomeng(f);
    internal Card[] ShanheS5QiaomengCards(int owner, int target) => CoreGameEngineShanheS5GongsunZanDefinitions.ShanheS5QiaomengZones.SelectMany(zone => _cardZonesOperations._cardZones.CardsAt(new(zone, target)).Where(card => !_coreForeignEquipmentDiscardPoliciesOperations.IsForeignEquipmentDiscardPrevented(owner, card, new(zone, target), OwnedCardMoveIntent.Discard))).ToArray();
    [ProgramTriggerLegality(ProgramTriggerLegalityPhase.BeforeGainQualification, 5740)]
    internal bool CanOfferShanheS5Gongsun(ProgramTriggerCandidate candidate, SkillProgramTrigger trigger, ProgramSkillWindowContext context)
    {
        if (trigger.Effects.Any(e => e.Op == SkillProgramEffectOp.DiscardAttackRangeCardsFromSlashTarget))
            return context is { Window: SkillProgramTriggerWindow.CardUseTargetsFinalized, TargetSeat: { } target, CardUse: { } identity } && _coreTableSeatQueriesOperations.IsValidPlayerSeat(target) && _coreMatchParticipantsPlayersAccess._players[target].IsAlive && _coreCardUseLifecycleOperations.LifecycleCardUse(identity.ParentCardUseFrameId)is { Action: { } action } use && action.ActionId == identity.CardActionId && action.ActorSeat == candidate.OwnerSeat && CoreNativeCardClassificationsRuntime.IsSlashCard(use.CardKind) && action.EffectiveDesignatedTargetSeats.Contains(target) && ShanheS5QiaomengCards(candidate.OwnerSeat, target).Length > 0;
        if (candidate.SkillId == "shanhe:s5-jiuzhu-505")
            return context is { Window: SkillProgramTriggerWindow.DyingResponse, TargetSeat: { } victim } && _coreTableSeatQueriesOperations.IsValidPlayerSeat(victim) && _coreMatchParticipantsPlayersAccess._players[candidate.OwnerSeat].HasHpValue && _coreMatchParticipantsPlayersAccess._players[candidate.OwnerSeat].Hp > 1 && _coreTeamActivityProgramsOperations.OtherRevealedPublicTeamAlly(_coreMatchParticipantsPlayersAccess._players[candidate.OwnerSeat], _coreMatchParticipantsPlayersAccess._players[victim]) && _coreMatchParticipantsPlayersAccess._players[victim].Hp <= 0 && _corePhysicalOwnedCardQueriesOperations.GetHand(_coreMatchParticipantsPlayersAccess._players[candidate.OwnerSeat]).Count + _corePhysicalOwnedCardQueriesOperations.GetEquipment(_coreMatchParticipantsPlayersAccess._players[candidate.OwnerSeat]).Count > 0;
        return true;
    }

    internal SkillProgramStepOutcome BeginShanheS5Qiaomeng(ProgramSkillFrame supplied)
    {
        var f = _coreProgramLifecycleOperations.GetActiveProgramFrame(supplied.Id);
        var use = _coreCurrentUsePhysicalClaimsOperations.CurrentFinalTargetSlashOwner(f);
        var target = f.WindowContext!.TargetSeat!.Value;
        if (f.ShanheS5Qiaomeng is not null || f.InstructionIndex != 1)
            throw new InvalidOperationException("The range discard must begin once on its exact designated-target instruction.");
        if (_coreMatchOutcomeStatusWinnerAccess._winner != Winner.None || !_coreMatchParticipantsPlayersAccess._players[f.OwnerSeat].IsAlive || !_coreMatchParticipantsPlayersAccess._players[target].IsAlive || !_coreProgramLifecycleOperations.CanInvokeProgramSkill(f))
            return SkillProgramStepOutcome.Continue;
        var range = _coreNativeRuleQueriesOperations.GetAttackRange(f.OwnerSeat);
        var count = Math.Min(range, ShanheS5QiaomengCards(f.OwnerSeat, target).Length);
        if (count == 0)
            return SkillProgramStepOutcome.Continue;
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { ShanheS5Qiaomeng = new(use.Id, use.Action!.ActionId, target, range, count) });
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new ShanheS5QiaomengStartedEvent(f.Id, use.Id, use.Action.ActionId, f.OwnerSeat, target, range, count));
        PublishShanheS5Qiaomeng(f);
        return SkillProgramStepOutcome.AwaitChoice;
    }

    internal IReadOnlyList<PromptChoice> ShanheS5QiaomengChoices(ProgramSkillFrame f)
    {
        var d = f.ShanheS5Qiaomeng!;
        return Array.AsReadOnly(_coreProgramCardPaymentsOperations.BuildOwnedCardPaymentChoices(f.Id, f.OwnerSeat, d.TargetSeat, CoreGameEngineShanheS5GongsunZanDefinitions.ShanheS5QiaomengZones, OwnedCardMoveIntent.Discard, canSelect: (_, card) => !d.SelectedIds.Contains(card.Id)).Select(choice => choice with { Targets = Array.AsReadOnly(new[] { d.TargetSeat }), Parameters = new System.Collections.ObjectModel.ReadOnlyDictionary<string, string>(new Dictionary<string, string>(choice.Parameters) { ["program-action"] = "shanhe-qiaomeng-card" }) }).ToArray());
    }

    internal void PublishShanheS5Qiaomeng(ProgramSkillFrame f)
    {
        var d = f.ShanheS5Qiaomeng!;
        var choices = ShanheS5QiaomengChoices(f);
        var skill = _contentRegistryOperations._contentRegistry.GetSkill(f.SkillId);
        _corePendingDecisionPolicyPipelineOperations._pendingDecision = new(DecisionKind.ProgramTrigger, f.OwnerSeat, $"【{(skill.Name)}】选择 {(_coreMatchParticipantsPlayersAccess._players[d.TargetSeat].Name)} 的区域牌（{(d.SelectedIds.Count)}/{(d.RequiredCount)}）。", choices.SelectMany(c => c.Cards).Distinct().ToArray(), [d.TargetSeat], f.OwnerSeat)
        {
            PromptId = _coreNativePromptBoundaryOperations.CreatePromptId(),
            IsPrivate = true,
            TargetSeat = d.TargetSeat,
            Choices = choices,
            SkillPrompt = new(f.SkillId, skill.Name, skill.Name, skill.Description)
        };
        _coreMatchOutcomeStatusWinnerAccess._status = _coreMatchParticipantsPlayersAccess._players[f.OwnerSeat].IsHuman ? EngineStatus.AwaitingHumanResponse : EngineStatus.Running;
    }

    [ProgramAnswer("shanhe-qiaomeng-card")]
    internal void ResolveShanheS5QiaomengChoice(PromptChoice choice)
    {
        var f = _resolutionStackOperations._resolutionStack.LastOrDefault() as ProgramSkillFrame ?? throw new InvalidOperationException("The range discard lost its owner.");
        var d = f.ShanheS5Qiaomeng ?? throw new InvalidOperationException("The range discard lost its original selection.");
        if (d.Paid || _corePendingDecisionPolicyPipelineOperations._pendingDecision?.PlayerSeat != f.OwnerSeat || !CoreGameEngineAssistedPhysicalCardsRuntime.AssistedChoicesEqual([choice], ShanheS5QiaomengChoices(f).Where(c => c.Id == choice.Id).ToArray()))
            throw new InvalidOperationException("The range discard no longer matches a published private zone slot.");
        _coreNativePromptBoundaryOperations.ClearPendingDecision();
        if (_coreMatchOutcomeStatusWinnerAccess._winner != Winner.None || !_coreMatchParticipantsPlayersAccess._players[f.OwnerSeat].IsAlive || !_coreMatchParticipantsPlayersAccess._players[d.TargetSeat].IsAlive || !_coreProgramLifecycleOperations.CanInvokeProgramSkill(f))
        {
            _coreProgramLifecycleOperations.CancelProgramBindingAndCleanup(f, "趫猛的未支付来源或目标失效。");
            return;
        }

        var zone = Enum.Parse<CardZoneKind>(choice.Parameters["source-zone"]);
        var slot = int.Parse(choice.Parameters["slot-index"], CultureInfo.InvariantCulture);
        var from = new CardLocation(zone, d.TargetSeat);
        var card = _cardZonesOperations._cardZones.CardsAt(from)[slot];
        var ids = d.SelectedIds.Append(card.Id).ToArray();
        var locations = d.OriginalLocations.Append(from).ToArray();
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { ShanheS5Qiaomeng = d with { SelectedIds = ids, OriginalLocations = locations } });
        if (ids.Length < d.RequiredCount)
        {
            PublishShanheS5Qiaomeng(f);
            return;
        }

        if (ids.Length != d.RequiredCount || ids.Select((id, index) => _cardZonesOperations._cardZones.GetLocation(id) != locations[index] || !ShanheS5QiaomengCards(f.OwnerSeat, d.TargetSeat).Any(c => c.Id == id)).Any(invalid => invalid))
            throw new InvalidOperationException("The complete unspent range-discard selection changed its original zone or legality.");
        var before = _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence;
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { ShanheS5Qiaomeng = f.ShanheS5Qiaomeng!with { Paid = true, Before = before, After = before }, PendingMovementContinuation = new(d.TargetSeat, 0, null) });
        _coreNativeLogicalDiscardsOperations.MoveProgramLogicalDiscard(f, f.OwnerSeat, ids, new(ShanheS5QiaomengReason), (batch, records) =>
        {
            foreach (var move in records)
                _characterBianFuRenOlOperations.RecordBianFuRenForeignDiscardProducer(move, f.OwnerSeat, f.Id);
            var root = _coreProgramLifecycleOperations.GetActiveProgramFrame(f.Id);
            _coreRuntimeOperations.ReplaceRuntimeTop(root with { ShanheS5Qiaomeng = root.ShanheS5Qiaomeng!with { BatchId = batch, After = _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence } });
        });
        f = _coreProgramLifecycleOperations.GetActiveProgramFrame(f.Id);
        var after = _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence;
        _coreRuntimeOperations.ReplaceRuntimeTop(f with { ShanheS5Qiaomeng = f.ShanheS5Qiaomeng!with { After = after } });
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new ShanheS5QiaomengPaidEvent(f.Id, d.UseFrameId, d.TargetSeat, ids.Length, before, after));
        _coreRuntimeOperations.AdvanceRuntimeProgram(f.Id);
    }

    [ProgramContinuation(ProgramContinuationKind.Resume, 5740)]
    internal bool ResumeShanheS5Qiaomeng(long id)
    {
        if (_resolutionStackOperations._resolutionStack.LastOrDefault()is not ProgramSkillFrame f || f.Id != id || f.ShanheS5Qiaomeng is not { Paid: true })
            return false;
        if (_coreRecoveryReplacementOperations.TryBeginQueuedRecoveryReplacement(id, PostEventContinuation.AwaitedProgramMovement) || _coreCharacterStateProgramsOperations.TryBeginCharacterStateProgramWindow(id, CharacterStateContinuation.Program) || _coreHpChangeProgramsOperations.TryBeginHpChangedProgramWindow(id, PostEventContinuation.AwaitedProgramMovement) || _coreCardMovementProgramsOperations.TryBeginCardsMovedProgramWindow(id) || _coreSkillOwnershipWindowsOperations.TryBeginAdvancedSkillsChanged(id))
            return true;
        _coreRuntimeOperations.ReplaceRuntimeTop(f with { ShanheS5Qiaomeng = null, PendingMovementContinuation = null });
        _coreRuntimeOperations.AdvanceRuntimeProgram(id);
        return true;
    }

    [ProgramContinuation(ProgramContinuationKind.Movement, 5740)]
    internal bool ReturnShanheS5QiaomengMovement(ProgramSkillFrame f)
    {
        if (f.ShanheS5Qiaomeng is not { Paid: true })
            return false;
        _coreRuntimeOperations.AdvanceRuntimeProgram(f.Id);
        return true;
    }

    [ProgramAiChoice(SkillProgramEffectOp.DiscardAttackRangeCardsFromSlashTarget)]
    internal PromptChoice SelectAiShanheS5Qiaomeng(PendingDecision decision, ProgramSkillFrame f)
    {
        var(choice, thought) = _coreAiBrainsAiBrainsAccess._aiBrains[decision.PlayerSeat].ChooseOtherOwnedCardDiscard(_corePlayerViewProjectionOperations.CreateSnapshot(decision.PlayerSeat), decision.Choices, _contentRegistryOperations._contentRegistry.GetSkill(f.SkillId).Name, ++_coreAiDiagnosticsThoughtSequenceAccess._thoughtSequence);
        _coreNativeAiDecisionsOperations.AddThought(thought);
        return choice;
    }
}
