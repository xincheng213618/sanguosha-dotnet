namespace CardGame.Core;
public sealed record ProgramPhaseColorRestriction(string SkillId, string SkillInstanceId, int OwnerSeat, int TargetSeat, int TurnNumber, bool IsRed, bool UsedSlash = false);
public sealed record ProgramPhaseColorRestrictionGrantedEvent(ProgramPhaseColorRestriction Restriction) : IGameEvent;
public sealed partial class GameEngine
{
    private ref readonly global::System.Collections.Generic.List<global::CardGame.Core.ProgramPhaseColorRestriction> _programPhaseColorRestrictions => ref _programPhaseColorRestrictions2.ReferenceToProgramPhaseColorRestrictions;
}
