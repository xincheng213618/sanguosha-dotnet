namespace CardGame.Core;
public sealed record NativeEffectiveFactionChangedEvent(long ChangeId, int TurnNumber, int ActualTurnOwnerSeat, int TargetSeat, string? PreviousFactionId, string CurrentFactionId, long? ParentFrameId, CardConversionSource? Source, string Cause) : IGameEvent;
public sealed partial class CharacterState
{
    internal string? NativeEffectiveFactionOverrideId { get; set; }
    internal bool NativeEffectiveFactionWasDeclared { get; set; }
}

public sealed partial class GameEngine
{
    internal sealed record NativeFactionAvatar(PrivateGeneralLibrarySource Source, string GeneralId, string? SkillId, string? SelectedFactionId);
    internal sealed record NativeFactionObservation(string? FactionId, global::CardGame.Core.GameEngine.NativeFactionAvatar? Avatar);
    private ref readonly global::System.Collections.Generic.Dictionary<int, global::CardGame.Core.GameEngine.NativeFactionObservation> _nativeFactionObservations => ref _nativeFactionChanges.ReferenceToNativeFactionObservations;
    private ref long _nativeFactionRevision => ref _nativeFactionChanges.ReferenceToNativeFactionRevision;
    private ref long _nativeFactionQualificationStamp => ref _nativeFactionChanges.ReferenceToNativeFactionQualificationStamp;
    private ref (long Base, long Revision) _nativeFactionQualificationDependencies => ref _nativeFactionChanges.ReferenceToNativeFactionQualificationDependencies;
    private ref bool _synchronizingNativeFactions => ref _nativeFactionChanges.ReferenceToSynchronizingNativeFactions;
}
