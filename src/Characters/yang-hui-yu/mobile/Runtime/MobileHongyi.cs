namespace CardGame.Core;

internal sealed class MarkHongyiTargetDescriptor : ProgramOperationDescriptorBase
{
    public override SkillProgramEffectOp Op => SkillProgramEffectOp.MarkHongyiTarget;
    public override ISkillProgramEffectHandler Handler { get; } = new MarkHongyiTargetHandler();
    public override ProgramOperationAiPolicy AiPolicy { get; } = new(ProgramOperationAiSemantic.SelectTarget,
        static (_, context) => context.PublicControlValue(1));

    public override SkillProgramEffect Parse(ProgramOperationNodeReader reader) => ParseOwnerAlways(reader);
    public override IReadOnlyList<ProgramResourceOperation> Resources(SkillProgramEffect effect) => [new ReadSelectedTarget()];
}

internal sealed class MarkHongyiTargetHandler : ISkillProgramEffectHandler
{
    public SkillProgramEffectOp Op => SkillProgramEffectOp.MarkHongyiTarget;

    public SkillProgramStepOutcome Execute(SkillProgramEffect effect, ProgramSkillFrame frame, int seat,
        ISkillProgramEffectHost host) => host.GetCapability<IMobileHongyi>().Mark(frame);
}

internal interface IMobileHongyi : IProgramRuntimeOperations
{
    SkillProgramStepOutcome Mark(ProgramSkillFrame source);
}

[ProgramRuntimeModule, ProgramCapability]
internal sealed class MobileHongyi(IProgramRuntimeFrames frames, IMatchParticipantStateAccess participants,
    IProgramRuntimeEvents events) : IMobileHongyi
{
    public SkillProgramStepOutcome Mark(ProgramSkillFrame source)
    {
        var frame = frames.ActiveProgram(source.Id);
        if (frame.TriggerId is not null || frame.SelectedTargetSeats is not [var targetSeat] ||
            targetSeat == frame.OwnerSeat || !participants.At(targetSeat).IsAlive)
            throw new InvalidOperationException("弘仪 requires one other living activation target.");

        var target = participants.At(targetSeat);
        var key = (PlayerMarkerKind.Hongyi, frame.OwnerSeat);
        if (target.MarkerSourceCounts.ContainsKey(key))
            return SkillProgramStepOutcome.Continue;
        target.MarkerSourceCounts.Add(key, 1);
        var count = target.Markers.GetValueOrDefault(PlayerMarkerKind.Hongyi) + 1;
        target.Markers[PlayerMarkerKind.Hongyi] = count;
        events.AdvanceFact(new PlayerMarkerChangedEvent(frame.Id, targetSeat, PlayerMarkerKind.Hongyi, 1,
            count, frame.OwnerSeat, "skill-program.mobile.hongyi"));
        return SkillProgramStepOutcome.Continue;
    }

    [ProgramTriggerLegality(ProgramTriggerLegalityPhase.BeforeGainQualification, 62211)]
    private bool IsMarkedDamageSource(ProgramTriggerCandidate candidate, SkillProgramTrigger trigger, ProgramSkillWindowContext context) =>
        candidate.SkillId != "mobile:hongyi" || trigger.Id != "marked-damage-judgment" ||
        context.SourceSeat is { } seat && participants.At(seat).IsAlive &&
        participants.At(seat).MarkerSourceCounts.ContainsKey((PlayerMarkerKind.Hongyi, candidate.OwnerSeat));

    [ProgramRuleObserver(ProgramRulePhase.BeforeEvent, 62211)]
    private void Expire(IGameEvent fact)
    {
        var ownerSeat = fact switch
        {
            TurnStartedEvent started => started.ActorSeat,
            PlayerDiedEvent death => death.VictimSeat,
            CharacterSkillsLostEvent loss when loss.SkillIds.Contains("mobile:hongyi", StringComparer.Ordinal) => loss.Seat,
            _ => -1
        };
        if (ownerSeat < 0)
            return;
        foreach (var holder in participants.Players)
        {
            if (!holder.MarkerSourceCounts.Remove((PlayerMarkerKind.Hongyi, ownerSeat)))
                continue;
            var count = holder.Markers[PlayerMarkerKind.Hongyi] - 1;
            if (count == 0)
                holder.Markers.Remove(PlayerMarkerKind.Hongyi);
            else
                holder.Markers[PlayerMarkerKind.Hongyi] = count;
        }
    }
}
