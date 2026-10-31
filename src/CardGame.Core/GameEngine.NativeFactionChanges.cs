namespace CardGame.Core;
public sealed record NativeEffectiveFactionChangedEvent(long ChangeId, int TurnNumber, int ActualTurnOwnerSeat, int TargetSeat, string? PreviousFactionId, string CurrentFactionId, long? ParentFrameId, CardConversionSource? Source, string Cause) : IGameEvent;
public sealed partial class CharacterState
{
    internal string? NativeEffectiveFactionOverrideId { get; set; }
    internal bool NativeEffectiveFactionWasDeclared { get; set; }
}
