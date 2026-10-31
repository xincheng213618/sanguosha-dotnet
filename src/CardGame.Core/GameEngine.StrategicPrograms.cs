namespace CardGame.Core;
public sealed record ProgramSkillSuppressedEvent(int OwnerSeat, int TargetSeat, string SkillId, bool Suppressed) : IGameEvent;
public sealed partial class GameEngine
{
    internal sealed record ProgramSuppression(int SourceSeat, int TargetSeat, string SkillId, int CreatedTurn, IReadOnlyList<string> GrantIds);
    private ref readonly global::System.Collections.Generic.List<global::CardGame.Core.GameEngine.ProgramSuppression> _programSuppressions => ref _strategicPrograms.ReferenceToProgramSuppressions;
    private ref int _strategicEndPlayTurn => ref _strategicPrograms.ReferenceToStrategicEndPlayTurn;
    private ref readonly global::System.Collections.Generic.Dictionary<int, int> _strategicHandLimitPenaltyTurns => ref _strategicPrograms.ReferenceToStrategicHandLimitPenaltyTurns;
}
