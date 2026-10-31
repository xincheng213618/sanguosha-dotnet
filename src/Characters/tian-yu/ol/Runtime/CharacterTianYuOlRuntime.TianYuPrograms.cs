#nullable enable

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterTianYuOlRuntime
{
    internal IReadOnlyList<ProgramDirectedDistanceGrantedEvent> ActiveDirectedDistanceGrants(int ownerSeat, int targetSeat) => ((ZhuitaoDistanceRuntime)_closedZhuitaoDistanceRuntimeSource.Get()).ActiveGrants(ownerSeat, targetSeat);
    // Shared consumption: every active directed grant against the queried pair
    // joins the outgoing distance as an additive contribution, bounded to the
    // game minimum of one, beneath the Set-style distance overlays.
    [ProgramDistanceRule(ProgramDistanceStage.DistanceProjection, 10)]
    internal RuleQueryEvaluation ApplyProgramDirectedDistance(CharacterState source, CharacterState target, RuleQueryEvaluation original) => ((ZhuitaoDistanceRuntime)_closedZhuitaoDistanceRuntimeSource.Get()).ApplyDistance(source.Seat, target.Seat, original);
    SkillProgramStepOutcome ITianYuProgramHost.SaodiExpandTargets(ProgramSkillFrame frame, SkillProgramEffect effect) => SaodiProgramExpandTargets(frame, effect);
    SkillProgramStepOutcome ITianYuProgramHost.ZhuitaoMarkTarget(ProgramSkillFrame frame, SkillProgramEffect effect) => ZhuitaoProgramMarkTarget(frame, effect);
    SkillProgramStepOutcome ITianYuProgramHost.ZhuitaoRevoke(ProgramSkillFrame frame, SkillProgramEffect effect) => ZhuitaoProgramRevoke(frame, effect);
}
