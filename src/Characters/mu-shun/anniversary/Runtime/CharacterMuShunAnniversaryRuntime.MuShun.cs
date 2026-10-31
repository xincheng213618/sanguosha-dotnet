#nullable enable

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterMuShunAnniversaryRuntime
{
    internal const string MuShunAction = "anniversary-mu-shun";
    internal static CardConversionSource MuShunSource(ProgramSkillFrame f) => new(f.SkillId, f.TriggerId!, f.OwnerSeat, f.SkillInstanceId);
    internal int MuShunJin(int seat) => _coreMatchParticipantsPlayersAccess._players[seat].Markers.GetValueOrDefault(PlayerMarkerKind.MuShunJin);
    internal bool HasMuShunCapability => _contentRegistryOperations._contentRegistry.ProgramDependencies.HasTriggerOperation(SkillProgramEffectOp.ResolveAnniversaryMuShunJinjian);
    internal bool HasMuShunShizhaoCapability => _contentRegistryOperations._contentRegistry.ProgramDependencies.HasTriggerOperation(SkillProgramEffectOp.ResolveAnniversaryMuShunShizhao);

    SkillProgramStepOutcome IAnniversaryMuShunHost.Resolve(ProgramSkillFrame frame, SkillProgramEffectOp operation) => operation == SkillProgramEffectOp.ResolveAnniversaryMuShunJinjian ? BeginMuShunJinjian(frame) : ResolveMuShunShizhao(frame);
    [ProgramTriggerLegality(ProgramTriggerLegalityPhase.BeforeGainQualification, 45300)]
    internal bool QualifyMuShun(ProgramTriggerLegalityContext g)
    {
        if (g.SkillId == MuShunComposition.Jinjian)
            return g.WindowContext is { Window: SkillProgramTriggerWindow.AfterDamageApplied, Amount: > 0 } c && (c.SourceSeat == g.OwnerSeat || c.TargetSeat == g.OwnerSeat);
        return g.SkillId != MuShunComposition.Shizhao || g.WindowContext is { } context && MuShunLossCandidate(g.Candidate, context, false);
    }

    internal void SetMuShunJin(ProgramSkillFrame f, int count)
    {
        var owner = _coreMatchParticipantsPlayersAccess._players[f.OwnerSeat];
        var before = MuShunJin(f.OwnerSeat);
        if (count < 0)
            throw new InvalidOperationException("Jin cannot be negative.");
        var key = (PlayerMarkerKind.MuShunJin, f.OwnerSeat);
        if (count == 0)
        {
            owner.Markers.Remove(PlayerMarkerKind.MuShunJin);
            owner.MarkerSourceCounts.Remove(key);
        }
        else
        {
            owner.Markers[PlayerMarkerKind.MuShunJin] = count;
            owner.MarkerSourceCounts[key] = count;
        }

        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new PlayerMarkerChangedEvent(f.Id, f.OwnerSeat, PlayerMarkerKind.MuShunJin, count - before, count, f.OwnerSeat, "skill-program." + f.SkillId + ".jin"));
    }

    internal SkillProgramStepOutcome BeginMuShunJinjian(ProgramSkillFrame supplied)
    {
        var f = _coreProgramLifecycleOperations.GetActiveProgramFrame(supplied.Id);
        if (f.MuShunJinjian is not null || f.InstructionIndex != 1 || f.SkillId != MuShunComposition.Jinjian || f.TriggerId != MuShunComposition.Damage || f.ActivationId != f.TriggerId || f.SelectedCardIds.Count != 0 || f.SelectedTargetSeats.Count != 0 || !_coreOrderedPrintedSkillLossObserversOperations.ExactOrderedPrintedSkillLossParent(f, out var window, out var damage, out var attack))
            throw new InvalidOperationException("Jinjian requires its exact original positive damage participant, candidate and binding.");
        var before = MuShunJin(f.OwnerSeat);
        var after = checked(before + 1);
        var r = new MuShunJinjianReceipt(MuShunSource(f), f.GameplayHash, window.Id, damage.Id, _coreActualTurnProgressTurnNumberTurnProgressionAccess._turnNumber, attack.IsSourceLess ? null : attack.SourceSeat, before, after);
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { MuShunJinjian = r });
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new MuShunJinjianIssuedEvent(f.Id, r.Source, r.Hash, r.WindowId, r.DamageFrameId, r.ActualTurn, r.DamageSourceSeat, before, after));
        SetMuShunJin(f, after);
        if (!CanMuShunContest(f))
        {
            CloseMuShunWithoutContest(f);
            return SkillProgramStepOutcome.AwaitChild;
        }

        PublishMuShun(f);
        return SkillProgramStepOutcome.AwaitChoice;
    }

    internal bool CanMuShunContest(ProgramSkillFrame f) => f.MuShunJinjian?.DamageSourceSeat is { } source && _coreTableSeatQueriesOperations.IsValidPlayerSeat(source) && source != f.OwnerSeat && _coreMatchOutcomeWinnerAccess._winner == Winner.None && _coreMatchParticipantsPlayersAccess._players[f.OwnerSeat].IsAlive && _coreMatchParticipantsPlayersAccess._players[source].IsAlive && _characterLuKangWumiaoOperations.HasWuLuKangPindianInput(_coreMatchParticipantsPlayersAccess._players[f.OwnerSeat]) && _characterLuKangWumiaoOperations.HasWuLuKangPindianInput(_coreMatchParticipantsPlayersAccess._players[source]) && _coreEquipmentSlotGroupProgramsOperations.CanBePindianTarget(f.OwnerSeat, source);
    internal IReadOnlyList<PromptChoice> MuShunChoices(ProgramSkillFrame f)
    {
        var list = new List<PromptChoice>();
        if (CanMuShunContest(f))
            list.Add(new(new($"mu-shun.{(f.Id)}.contest"), "与本次伤害来源拼点", [], [], new Dictionary<string, string> { ["program-action"] = MuShunAction, ["mode"] = "contest" }));
        list.Add(new(new($"mu-shun.{(f.Id)}.decline"), "保留已获得的劲，不拼点", [], [], new Dictionary<string, string> { ["program-action"] = MuShunAction, ["mode"] = "decline" }));
        return Array.AsReadOnly(list.ToArray());
    }

    internal void PublishMuShun(ProgramSkillFrame f)
    {
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { Step = ResolutionFrameStep.AwaitingResponse });
        _coreParticipantHandProgramsOperations.PublishParticipantHandChoice(f, f.OwnerSeat, MuShunChoices(f), "劲坚：已获得1个劲，你可以与原伤害来源拼点。", f.MuShunJinjian!.DamageSourceSeat!.Value);
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceRulesAndPublishState();
    }

    internal void CloseMuShunWithoutContest(ProgramSkillFrame f)
    {
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { MuShunJinjian = f.MuShunJinjian!with { Stage = MuShunContestStage.Declined } });
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new MuShunContestClosedEvent(f.Id, 0, false, false));
        _coreSkillProgramsOperations.FinishProgramSkill(f, true);
    }

    [ProgramAnswer(MuShunAction)]
    internal void AnswerMuShun(PromptChoice choice)
    {
        if (_resolutionStackOperations._resolutionStack.LastOrDefault()is not ProgramSkillFrame { MuShunJinjian.Stage: MuShunContestStage.Choice } f || !ValidMuShunJinjian(f) || _corePendingDecisionPolicyPipelineOperations._pendingDecision?.PlayerSeat != f.OwnerSeat || !CoreGameEngineAssistedPhysicalCardsRuntime.AssistedChoicesEqual([choice], MuShunChoices(f).Where(c => c.Id == choice.Id).ToArray()))
            throw new InvalidOperationException("Jinjian lost its exact original damage-source choice.");
        _coreNativePromptBoundaryOperations.ClearPendingDecision();
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { Step = ResolutionFrameStep.ResolvingEffect });
        if (choice.Parameters["mode"] == "decline" || !CanMuShunContest(f))
        {
            CloseMuShunWithoutContest(f);
            return;
        }

        var opponent = f.MuShunJinjian!.DamageSourceSeat!.Value;
        var contestId = _coreResolutionIdentityBoundaryOperations._resolutionSequence + 1;
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { MuShunJinjian = f.MuShunJinjian with { Stage = MuShunContestStage.Contest, ContestId = contestId, MovementBefore = _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence } });
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new MuShunContestIssuedEvent(f.Id, contestId, f.OwnerSeat, opponent, _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence));
        var skill = _contentRegistryOperations._contentRegistry.GetSkill(f.SkillId);
        _corePindianOperations.BeginSharedPindian(f.Id, new(f.SkillId, skill.Name, "劲坚 · 拼点", skill.Description), f.OwnerSeat, opponent, programResultBind: MuShunComposition.Result, programResultVisibility: SkillProgramCardSetVisibility.Public);
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceRulesAndPublishState();
    }

    [ProgramContinuation(ProgramContinuationKind.Resume, 45300)]
    internal bool ResumeMuShunContest(long id)
    {
        if (_resolutionStackOperations._resolutionStack.LastOrDefault()is not ProgramSkillFrame { MuShunJinjian.Stage: MuShunContestStage.Contest or MuShunContestStage.ContestChildren } f || f.Id != id)
            return false;
        if (!ValidMuShunJinjian(f) || f.PindianResultBindings.Where(b => b.Name == MuShunComposition.Result).ToArray()is not [var result])
            throw new InvalidOperationException("Jinjian lost the original native pair Pindian return.");
        if (f.MuShunJinjian!.Stage == MuShunContestStage.Contest)
        {
            _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { MuShunJinjian = f.MuShunJinjian with { Stage = MuShunContestStage.ContestChildren, MovementAfter = _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence } });
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new MuShunContestReturnedEvent(f.Id, f.MuShunJinjian.ContestId, f.MuShunJinjian.MovementBefore, f.MuShunJinjian.MovementAfter));
        }

        if (_coreCardMovementProgramsOperations.TryBeginCardsMovedProgramWindow(f.Id))
            return true;
        f = _coreProgramLifecycleOperations.GetActiveProgramFrame(id);
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { MuShunJinjian = f.MuShunJinjian!with { Stage = MuShunContestStage.Returned } });
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new MuShunContestClosedEvent(f.Id, f.MuShunJinjian!.ContestId, true, result.SourceWon));
        _coreRuntimeOperations.AdvanceRuntimeProgram(f.Id);
        return true;
    }

    [ProgramAiChoice(SkillProgramEffectOp.ResolveAnniversaryMuShunJinjian)]
    internal PromptChoice MuShunAi(PendingDecision p, ProgramSkillFrame f) => p.Choices.FirstOrDefault(c => c.Parameters.GetValueOrDefault("mode") == "contest") ?? p.Choices[0];
    [ProgramDistanceRule(ProgramDistanceStage.AttackRangeContributions, 45300)]
    internal void MuShunRange(CharacterState actor, List<RuleQueryContribution> contributions)
    {
        if (!HasMuShunCapability || !_coreNationalSkillsOperations.HasRuntimeSkill(actor, MuShunComposition.Jinjian))
            return;
        var count = MuShunJin(actor.Seat);
        if (count > 0)
            contributions.Add(new FiniteRuleQueryContribution("anniversary:jinjian:" + actor.Seat, SkillRuleOperation.Add, count));
    }

    internal SkillProgramStepOutcome ResolveMuShunShizhao(ProgramSkillFrame supplied)
    {
        var f = _coreProgramLifecycleOperations.GetActiveProgramFrame(supplied.Id);
        if (f.InstructionIndex != 1 || f.SkillId != MuShunComposition.Shizhao || f.TriggerId != MuShunComposition.Loss || f.ActivationId != f.TriggerId || f.SelectedCardIds.Count != 0 || f.SelectedTargetSeats.Count != 0 || f.WindowContext is not { } c || !MuShunLossCandidate(MuShunCandidate(f), c, false))
            throw new InvalidOperationException("Shizhao requires its first original outside-turn last-Hand movement.");
        var before = MuShunJin(f.OwnerSeat);
        var after = before > 0 ? before - 1 : 0;
        var batch = c.MovementBatch!;
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new MuShunShizhaoIssuedEvent(f.Id, MuShunSource(f), f.GameplayHash, c.ParentFrameId, batch.Id, batch.TurnNumber, batch.MovementTiming!.ActualTurnOwnerSeat, before, after, before > 0));
        if (before > 0)
        {
            SetMuShunJin(f, after);
            return SkillProgramStepOutcome.Continue;
        }

        _coreSkillProgramsOperations.FinishProgramSkill(f, true);
        return SkillProgramStepOutcome.AwaitChild;
    }
}
