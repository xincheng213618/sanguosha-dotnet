#nullable enable

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterGuoSiOlRuntime
{
    [ProgramTriggerLegality(ProgramTriggerLegalityPhase.BeforeGainQualification, 470)]
    internal bool CanOfferGuoSi(ProgramTriggerCandidate c, SkillProgramTrigger t, ProgramSkillWindowContext w)
    {
        if (!ProgramInstructionResolver.Default.Features(t).HasOperation(SkillProgramEffectOp.OfferSidaoCompletedPairSnatch))
            return true;
        return ProgramExpressions.EvaluateParticipantSet(t.Effects[0].TargetSetExpression!,
            _expressionWorlds.CreateTriggerExpressionWorld(c, w, w.Facts)) is { } targets &&
            SidaoActions(c.OwnerSeat, targets).Count > 0;
    }
}
