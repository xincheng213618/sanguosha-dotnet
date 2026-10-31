namespace CardGame.Core;

/// <summary>Owns program draw operands, recipient order and returns on the original program frame.</summary>
[ProgramRuntimeModule]
internal sealed class ProgramDrawRuntime(IProgramRuntimeFrames frames, IProgramContentQueries content,
    ICardZoneQueries zones, IMatchRoleQueries roles, ISelectionParticipantQueries participants,
    IProgramDrawFacts facts, IProgramDrawOperations operations, IProgramCardSetBindings bindings)
{
    internal void DrawProgramCards(long frameId, int targetSeat, int amount,
        SkillProgramNumberExpression? numberExpression, string? resultBind,
        SkillProgramCardSetVisibility visibility, CardMoveReason reason)
    {
        var frame = frames.ActiveProgram(frameId);
        var count = ResolveProgramDrawCount(frame, targetSeat, amount, numberExpression);
        if (count is null) return;
        var before = frame.FixedRecipient is null ? 0 : facts.PairBenefitSequence;
        var drawn = operations.DrawCards(targetSeat, count.Value, log: true, reason);
        CompleteProgramDrawCards(frameId, targetSeat, resultBind, visibility, reason, before, drawn, count.Value);
    }

    internal int? ResolveProgramDrawCount(ProgramSkillFrame frame, int targetSeat, int amount,
        SkillProgramNumberExpression? numberExpression)
    {
        if ((numberExpression is SkillProgramNumberExpression.OwnerLostHp or SkillProgramNumberExpression.OwnerMaxHp or
                SkillProgramNumberExpression.CurrentHp or SkillProgramNumberExpression.BoundaryWangYiMijiCount) && !facts.HasHpValue(frame.OwnerSeat) ||
            (numberExpression is SkillProgramNumberExpression.TargetLostHp) && !facts.HasHpValue(targetSeat) ||
            numberExpression == SkillProgramNumberExpression.LivingPlayersMinHp &&
                participants.Seats.Any(seat => participants.IsAlive(seat) && !facts.HasHpValue(seat)) &&
                !participants.Seats.Any(seat => participants.IsAlive(seat) && facts.HasHpValue(seat)))
        { operations.Cancel(frame, "体力值未定义，无法计算此效果。"); return null; }
        return numberExpression switch
        {
            null => amount,
            SkillProgramNumberExpression.OwnerMaxHp => Math.Max(0, facts.MaxHp(frame.OwnerSeat)),
            SkillProgramNumberExpression.CurrentHp => Math.Max(0, participants.Hp(frame.OwnerSeat)),
            SkillProgramNumberExpression.TargetLostHp => Math.Max(0, facts.MaxHp(targetSeat) - participants.Hp(targetSeat)),
            SkillProgramNumberExpression.CurrentHandEmptyTwoOtherwiseOne => zones.Count(CardLocation.Hand(targetSeat)) == 0 ? 2 : 1,
            SkillProgramNumberExpression.OwnerLeastHandTwoOtherwiseOne =>
                participants.Seats.Where(participants.IsAlive).All(seat =>
                    zones.Count(CardLocation.Hand(seat)) >= zones.Count(CardLocation.Hand(targetSeat))) ? 2 : 1,
            SkillProgramNumberExpression.CurrentTurnUsedCardCategoryCount => facts.CurrentTurnCategoryDrawCount(frame, targetSeat),
            SkillProgramNumberExpression.ActualTurnDeathBenefits => facts.ActualTurnDeathDrawCount(frame, targetSeat),
            SkillProgramNumberExpression.CategoryTargetTurnUsage => facts.CategoryTargetTurnUsage(frame),
            SkillProgramNumberExpression.SelectedTargetsHandGreaterThanLord => CountSelectedTargetsHandGreaterThanLord(frame),
            SkillProgramNumberExpression.OwnerLostHp => facts.OwnerLostHp(frame),
            SkillProgramNumberExpression.BoundaryWangYiMijiCount => facts.CommittedMijiCount(frame),
            SkillProgramNumberExpression.LivingFactionCount => participants.Seats.Where(participants.IsAlive)
                .Select(facts.EffectiveFaction).OfType<string>().Distinct(StringComparer.Ordinal).Count(),
            SkillProgramNumberExpression.LivingPlayersMinHp => participants.Seats
                .Where(seat => participants.IsAlive(seat) && facts.HasHpValue(seat))
                .Select(participants.Hp).DefaultIfEmpty().Min(),
            SkillProgramNumberExpression.CurrentAttackRange =>
                frame.WindowContext?.Facts is { } frozen && frozen.CurrentAttackRange > 0
                    ? frozen.CurrentAttackRange
                    : facts.AttackRange(targetSeat),
            SkillProgramNumberExpression.TurnOwnerDiscardPhaseHandDiscardCount => facts.TurnOwnerDiscardPhaseHandDiscardCount,
            _ => throw new InvalidOperationException(
                $"Unsupported draw number expression '{numberExpression}'.")
        };
    }

    private int CountSelectedTargetsHandGreaterThanLord(ProgramSkillFrame frame)
    {
        var lord = participants.Seats.Where(seat => participants.IsAlive(seat) && roles.Role(seat) == Role.Lord)
            .Select(seat => (int?)seat).SingleOrDefault();
        return lord is null ? 0 : frame.SelectedTargetSeats.Count(seat => participants.IsAlive(seat) &&
            zones.Count(CardLocation.Hand(seat)) > zones.Count(CardLocation.Hand(lord.Value)));
    }

    internal void CompleteProgramDrawCards(long frameId, int targetSeat, string? resultBind,
        SkillProgramCardSetVisibility visibility, CardMoveReason reason,
        long fixedRecipientSequenceBefore, IReadOnlyList<int> drawn, int? requestedCount = null)
    {
        var frame = frames.ActiveProgram(frameId);
        if (frame.FixedRecipient is not null)
            operations.CaptureFixedRecipientDraw(frameId, targetSeat, reason, fixedRecipientSequenceBefore, drawn.Count);
        if (frame.WindowContext?.JudgmentReplacement is { } replacement)
        {
            var active = frames.ActiveProgram(frameId);
            frames.ReplaceTop(active with
            {
                WindowContext = active.WindowContext! with
                {
                    JudgmentReplacement = replacement with { DrawnCards = replacement.DrawnCards + drawn.Count }
                }
            });
        }
        if (resultBind is not null)
        {
            bindings.SetProgramCardSet(frameId, resultBind, drawn, visibility,
                drawn.Select(_ => CardLocation.Hand(targetSeat)).ToArray());
        }
        if ((frame.NativeDraw?.RequestedCount ?? requestedCount) is { } requested)
            operations.PublishDrawCompleted(new(frame.Id, frame.InstructionIndex, targetSeat, requested, drawn.Count));
    }

    internal void DrawProgramSelectedTargets(long frameId, int amount, CardMoveReason reason)
    {
        var frame = frames.ActiveProgram(frameId);
        if (amount < 1 || frame.SelectedTargetSeats.Distinct().Count() != frame.SelectedTargetSeats.Count)
            throw new InvalidOperationException("The selected-target draw has invalid participants.");
        if (frame.SelectedTargetSeats.Count == 0)
        {
            var plan = ProgramInstructionResolver.Default.Resolve(frame, content.Skill(frame.SkillId).Program!);
            if (!NativeDrawProducerRuntime.IsCommittedEmptyTargetDraw(frame, plan, amount))
                throw new InvalidOperationException("An empty draw requires its committed optional target selection.");
            return;
        }
        foreach (var seat in frame.SelectedTargetSeats)
        {
            if (participants.IsAlive(seat))
                DrawProgramCards(frameId, seat, amount, null, null, SkillProgramCardSetVisibility.Private, reason);
        }
    }

    internal int GetProgramPlannedNormalDrawCount(ProgramSkillFrame frame, bool allowReplacedNoOp = false)
    {
        if (frame.WindowContext is not
            { Window: SkillProgramTriggerWindow.DrawPhaseStarting, ParentFrameId: var parentId } ||
            frames.Top is not { } top || frames.ParentOf(top.Id) is not ProgramLifecycleTriggerWindowFrame parent ||
            parent.Id != parentId || parent.Continuation != ProgramLifecycleContinuation.CompleteDrawPhase ||
            parent.NormalDrawReplaced && !allowReplacedNoOp)
            throw new InvalidOperationException(
                "A planned normal-draw target limit requires its active draw-phase parent.");
        if (parent.NormalDrawReplaced) return 0;
        var baseDrawCount = parent.FrozenBaseDrawCount ?? operations.FreezeNormalDrawBase(frame.OwnerSeat);
        if (parent.FrozenBaseDrawCount is null)
            frames.Replace(parent.Id, parent with { FrozenBaseDrawCount = baseDrawCount });
        return Math.Max(0, checked(baseDrawCount + parent.NormalDrawAdjustment));
    }
}
