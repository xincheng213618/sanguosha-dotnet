namespace CardGame.Core;

internal interface IProgramPindianPolicyQueries : IProgramSelectionQuery
{
    bool ProhibitsPindianTarget(int seat);
    bool CanDiscardForPindian(int seat);
}

internal interface IProgramPindianProducer : IProgramRuntimeOperations
{
    void Begin(long programFrameId, SkillPromptPresentation presentation, int sourceSeat, int opponentSeat,
        string resultBind, SkillProgramCardSetVisibility visibility);
}

/// <summary>Qualifies native program contests against their original participants and owning phase frames.</summary>
[ProgramRuntimeModule]
internal sealed class ProgramPindianRuntime(IProgramRuntimeFrames frames, ICardZoneQueries zones,
    IProgramContentQueries content, ISelectionParticipantQueries participants, IProgramPhaseFrameQueries phaseFrames,
    IProgramPindianPolicyQueries policies, IProgramPindianProducer producer)
{
    internal SkillProgramStepOutcome Start(ProgramSkillFrame frame,
        ProgramParticipantReference opponentReference, string resultBind, SkillProgramCardSetVisibility visibility)
    {
        var eventOpponent = opponentReference.Kind == ProgramParticipantRef.EventTarget &&
            frame.WindowContext is { Window: SkillProgramTriggerWindow.AfterDamageApplied, TargetSeat: { } };
        var phaseOpponent = opponentReference.Kind == ProgramParticipantRef.SelectedTarget &&
            (frame.WindowContext is { Window: SkillProgramTriggerWindow.PlayPhaseStarting } ||
             IsExactOwnPlayEndingPindian(frame, opponentReference, resultBind, visibility) || phaseFrames.IsForeignActualTurnPindian(frame));
        if (!eventOpponent && (frame.TriggerId is not null && !phaseOpponent ||
                opponentReference.Kind != ProgramParticipantRef.SelectedTarget ||
                frame.SelectedTargetSeats.Count != 1 ||
                zones.CardsAt(CardLocation.Processing).Count != 0) ||
            frame.PindianResultBindings.Any(item => item.Name == resultBind) || HasPendingBoundCards())
            throw new InvalidOperationException(
                "Program Pindian requires an active selected opponent or a damage-event opponent and no pending program cards.");
        var opponentSeat = eventOpponent ? frame.WindowContext!.TargetSeat!.Value : frame.SelectedTargetSeats.Single();
        if (!participants.IsAlive(frame.OwnerSeat) || !participants.IsAlive(opponentSeat) || opponentSeat == frame.OwnerSeat ||
            !ProgramOperationLegalityPolicy.HandContest.CanSelectTarget(Participant(frame.OwnerSeat), Participant(opponentSeat)) ||
            zones.CardsAt(CardLocation.Hand(frame.OwnerSeat)).Count == 0 && !policies.CanDiscardForPindian(frame.OwnerSeat) ||
            zones.CardsAt(CardLocation.Hand(opponentSeat)).Count == 0 && !policies.CanDiscardForPindian(opponentSeat))
            throw new InvalidOperationException("Program Pindian participants no longer have a legal native source.");
        var skill = content.Skill(frame.SkillId);
        producer.Begin(frame.Id,
            new SkillPromptPresentation(frame.SkillId, skill.Name, $"{skill.Name} · 拼点", skill.Description),
            frame.OwnerSeat, opponentSeat, resultBind, visibility);
        return SkillProgramStepOutcome.AwaitChild;
    }

    private ProgramLegalityParticipant Participant(int seat) => new(seat,
        zones.CardsAt(CardLocation.Hand(seat)).Count, policies.ProhibitsPindianTarget(seat), policies.CanDiscardForPindian(seat));

    private bool HasPendingBoundCards() => frames.OfType<ProgramSkillFrame>()
        .SelectMany(frame => frame.CardSetBindings).SelectMany(binding => binding.CardIds).Distinct()
        .Any(cardId => zones.GetLocation(cardId) == CardLocation.Processing);

    private bool IsExactOwnPlayEndingPindian(ProgramSkillFrame frame, ProgramParticipantReference opponentReference,
        string resultBind, SkillProgramCardSetVisibility visibility)
    {
        if (!phaseFrames.IsExactOwnPlayEndingProgram(frame) || frame.InstructionIndex <= 0 || frames.Top?.Id != frame.Id) return false;
        var effect = ProgramInstructionResolver.Default.Resolve(frame, content.Skill(frame.SkillId).Program!)
            .GetPausedInstruction(frame.InstructionIndex).Effect;
        return effect.Op == SkillProgramEffectOp.StartPindian && effect.OpponentReference == opponentReference &&
            effect.ResultBind == resultBind && effect.Visibility == visibility &&
            effect.Condition.Kind == SkillProgramConditionKind.Always;
    }
}
