#nullable enable

namespace CardGame.Core.EngineOwners;
internal sealed partial class CoreGameEngineAttributedMarkerQualificationsRuntime
{
    [ProgramTriggerLegality(ProgramTriggerLegalityPhase.BeforeCondition, 85, SkillProgramEffectOp.ChangeAttributedMarker)]
    internal bool CanOfferAfterDamageAttributedMarker(ProgramTriggerLegalityContext gate)
    {
        var context = gate.Context;
        if (context.Window is not (SkillProgramTriggerWindow.DamageAppliedBeforeDying or SkillProgramTriggerWindow.AfterDamageApplied) || context.SourceSeat is { } sourceSeat && _coreTableSeatQueriesOperations.IsValidPlayerSeat(sourceSeat))
            return true;
        // Source-less damage still pays damage costs and runs owner rewards.
        // It cannot invoke a branch whose actual recipient is the absent
        // damage source. Keep participant resolution strict after admission.
        var owner = _coreNativeSkillContextQueriesOperations.CreateSkillContext(gate.Owner);
        return !gate.Features.ForOperation(SkillProgramEffectOp.ChangeAttributedMarker).Any(effect => effect.TargetReference?.Kind == ProgramParticipantRef.EventSource && effect.Condition.CanEvaluateWithoutProgramFrame() && effect.Condition.Evaluate(owner));
    }
}
