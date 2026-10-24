namespace CardGame.Core;

public sealed partial class GameEngine
{
    [ProgramCapability] private sealed class BoundaryLingTongHost(GameEngine engine) : IBoundaryLingTongHost
    { public SkillProgramStepOutcome SelectEquipmentPair(ProgramSkillFrame frame) => engine.SelectBoundaryLingTongEquipmentPair(frame); }
    [ProgramCapability] private sealed class BoundaryLingTongQueries(GameEngine engine) : IBoundaryLingTongQueries
    { public IReadOnlyList<int> MovableSources(int ownerSeat) => Array.AsReadOnly(engine.BoundaryLingTongEquipmentPairs(ownerSeat).Select(p => p.Source).Distinct().ToArray()); }

    private IEnumerable<(int Source, int Destination)> BoundaryLingTongEquipmentPairs(int ownerSeat) =>
        _players.Where(p => p.IsAlive && p.Seat != ownerSeat).SelectMany(source =>
            _players.Where(p => p.IsAlive && p.Seat != ownerSeat && p.Seat != source.Seat &&
                GetEquipment(source).Any(c => !c.IsGeneralWeapon && CanEnterEquipmentSlot(p.Seat, c)))
            .Select(destination => (source.Seat, destination.Seat)));

    [ProgramTriggerLegality(ProgramTriggerLegalityPhase.BeforeGainQualification, 56800, SkillProgramEffectOp.SelectBoundaryLingTongEquipmentPair)]
    private bool QualifyBoundaryLingTongXuanfeng(ProgramTriggerLegalityContext gate)
    {
        if (gate.SkillId != BoundaryLingTongComposition.Skill) return true;
        if (gate.Trigger.Id == "equipment-left") return gate.Context.MovementBatch is { } b &&
            b.Movements.Any(m => m.From == CardLocation.Equipment(gate.OwnerSeat));
        var phase = CurrentActualDiscardRecoveryPhase();
        if (gate.Trigger.Id != "discard-phase" || phase is not { Kind: ActualDiscardRecoveryPhaseKind.Discard } ||
            phase.ActorSeat != gate.OwnerSeat || phase.ActualTurnOwnerSeat != gate.OwnerSeat) return false;
        var invocations = ProgramEventHistory<NativeLogicalDiscardInvocationRecordedEvent>()
            .Where(e => e.Phase == phase && e.ExecutorSeat == gate.OwnerSeat).Select(e => e.InvocationId).ToHashSet();
        return ProgramEventHistory<NativeLogicalDiscardMaterialCompletedEvent>()
            .Count(e => invocations.Contains(e.InvocationId) && e.From.OwnerSeat == gate.OwnerSeat &&
                e.From.Zone is CardZoneKind.Hand or CardZoneKind.Equipment) >= 2;
    }

    private SkillProgramStepOutcome SelectBoundaryLingTongEquipmentPair(ProgramSkillFrame frame)
    {
        var f = GetActiveProgramFrame(frame.Id);
        var choices = BoundaryLingTongEquipmentPairs(f.OwnerSeat).Select(pair => new PromptChoice(
            new ChoiceId($"boundary-ling-tong.{f.Id}.{pair.Source}.{pair.Destination}"),
            $"将 {_players[pair.Source].Name} 的一张装备移动给 {_players[pair.Destination].Name}", [], [pair.Source, pair.Destination],
            new Dictionary<string, string> { ["program-action"] = "boundary-ling-tong-equipment", ["frame-id"] = f.Id.ToString(System.Globalization.CultureInfo.InvariantCulture) })).ToArray();
        if (choices.Length == 0) { CancelProgramBindingAndCleanup(f, "已经没有可移动的其他角色装备。"); return SkillProgramStepOutcome.AwaitChild; }
        var skill = _contentRegistry.GetSkill(f.SkillId);
        _pendingDecision = new PendingDecision(DecisionKind.ProgramTrigger, f.OwnerSeat, "【旋风】选择装备的原角色与接收角色。", [], [], f.OwnerSeat)
        { PromptId = CreatePromptId(), SkillPrompt = new(f.SkillId, skill.Name, "旋风 · 移动装备", skill.Description), Choices = Array.AsReadOnly(choices) };
        _status = _players[f.OwnerSeat].IsHuman ? EngineStatus.AwaitingHumanResponse : EngineStatus.Running;
        return SkillProgramStepOutcome.AwaitChoice;
    }

    [ProgramAiChoice(SkillProgramEffectOp.SelectBoundaryLingTongEquipmentPair)]
    private static PromptChoice ChooseAiBoundaryLingTongEquipmentPair(PendingDecision decision, ProgramSkillFrame frame) => decision.Choices[0];

    [ProgramAnswer("boundary-ling-tong-equipment")]
    private void AnswerBoundaryLingTongEquipmentPair(PromptChoice choice)
    {
        if (_resolutionStack.LastOrDefault() is not ProgramSkillFrame { SkillId: BoundaryLingTongComposition.Skill } f ||
            ProgramInstructionResolver.Default.Resolve(f, _contentRegistry.GetSkill(f.SkillId).Program!).GetPausedInstruction(f.InstructionIndex).Effect.Op != SkillProgramEffectOp.SelectBoundaryLingTongEquipmentPair ||
            _pendingDecision?.PlayerSeat != f.OwnerSeat || choice.Parameters.GetValueOrDefault("frame-id") != f.Id.ToString(System.Globalization.CultureInfo.InvariantCulture) ||
            choice.Cards.Count != 0 || choice.Targets is not [var source, var destination] ||
            !BoundaryLingTongEquipmentPairs(f.OwnerSeat).Contains((source, destination)))
            throw new InvalidOperationException("The mobile Xuanfeng pair no longer matches its original frame and legal equipment transfer.");
        ClearPendingDecision();
        ReplaceRuntimeTop(f with { SelectedTargetSeats = Array.AsReadOnly(new[] { source, destination }) });
        AdvanceEventRulesAndQueueFact(new BoundaryLingTongEquipmentPairSelectedEvent(f.Id, f.OwnerSeat, f.SkillInstanceId, f.GameplayHash, source, destination));
        AdvanceRuntimeProgram(f.Id);
    }

    private bool ValidBoundaryLingTongEquipmentSelection(ProgramSkillFrame f)
    {
        if (f.SkillId != BoundaryLingTongComposition.Skill) return false;
        var receipts = ProgramEventHistory<BoundaryLingTongEquipmentPairSelectedEvent>().Where(e => e.FrameId == f.Id).ToArray();
        if (receipts.Length == 0) return f.SelectedTargetSeats.Count == 0;
        return receipts is [var r] && r.OwnerSeat == f.OwnerSeat && r.SkillInstanceId == f.SkillInstanceId && r.GameplayHash == f.GameplayHash &&
            r.SourceSeat != f.OwnerSeat && r.DestinationSeat != f.OwnerSeat && r.SourceSeat != r.DestinationSeat &&
            f.SelectedTargetSeats.SequenceEqual(new[] { r.SourceSeat, r.DestinationSeat }) &&
            f.ChoiceBindings.Any(c => c.Name == "xuanfeng-branch" && c.OptionId == "move" && c.ChooserSeat == f.OwnerSeat);
    }
}
