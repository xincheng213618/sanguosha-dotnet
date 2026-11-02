namespace CardGame.Core;

internal interface INativeCardUseAdmission : IProgramSelectionQuery
{
    bool IsDeclaredUse(CardUseFrame use, CardUseDeclaredEvent declaration);
    bool IsLegacyProducerUse(CardUseFrame use);
}

[ProgramRuntimeModule, ProgramCapability]
internal sealed class NativeCardUseAdmission(IProgramRuntimeFrames frames, IProgramContentQueries content)
    : INativeCardUseAdmission
{
    public bool IsDeclaredUse(CardUseFrame use, CardUseDeclaredEvent declaration) =>
        ReferenceEquals(frames.Find(use.Id), use) && declaration.ResolutionId == use.Id &&
        declaration.SourceSeat == use.SourceSeat && declaration.CardId == use.CardId && declaration.CardKind == use.CardKind &&
        (use.Action is not { } action || action.Type == CardActionType.Use &&
            action.ActorSeat == use.SourceSeat && action.EffectiveKind == use.CardKind);

    public bool IsLegacyProducerUse(CardUseFrame use)
    {
        if (frames.Find(use.Id) is not CardUseFrame || use.CardId != 0 || use.PhysicalCardIds is not { Count: 0 } ||
            frames.ParentOf(use.Id) is not ProgramSkillFrame producer || producer.OwnerSeat != use.SourceSeat)
            return false;
        var program = content.FindSkill(producer.SkillId)?.Program;
        if (program is null || program.GameplayHash != producer.GameplayHash)
            return false;
        var paused = ProgramInstructionResolver.Default.Resolve(producer, program)
            .GetPausedInstruction(producer.InstructionIndex).Effect;
        if (use.CardKind == CardKind.Slash)
            return producer.SelectedTargetSeats.Count == 1 && producer.SelectedTargetSeats.SequenceEqual(use.TargetSeats) &&
                paused is { Op: SkillProgramEffectOp.UseVirtualCard, OutputKind: CardKind.Slash, UseCardActionWindows: false };
        if (use.CardKind != CardKind.Alcohol || paused.Op != SkillProgramEffectOp.UseVirtualDyingAlcohol ||
            producer.WindowContext is not { Window: SkillProgramTriggerWindow.SelfDyingResponse } context ||
            context.TargetSeat != use.SourceSeat || !use.TargetSeats.SequenceEqual([use.SourceSeat]))
            return false;
        return frames.Find(context.ParentFrameId) is DyingFrame dying && dying.VictimSeat == use.SourceSeat &&
            dying.ResponderIndex >= 0 && dying.ResponderIndex < dying.ResponderSeats.Count &&
            dying.ResponderSeats[dying.ResponderIndex] == use.SourceSeat;
    }
}
