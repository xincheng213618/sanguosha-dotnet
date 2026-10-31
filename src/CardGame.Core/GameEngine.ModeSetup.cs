namespace CardGame.Core;
public sealed partial class GameEngine
{
    private ref int? _modeInitialActorSeat => ref _matchSetup.ReferenceToModeInitialActorSeat;

    private MatchParticipantSetup? _participantSetup;
    private MatchParticipantSetup ParticipantSetup => _participantSetup ??= new(_contentRegistry, _generalPool);
}
