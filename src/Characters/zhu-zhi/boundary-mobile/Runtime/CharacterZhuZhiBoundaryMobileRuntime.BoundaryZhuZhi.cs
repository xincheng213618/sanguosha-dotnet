#nullable enable

using System.Globalization;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterZhuZhiBoundaryMobileRuntime
{
    internal const string BoundaryAnguoAction = "boundary-zhu-zhi-anguo";
    internal static CardConversionSource BoundaryAnguoSource(ProgramSkillFrame frame) => new(frame.SkillId, CoreGameEngineProgramLifecycleRuntime.GetProgramBindingId(frame), frame.OwnerSeat, frame.SkillInstanceId);
    internal bool HasBoundaryAnguoMark(int seat) => _coreTableSeatQueriesOperations.IsValidPlayerSeat(seat) && _coreMatchParticipantsPlayersAccess._players[seat].IsAlive && _coreMatchParticipantsPlayersAccess._players[seat].Markers.GetValueOrDefault(PlayerMarkerKind.MobileBoundaryAnguo) > 0;
    internal bool HadBoundaryAnguoMark(int seat) => _coreProgramEventHistoryOperations.Value.ProgramEventHistory<PlayerMarkerChangedEvent>().Any(e => e.Marker == PlayerMarkerKind.MobileBoundaryAnguo && e.PlayerSeat == seat && e.Delta > 0);
    internal int[] BoundaryAnguoNewRecipients(int owner) => _coreMatchParticipantsPlayersAccess._players.Where(p => p.IsAlive && p.Seat != owner && !HadBoundaryAnguoMark(p.Seat)).Select(p => p.Seat).ToArray();
    [ProgramTriggerLegality(ProgramTriggerLegalityPhase.BeforeGainQualification, 53200)]
    internal bool CanOfferBoundaryAnguo(ProgramTriggerLegalityContext gate)
    {
        if (gate.SkillId != BoundaryZhuZhiComposition.Skill)
            return true;
        var context = gate.Context;
        return gate.Trigger.Id switch
        {
            "initial-mark" => _coreMatchParticipantsPlayersAccess._players.Any(p => p.IsAlive && p.Seat != gate.OwnerSeat),
            "move-mark" => _coreMatchParticipantsPlayersAccess._players.Any(p => HasBoundaryAnguoMark(p.Seat)) && BoundaryAnguoNewRecipients(gate.OwnerSeat).Length > 0,
            "prevent-lethal" => context.TargetSeat == gate.OwnerSeat && gate.Owner.HasHpValue && context.Amount >= gate.Owner.Hp && _coreMatchParticipantsPlayersAccess._players.Any(p => HasBoundaryAnguoMark(p.Seat)) && (context.SourceSeat is not { } source || !HasBoundaryAnguoMark(source)),
            "rescue-marked" => context.TargetSeat is { } victim && HasBoundaryAnguoMark(victim) && _coreMatchParticipantsPlayersAccess._players[victim].Hp <= 0 || _resolutionStackOperations._resolutionStack.OfType<ProgramSkillFrame>().Any(f => f.OwnerSeat == gate.OwnerSeat && f.SkillId == gate.SkillId && f.SkillInstanceId == gate.Candidate.SkillInstanceId && f.WindowContext == context && HasExactBoundaryAnguoRescueTarget(f)),
            _ => false
        };
    }

    SkillProgramStepOutcome IBoundaryZhuZhiHost.Resolve(ProgramSkillFrame frame, SkillProgramEffectOp operation) => operation == SkillProgramEffectOp.PayBoundaryZhuZhiAnguo ? BeginBoundaryAnguoPayment(frame) : BeginBoundaryAnguo(frame, operation == SkillProgramEffectOp.BeginBoundaryZhuZhiRescue);
    internal SkillProgramStepOutcome BeginBoundaryAnguo(ProgramSkillFrame supplied, bool rescue)
    {
        var frame = _coreProgramLifecycleOperations.GetActiveProgramFrame(supplied.Id);
        if (frame.SkillId != BoundaryZhuZhiComposition.Skill || frame.InstructionIndex != 1 || frame.BoundaryZhuZhi is not null || frame.WindowContext is not { } context)
            throw new InvalidOperationException("Anguo needs its original mark or dying-entry binding.");
        var victim = -1;
        long dyingId = 0;
        if (rescue)
        {
            if (frame.TriggerId != "rescue-marked" || !_coreDyingOwnedCardSelectionsOperations.ExactDyingOwnedCardEntry(frame, out var dying, out _) || !HasBoundaryAnguoMark(dying.VictimSeat))
                throw new InvalidOperationException("Anguo rescue requires its exact still-marked original dying participant.");
            victim = dying.VictimSeat;
            dyingId = dying.Id;
        }
        else if (frame.TriggerId is not ("initial-mark" or "move-mark"))
            throw new InvalidOperationException("Anguo marks require the game-start or owner's Play-start binding.");
        var receipt = new BoundaryZhuZhiReceipt(BoundaryAnguoSource(frame), frame.GameplayHash, context.ParentFrameId, frame.TriggerId!, rescue ? BoundaryZhuZhiStage.Recovery : BoundaryZhuZhiStage.MarkTarget, victim, dyingId);
        _coreRuntimeOperations.ReplaceRuntimeTop(frame = frame with { BoundaryZhuZhi = receipt, SelectedTargetSeats = rescue ? Array.AsReadOnly(new[] { victim }) : frame.SelectedTargetSeats });
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new BoundaryZhuZhiStartedEvent(frame.Id, receipt.Source, receipt.GameplayHash, receipt.WindowId, receipt.Binding, victim, dyingId));
        if (!rescue)
        {
            PublishBoundaryAnguo(frame);
            return SkillProgramStepOutcome.AwaitChoice;
        }

        var count = _coreMatchParticipantsPlayersAccess._players[victim].Markers.GetValueOrDefault(PlayerMarkerKind.MobileBoundaryAnguo);
        _coreAttributedMarkerEventsOperations.MutateParticipantMarker(frame, victim, PlayerMarkerKind.MobileBoundaryAnguo, -count);
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new BoundaryZhuZhiRescueIssuedEvent(frame.Id, victim, dyingId, count));
        return SkillProgramStepOutcome.Continue;
    }

    internal PromptChoice BoundaryAnguoChoice(ProgramSkillFrame frame, string branch, int previous, int target, string label) => new(new($"anguo.{(frame.Id)}.{(branch)}.{(previous)}.{(target)}"), label, [], target < 0 ? [] : [target], new Dictionary<string, string> { ["program-action"] = BoundaryAnguoAction, ["branch"] = branch, ["previous"] = previous.ToString(CultureInfo.InvariantCulture), ["frame-id"] = frame.Id.ToString(CultureInfo.InvariantCulture) });
    internal IReadOnlyList<PromptChoice> BoundaryAnguoChoices(ProgramSkillFrame frame)
    {
        var receipt = frame.BoundaryZhuZhi!;
        var choices = new List<PromptChoice>();
        if (receipt.Stage == BoundaryZhuZhiStage.MarkTarget)
        {
            var sources = receipt.Binding == "initial-mark" ? new[]
            {
                -1
            }

            : _coreMatchParticipantsPlayersAccess._players.Where(p => HasBoundaryAnguoMark(p.Seat)).Select(p => p.Seat).ToArray();
            var targets = receipt.Binding == "initial-mark" ? _coreMatchParticipantsPlayersAccess._players.Where(p => p.IsAlive && p.Seat != frame.OwnerSeat).Select(p => p.Seat).ToArray() : BoundaryAnguoNewRecipients(frame.OwnerSeat);
            foreach (var source in sources)
                foreach (var target in targets)
                    choices.Add(BoundaryAnguoChoice(frame, "mark", source, target, source < 0 ? $"令 {(_coreMatchParticipantsPlayersAccess._players[target].Name)} 获得“安国”" : $"将 {(_coreMatchParticipantsPlayersAccess._players[source].Name)} 的“安国”移至 {(_coreMatchParticipantsPlayersAccess._players[target].Name)}"));
        }
        else if (receipt.Stage == BoundaryZhuZhiStage.PaymentChoice)
        {
            var owner = _coreMatchParticipantsPlayersAccess._players[frame.OwnerSeat];
            if (owner.IsAlive && owner.HasHpValue && owner.Hp > 1)
                choices.Add(BoundaryAnguoChoice(frame, "hp", -1, -1, $"失去 {(owner.Hp - 1)} 点体力，体力降至1"));
            if (owner.IsAlive && owner.MaxHp > 1)
                choices.Add(BoundaryAnguoChoice(frame, "maximum", -1, -1, $"减少 {(owner.MaxHp - 1)} 点体力上限，上限降至1"));
        }

        return Array.AsReadOnly(choices.ToArray());
    }

    internal void PublishBoundaryAnguo(ProgramSkillFrame frame)
    {
        var choices = BoundaryAnguoChoices(frame);
        if (choices.Count == 0)
        {
            _coreRuntimeOperations.ReplaceRuntimeTop(frame = frame with { BoundaryZhuZhi = frame.BoundaryZhuZhi!with { Stage = BoundaryZhuZhiStage.Paid } });
            _coreRuntimeOperations.AdvanceRuntimeProgram(frame.Id);
            return;
        }

        _coreRuntimeOperations.ReplaceRuntimeTop(frame = frame with { Step = ResolutionFrameStep.AwaitingResponse });
        var skill = _contentRegistryOperations._contentRegistry.GetSkill(frame.SkillId);
        _corePendingDecisionPolicyPipelineOperations._pendingDecision = new(DecisionKind.ProgramTrigger, frame.OwnerSeat, frame.BoundaryZhuZhi!.Stage == BoundaryZhuZhiStage.MarkTarget ? "安国：选择本局从未获得过标记的其他角色。" : "安国：受救角色已回复，选择自身代价。", [], Array.AsReadOnly(choices.SelectMany(c => c.Targets).Distinct().ToArray()), frame.OwnerSeat)
        {
            PromptId = _coreNativePromptBoundaryOperations.CreatePromptId(),
            IsPrivate = true,
            Choices = choices,
            SkillPrompt = new(frame.SkillId, skill.Name, skill.Name, skill.Description)
        };
        _coreMatchOutcomeStatusAccess._status = _coreMatchParticipantsPlayersAccess._players[frame.OwnerSeat].IsHuman ? EngineStatus.AwaitingHumanResponse : EngineStatus.Running;
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceRulesAndPublishState();
    }

    [ProgramAnswer(BoundaryAnguoAction)]
    internal void AnswerBoundaryAnguo(PromptChoice choice)
    {
        if (_resolutionStackOperations._resolutionStack.LastOrDefault()is not ProgramSkillFrame { BoundaryZhuZhi: { } receipt } frame || !ValidBoundaryAnguo(frame) || _corePendingDecisionPolicyPipelineOperations._pendingDecision?.PlayerSeat != frame.OwnerSeat || !CoreGameEngineAssistedPhysicalCardsRuntime.AssistedChoicesEqual([choice], BoundaryAnguoChoices(frame).Where(c => c.Id == choice.Id).ToArray()))
            throw new InvalidOperationException("Anguo requires its actual fresh recipient or currently payable original rescue choice.");
        _coreNativePromptBoundaryOperations.ClearPendingDecision();
        _coreRuntimeOperations.ReplaceRuntimeTop(frame = frame with { Step = ResolutionFrameStep.ResolvingEffect });
        if (receipt.Stage == BoundaryZhuZhiStage.MarkTarget)
        {
            var previous = int.Parse(choice.Parameters["previous"], CultureInfo.InvariantCulture);
            var target = choice.Targets.Single();
            if (previous >= 0)
                _coreAttributedMarkerEventsOperations.MutateParticipantMarker(frame, previous, PlayerMarkerKind.MobileBoundaryAnguo, -1);
            _coreAttributedMarkerEventsOperations.MutateParticipantMarker(frame, target, PlayerMarkerKind.MobileBoundaryAnguo, 1);
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new BoundaryZhuZhiMarkEvent(frame.Id, receipt.Source, previous, target));
            _coreRuntimeOperations.ReplaceRuntimeTop(frame with { BoundaryZhuZhi = receipt with { Stage = BoundaryZhuZhiStage.Paid } });
            _coreRuntimeOperations.AdvanceRuntimeProgram(frame.Id);
            return;
        }

        var owner = _coreMatchParticipantsPlayersAccess._players[frame.OwnerSeat];
        var branch = choice.Parameters["branch"];
        _coreRuntimeOperations.ReplaceRuntimeTop(frame = frame with { BoundaryZhuZhi = receipt with { Payment = branch, Stage = BoundaryZhuZhiStage.Paying, HpBefore = owner.Hp, MaximumBefore = owner.MaxHp, Requested = branch == "hp" ? owner.Hp - 1 : owner.MaxHp - 1 } });
        ApplyBoundaryAnguoPayment(frame);
    }

    internal SkillProgramStepOutcome BeginBoundaryAnguoPayment(ProgramSkillFrame supplied)
    {
        var frame = _coreProgramLifecycleOperations.GetActiveProgramFrame(supplied.Id);
        if (!ValidBoundaryAnguo(frame) || frame.InstructionIndex != 3 || frame.BoundaryZhuZhi is not { Stage: BoundaryZhuZhiStage.Recovery } receipt)
            throw new InvalidOperationException("Anguo payment must follow its original native recovery exactly once.");
        _coreRuntimeOperations.ReplaceRuntimeTop(frame = frame with { BoundaryZhuZhi = receipt with { Stage = BoundaryZhuZhiStage.PaymentChoice } });
        PublishBoundaryAnguo(frame);
        return SkillProgramStepOutcome.AwaitChoice;
    }

    internal void ApplyBoundaryAnguoPayment(ProgramSkillFrame frame)
    {
        var receipt = frame.BoundaryZhuZhi!;
        if (receipt.Payment == "hp")
        {
            var result = _corePrepPoisonOutcomesOperations.ApplyProgramHpLoss(frame.Id, frame.SkillId, frame.OwnerSeat, receipt.Requested, (_, _, _) => RecordBoundaryAnguoPayment(frame.Id));
            if (result == SkillProgramStepOutcome.AwaitChild)
                return;
        }
        else
        {
            _coreProgramLifecycleOperations.ChangeProgramMaximumHp(frame, -receipt.Requested);
            if (_resolutionStackOperations._resolutionStack.LastOrDefault()?.Id != frame.Id || _corePendingDecisionPolicyPipelineOperations._pendingDecision is not null)
                return;
            RecordBoundaryAnguoPayment(frame.Id);
        }

        ContinueBoundaryAnguoPayment(frame.Id);
    }

    internal void RecordBoundaryAnguoPayment(long id)
    {
        var frame = _coreProgramLifecycleOperations.GetActiveProgramFrame(id);
        var receipt = frame.BoundaryZhuZhi!;
        var owner = _coreMatchParticipantsPlayersAccess._players[frame.OwnerSeat];
        if (receipt.Stage != BoundaryZhuZhiStage.Paying)
            throw new InvalidOperationException("Anguo cannot repeat a completed payment.");
        _coreRuntimeOperations.ReplaceRuntimeTop(frame with { BoundaryZhuZhi = receipt with { Stage = BoundaryZhuZhiStage.PaymentChildren }, PendingMovementContinuation = new(frame.OwnerSeat, 0, null) });
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new BoundaryZhuZhiPaymentEvent(id, frame.OwnerSeat, receipt.Payment!, receipt.HpBefore, owner.Hp, receipt.MaximumBefore, owner.MaxHp));
    }

    [ProgramContinuation(ProgramContinuationKind.Resume, 53200)]
    internal bool ContinueBoundaryAnguoPayment(long id)
    {
        if (_resolutionStackOperations._resolutionStack.LastOrDefault()is not ProgramSkillFrame { BoundaryZhuZhi: { } receipt } frame || frame.Id != id || receipt.Stage is not (BoundaryZhuZhiStage.Paying or BoundaryZhuZhiStage.PaymentChildren))
            return false;
        if (!ValidBoundaryAnguo(frame))
            throw new InvalidOperationException("Anguo lost its original issued rescue payment.");
        if (receipt.Stage == BoundaryZhuZhiStage.Paying)
        {
            ApplyBoundaryAnguoPayment(frame);
            return true;
        }

        if (_coreRecoveryReplacementOperations.TryBeginQueuedRecoveryReplacement(id, PostEventContinuation.AwaitedProgramMovement) || _coreCharacterStateProgramsOperations.TryBeginCharacterStateProgramWindow(id, CharacterStateContinuation.Program) || _coreHpChangeProgramsOperations.TryBeginHpChangedProgramWindow(id, PostEventContinuation.AwaitedProgramMovement) || _coreSkillOwnershipWindowsOperations.TryBeginAdvancedSkillsChanged(id) || _coreCardMovementProgramsOperations.TryBeginCardsMovedProgramWindow(id))
            return true;
        _coreRuntimeOperations.ReplaceRuntimeTop(frame = _coreProgramLifecycleOperations.GetActiveProgramFrame(id)with { BoundaryZhuZhi = receipt with { Stage = BoundaryZhuZhiStage.Paid }, PendingMovementContinuation = null });
        _coreRuntimeOperations.AdvanceRuntimeProgram(id);
        return true;
    }

    [ProgramContinuation(ProgramContinuationKind.Movement, 53200)]
    internal bool ReturnBoundaryAnguoPayment(ProgramSkillFrame frame) => frame.BoundaryZhuZhi is not null && ContinueBoundaryAnguoPayment(frame.Id);

    internal bool ValidBoundaryAnguo(ProgramSkillFrame frame) => frame.BoundaryZhuZhi is { } receipt && frame.SkillId == BoundaryZhuZhiComposition.Skill && receipt.Source == BoundaryAnguoSource(frame) && receipt.GameplayHash == frame.GameplayHash && receipt.WindowId == frame.WindowContext?.ParentFrameId && receipt.Binding == frame.TriggerId && (receipt.Binding != "rescue-marked" || frame.SelectedTargetSeats.SequenceEqual([receipt.Victim]) && _coreDyingOwnedCardSelectionsOperations.ExactDyingOwnedCardEntry(frame, out var dying, out _) && dying.Id == receipt.DyingId && dying.VictimSeat == receipt.Victim) && _coreProgramEventHistoryOperations.Value.ProgramEventHistory<BoundaryZhuZhiStartedEvent>().Count(e => e.FrameId == frame.Id && e.Source == receipt.Source && e.GameplayHash == receipt.GameplayHash && e.WindowId == receipt.WindowId && e.Binding == receipt.Binding && e.Victim == receipt.Victim && e.DyingId == receipt.DyingId) == 1;
    internal bool HasExactBoundaryAnguoRescueTarget(ProgramSkillFrame frame)
    {
        if (frame.BoundaryZhuZhi is not { Binding: "rescue-marked" } receipt || frame.TriggerId != "rescue-marked" || frame.InstructionIndex is < 1 or > 4 || frame.SelectedCardIds.Count != 0 || !ValidBoundaryAnguo(frame))
            return false;
        var issues = _coreProgramEventHistoryOperations.Value.ProgramEventHistory<BoundaryZhuZhiRescueIssuedEvent>().Where(e => e.FrameId == frame.Id && e.Victim == receipt.Victim && e.DyingId == receipt.DyingId && e.RemovedMarks > 0).ToArray();
        if (issues is not [var issue])
            return false;
        var removals = _coreProgramEventHistoryOperations.Value.ProgramEventHistory<PlayerMarkerChangedEvent>().Where(e => e.ResolutionId == frame.Id && e.PlayerSeat == receipt.Victim && e.Marker == PlayerMarkerKind.MobileBoundaryAnguo && e.Delta < 0).ToArray();
        return removals.Length > 0 && removals.Sum(e => -e.Delta) == issue.RemovedMarks && removals[^1].Count == 0;
    }

    [ProgramRuntimeHook(ProgramRuntimePhase.AfterSkillLoss, 53200)]
    [ProgramRuntimeHook(ProgramRuntimePhase.AfterOwnerDeath, 53200)]
    internal bool KeepIssuedBoundaryAnguo(ProgramSkillFrame frame) => frame.BoundaryZhuZhi is { Binding: "rescue-marked" } && ValidBoundaryAnguo(frame);
    [ProgramAiChoice(SkillProgramEffectOp.ResolveBoundaryZhuZhiAnguo, SkillProgramEffectOp.PayBoundaryZhuZhiAnguo)]
    internal PromptChoice BoundaryAnguoAi(PendingDecision decision, ProgramSkillFrame frame) => decision.Choices.OrderByDescending(c => c.Targets.Count == 1 ? _coreAiBrainsAiBrainsAccess._aiBrains[frame.OwnerSeat].ScoreProgramTarget(_corePlayerViewProjectionOperations.CreateSnapshot(frame.OwnerSeat), c.Targets[0], new(0, 0, 0, 1, 0, 0, false, false)) : c.Parameters.GetValueOrDefault("branch") == "hp" ? 1 : 0).First();
    internal RuleQueryEvaluation ApplyBoundaryZhuZhiHandLimit(CharacterState player, RuleQueryEvaluation ordinary)
    {
        if (!HasBoundaryAnguoMark(player.Seat))
            return ordinary;
        var set = new FiniteRuleQueryContribution($"marker:{(player.Seat)}:boundary:anguo-mobile:maximum-hp", SkillRuleOperation.Set, player.MaxHp, 53200);
        var terms = Array.AsReadOnly(ordinary.Value.Contributions.Append(set).OrderBy(c => c.SourceId, StringComparer.Ordinal).ToArray());
        var stage = new RuleQueryStageSnapshot("anguo-maximum-hp", ordinary.Value is FiniteRuleQueryValue finite ? finite.Value : 0, false, player.MaxHp, Array.AsReadOnly<RuleQueryContribution>([set]));
        return new(new FiniteRuleQueryValue(player.MaxHp, terms), ordinary.BaseTerms, Array.AsReadOnly(ordinary.Stages.Append(stage).ToArray()));
    }

    internal IReadOnlyList<ProgramPublicRuleStateSnapshot> BoundaryZhuZhiPublicStates(CharacterState owner, string skillId)
    {
        if (skillId != BoundaryZhuZhiComposition.Skill)
            return[];
        var marked = _coreMatchParticipantsPlayersAccess._players.Count(p => HasBoundaryAnguoMark(p.Seat));
        var unused = BoundaryAnguoNewRecipients(owner.Seat).Length;
        return _coreNationalSkillsOperations.GetSkillBindingShard(owner).ProgramInstances.Where(i => i.SkillId == skillId).SelectMany(i => new[] { new ProgramPublicRuleStateSnapshot(i.SkillInstanceId, "anguo-marked", ProgramPublicRuleStateKind.GameActivationProgress, marked), new ProgramPublicRuleStateSnapshot(i.SkillInstanceId, "anguo-unused", ProgramPublicRuleStateKind.GameActivationProgress, unused) }).ToArray();
    }
}
