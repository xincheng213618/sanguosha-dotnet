namespace CardGame.Core;

public sealed partial class GameEngine
{
    [ProgramTriggerLegality(ProgramTriggerLegalityPhase.BeforeCondition, 85, SkillProgramEffectOp.ChangeAttributedMarker)]
    private bool CanOfferAfterDamageAttributedMarker(ProgramTriggerLegalityContext gate)
    {
        var context = gate.Context;
        if (context.Window is not (SkillProgramTriggerWindow.DamageAppliedBeforeDying or SkillProgramTriggerWindow.AfterDamageApplied) ||
            context.SourceSeat is { } sourceSeat && IsValidPlayerSeat(sourceSeat)) return true;

        // Source-less damage still pays damage costs and runs owner rewards.
        // It cannot invoke a branch whose actual recipient is the absent
        // damage source. Keep participant resolution strict after admission.
        var owner = CreateSkillContext(gate.Owner);
        return !gate.Features.ForOperation(SkillProgramEffectOp.ChangeAttributedMarker).Any(effect =>
            effect.TargetReference?.Kind == ProgramParticipantRef.EventSource &&
            effect.Condition.CanEvaluateWithoutProgramFrame() && effect.Condition.Evaluate(owner));
    }
}
