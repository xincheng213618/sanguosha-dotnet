namespace CardGame.Core;

public sealed partial class GameEngine
{
    // A match-local immutable service cache, never a second allegiance or pending-state store.
    private IMatchModeAllegiancePolicy? _allegiancePolicy;
    private IMatchModeAllegiancePolicy ModeAllegiancePolicy => _allegiancePolicy ??=
        _modeRuntime.Find<IMatchModeAllegiancePolicy>() ??
            throw new InvalidOperationException("The mode requires its installed allegiance policy.");

    private static ModeAllegianceSeatFacts CaptureAllegianceSeat(CharacterState player) =>
        new(player.Seat, player.IsAlive, player.TeamId);
}
