namespace CardGame.Core;

internal sealed partial class ProgramTargetSelectionRuntime
{
    internal IReadOnlyList<int> SelectionSeats(int ownerSeat, SkillProgramEffect selection,
        IReadOnlyList<SkillProgramEffect> instructions, ProgramSkillWindowContext? context = null)
    {
        var seats = GetProgramTargetSeats(ownerSeat, selection.TargetKind!.Value, context,
            selection.Marker, selection.ActorReference);
        if (!instructions.Select((effect, index) => (effect, index)).Any(item =>
                ReferenceEquals(item.effect, selection) &&
                facts.IsPindianSourceSelectionInstruction(instructions, item.index))) return seats;
        return seats.Concat(Players.Where(player => player.IsAlive && player.Seat != ownerSeat &&
                pindianPolicies.CanDiscardForPindian(player.Seat) && CanBePindianTarget(ownerSeat, player.Seat))
            .Select(player => player.Seat)).Distinct().Order().ToArray();
    }

    internal IReadOnlyList<int> SelectionSeats(ProgramSkillFrame frame, SkillProgramTargetKind kind,
        IReadOnlyList<CardZoneKind> selectedZones, PlayerMarkerKind? marker, ProgramParticipantReference? actor)
    {
        var plan = ProgramInstructionResolver.Default.Resolve(frame, content.Skill(frame.SkillId).Program!);
        var index = frame.InstructionIndex - 1;
        return facts.IsPindianSourceSelectionInstruction(plan.Instructions, index) && selectedZones.Count == 0 &&
            plan.Instructions[index].TargetKind == kind && plan.Instructions[index].Marker == marker &&
            plan.Instructions[index].ActorReference == actor
            ? SelectionSeats(frame.OwnerSeat, plan.Instructions[index], plan.Instructions, frame.WindowContext)
            : GetProgramTargetSeats(frame.OwnerSeat, kind, frame.WindowContext, marker, actor);
    }

    internal bool SelectionTargetEligible(ProgramSkillFrame frame, SkillProgramTargetKind kind,
        IReadOnlyList<CardZoneKind> selectedZones, int seat, PlayerMarkerKind? marker, ProgramParticipantReference? actor) =>
        IsProgramTargetEligible(frame.OwnerSeat, kind, selectedZones, seat, marker, frame.WindowContext, actor) ||
        selectedZones.Count == 0 && SelectionSeats(frame, kind, selectedZones, marker, actor).Contains(seat);

    internal bool CanSelectPindianOpponent(ProgramSkillFrame frame, int targetSeat)
    {
        if (CanBePindianTarget(frame.OwnerSeat, targetSeat)) return true;
        var instructions = ProgramInstructionResolver.Default.Resolve(frame, content.Skill(frame.SkillId).Program!).Instructions;
        // Only the immediately following unconditional owner contest consumes this selection.
        if (frame.InstructionIndex < instructions.Count &&
            instructions[frame.InstructionIndex] is
            {
                Op: SkillProgramEffectOp.StartPindian,
                Target: SkillProgramEffectTarget.Owner,
                Condition.Kind: SkillProgramConditionKind.Always,
                OpponentReference.Kind: ProgramParticipantRef.SelectedTarget
            }) return false;
        return true;
    }

    private bool CanBePindianTarget(int sourceSeat, int targetSeat) =>
        ProgramOperationLegalityPolicy.HandContest.CanSelectTarget(PindianParticipant(sourceSeat), PindianParticipant(targetSeat));

    private ProgramLegalityParticipant PindianParticipant(int seat) => new(seat, Hand(Player(seat)).Count,
        pindianPolicies.ProhibitsPindianTarget(seat), pindianPolicies.CanDiscardForPindian(seat));
}
