#nullable enable

using System.Globalization;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CoreGameEnginePrintedLordBenefitsRuntime
{
    internal bool PrintedLordUnissuedSource(ProgramSkillFrame f) => _coreMatchParticipantsPlayersAccess._players[f.OwnerSeat].IsAlive && _coreProgramLifecycleOperations.CanInvokeProgramSkill(f) && _coreMatchOutcomeStatusWinnerAccess._winner == Winner.None;
    internal bool ExactPrintedLordStart(ProgramSkillFrame f)
    {
        if (f.WindowContext is not { Window: SkillProgramTriggerWindow.TurnStartBeforeNormalFlow } context || f.OwnerSeat != _coreActualTurnProgressCurrentSeatTurnNumberAccess._currentSeat || context.OwnerSeat != f.OwnerSeat || context.SourceSeat != f.OwnerSeat || _resolutionStackOperations._resolutionStack.OfType<ProgramLifecycleTriggerWindowFrame>().SingleOrDefault(w => w.Id == context.ParentFrameId)is not { Window: SkillProgramTriggerWindow.TurnStartBeforeNormalFlow, Continuation: ProgramLifecycleContinuation.NormalTurnStart } parent || parent.OwnerSeat != f.OwnerSeat || parent.CandidateIndex < 0 || parent.CandidateIndex >= parent.Candidates.Count || !CoreGameEngineDamageTargetMountObserverRuntime.MountObserverCandidateMatches(f, parent.Candidates[parent.CandidateIndex]))
            return false;
        var index = _resolutionStackOperations._resolutionStack.FindIndex(frame => frame.Id == f.Id);
        if (index <= 0 || _resolutionStackOperations._resolutionStack[index - 1].Id != parent.Id)
            return false;
        return _coreProgramLifecycleOperations.GetProgramTrigger(f).Effects is [{ Op: SkillProgramEffectOp.RaiseMaximumRecoverAndQualifyPrintedLord }];
    }

    internal SkillProgramStepOutcome BeginPrintedLordBenefit(ProgramSkillFrame input)
    {
        var f = _coreProgramLifecycleOperations.GetActiveProgramFrame(input.Id);
        if (f.PrintedLordBenefit is not null || f.InstructionIndex != 1 || !ExactPrintedLordStart(f))
            throw new InvalidOperationException("Printed lord qualification requires the exact own actual-start candidate.");
        foreach (var player in _coreMatchParticipantsPlayersAccess._players)
            _observedSkillGrantRevisionsOperations._observedSkillGrantRevisions.TryAdd(player.Seat, player.SkillGrants.GrantRevision);
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { PrintedLordBenefit = new(1, CoreGameEngineBlackGiftContestsRuntime.RecipientConsequencesSource(f), f.GameplayHash, _coreActualTurnProgressCurrentSeatTurnNumberAccess._turnNumber, f.WindowContext!.ParentFrameId, PrintedLordBenefitStage.ChoosingBeneficiary) });
        PublishPrintedLordChoice(f);
        return SkillProgramStepOutcome.AwaitChoice;
    }

    internal int[] PrintedLordBeneficiaries(ProgramSkillFrame f) => _coreMatchParticipantsPlayersAccess._players.Where(p => p.IsAlive && p.Seat != f.OwnerSeat && p.Gender == GeneralGender.Male).Select(p => p.Seat).ToArray();
    internal IReadOnlyList<PromptChoice> PrintedLordChoices(ProgramSkillFrame f) => Array.AsReadOnly(PrintedLordBeneficiaries(f).Select(s => new PromptChoice(new($"printed-lord.{(f.Id)}.beneficiary-{(s)}"), $"令 {(_coreMatchParticipantsPlayersAccess._players[s].Name)} 增加体力上限、回复体力并启用其主公技。", [], [s], new Dictionary<string, string> { ["program-action"] = "printed-lord-benefit", ["frame-id"] = f.Id.ToString(CultureInfo.InvariantCulture) })).ToArray());
    internal void PublishPrintedLordChoice(ProgramSkillFrame f)
    {
        if (_coreRecipientConsequencesReceiptsOperations.RecipientConsequencesGameEnded())
            return;
        var choices = PrintedLordChoices(f);
        if (!PrintedLordUnissuedSource(f) || choices.Count == 0)
        {
            _coreProgramLifecycleOperations.CancelProgramBindingAndCleanup(f, "没有仍然合法的拥嫡受益者。");
            return;
        }

        var skill = _contentRegistryOperations._contentRegistry.GetSkill(f.SkillId);
        _corePendingDecisionPolicyPipelineOperations._pendingDecision = new(DecisionKind.ProgramTrigger, f.OwnerSeat, "选择一名其他男性角色。", [], choices.SelectMany(c => c.Targets).ToArray(), f.OwnerSeat)
        {
            PromptId = _coreNativePromptBoundaryOperations.CreatePromptId(),
            IsPrivate = true,
            TargetSeat = f.OwnerSeat,
            Choices = choices,
            SkillPrompt = new(f.SkillId, skill.Name, skill.Name, skill.Description)
        };
        _coreMatchOutcomeStatusWinnerAccess._status = _coreMatchParticipantsPlayersAccess._players[f.OwnerSeat].IsHuman ? EngineStatus.AwaitingHumanResponse : EngineStatus.Running;
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceRulesAndPublishState();
    }

    [ProgramAnswer("printed-lord-benefit")]
    internal void ResolvePrintedLordChoice(PromptChoice choice)
    {
        var f = (ProgramSkillFrame)_resolutionStackOperations._resolutionStack.Last();
        var r = f.PrintedLordBenefit!;
        if (r.Stage != PrintedLordBenefitStage.ChoosingBeneficiary || _corePendingDecisionPolicyPipelineOperations._pendingDecision?.PlayerSeat != f.OwnerSeat || !CoreGameEngineAssistedPhysicalCardsRuntime.AssistedChoicesEqual([choice], [PrintedLordChoices(f).Single(c => c.Id == choice.Id)]))
            throw new InvalidOperationException("Printed lord benefit lost its exact published beneficiary.");
        _coreNativePromptBoundaryOperations.ClearPendingDecision();
        if (!PrintedLordUnissuedSource(f))
        {
            _coreProgramLifecycleOperations.CancelProgramBindingAndCleanup(f, "拥嫡支付前来源已失效。");
            return;
        }

        var seat = choice.Targets.Single();
        var target = _coreMatchParticipantsPlayersAccess._players[seat];
        var before = target.MaxHp;
        var after = checked(before + 1);
        _coreRuntimeOperations.ReplaceRuntimeTop(f with { PrintedLordBenefit = r with { Stage = PrintedLordBenefitStage.MaximumPaid, BeneficiarySeat = seat, MaximumBefore = before, MaximumAfter = after } });
        target.MaxHp = after;
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new MaximumHpChangedEvent(seat, 1, after, f.SkillId));
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new PrintedLordMaximumPaidEvent(f.Id, seat, before, after));
        _coreRuntimeOperations.AdvanceRuntimeProgram(f.Id);
    }

    [ProgramContinuation(ProgramContinuationKind.Resume, 400)]
    internal bool ResumePrintedLordBenefit(long id)
    {
        if (_resolutionStackOperations._resolutionStack.LastOrDefault()is not ProgramSkillFrame f || f.Id != id || f.PrintedLordBenefit is not { } r)
            return false;
        if (_coreRecipientConsequencesReceiptsOperations.RecipientConsequencesGameEnded())
            return true;
        if (r.Stage == PrintedLordBenefitStage.ChoosingBeneficiary)
        {
            if (_corePendingDecisionPolicyPipelineOperations._pendingDecision is null)
                PublishPrintedLordChoice(f);
            return true;
        }

        if (_coreBlackGiftContestsOperations.DrainRecipientConsequences(f, false))
            return true;
        f = _coreProgramLifecycleOperations.GetActiveProgramFrame(id);
        r = f.PrintedLordBenefit!;
        if (r.Stage == PrintedLordBenefitStage.Complete)
        {
            _coreSkillProgramsOperations.FinishProgramSkill(f, true);
            return true;
        }

        if (!PrintedLordUnissuedSource(f) || !_coreMatchParticipantsPlayersAccess._players[r.BeneficiarySeat].IsAlive)
        {
            _coreProgramLifecycleOperations.CancelProgramBindingAndCleanup(f, "拥嫡已支付的上限、回复和全部孩子保留，未发行的后继取消。");
            return true;
        }

        if (r.Stage == PrintedLordBenefitStage.MaximumPaid)
        {
            var beneficiary = _coreMatchParticipantsPlayersAccess._players[r.BeneficiarySeat];
            var requested = beneficiary.HasHpValue ? Math.Min(1, Math.Max(0, beneficiary.MaxHp - beneficiary.Hp)) : 0;
            _coreRuntimeOperations.ReplaceRuntimeTop(f with { PrintedLordBenefit = r with { Stage = PrintedLordBenefitStage.RecoveryPaid, RecoveryRequested = requested } });
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new PrintedLordRecoveryRequestedEvent(id, r.BeneficiarySeat, requested));
            _kernelCoreGameEngineAdvancedLifecycleHostOperations.CreateProgramSkillHost().Recover(id, f.OwnerSeat, r.BeneficiarySeat, requested, null, null);
            _coreRuntimeOperations.AdvanceRuntimeProgram(id);
            return true;
        }

        if (r.Stage == PrintedLordBenefitStage.RecoveryPaid)
        {
            // “然后” freezes the actual printed grants after recovery children,
            // including a genuine general replacement made by those children.
            r = r with
            {
                Stage = PrintedLordBenefitStage.Qualifying,
                PrintedQualifications = _corePrintedLordSkillQualificationsOperations.CapturePrintedLordQualifications(f, r.BeneficiarySeat)
            };
            _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { PrintedLordBenefit = r });
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new PrintedLordQualificationCapturedEvent(id, r.BeneficiarySeat));
            _corePrintedLordSkillQualificationsOperations.IssueCapturedPrintedLordQualifications(f, r);
            _coreRuntimeOperations.ReplaceRuntimeTop(f = _coreProgramLifecycleOperations.GetActiveProgramFrame(id)with { PrintedLordBenefit = r with { Stage = PrintedLordBenefitStage.Complete } });
            _coreRuntimeOperations.AdvanceRuntimeProgram(id);
            return true;
        }

        throw new InvalidOperationException("A printed qualification cannot resume a completed benefit twice.");
    }

    [ProgramAiChoice(SkillProgramEffectOp.GiveBlackHandAndResolveRecipientContest, SkillProgramEffectOp.RaiseMaximumRecoverAndQualifyPrintedLord)]
    internal PromptChoice SelectAiRecipientConsequences(PendingDecision p, ProgramSkillFrame f)
    {
        if (f.BlackGiftContest is { Stage: BlackGiftContestStage.ChoosingDiscard })
            return p.Choices.OrderBy(c => CoreNativeAiDecisionsRuntime.GetKeepValue(_coreAdvancedLifecycleOperations.GetAdvancedCard(c.Cards.Single()), _coreMatchParticipantsPlayersAccess._players[p.PlayerSeat])).ThenBy(c => c.Cards.Single()).First();
        var hint = f.PrintedLordBenefit is not null ? new SkillProgramAiHint(0, 0, 0, 0, 1, 0, false, false) : new SkillProgramAiHint(0, 0, 0, 0, 0, 1, false, false);
        // Shared public team/HP/hand-count target scoring; no foreign Hand ids.
        return p.Choices.OrderByDescending(c => _coreAiBrainsAiBrainsAccess._aiBrains[f.OwnerSeat].ScoreProgramTarget(_corePlayerViewProjectionOperations.CreateSnapshot(f.OwnerSeat), c.Targets.Single(), hint)).ThenBy(c => c.Targets.Single()).First();
    }
}
