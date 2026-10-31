namespace CardGame.Core;

// Trusted core primitives receive the actual participant, not a snapshot. A
// mutation can retain the original reference across native/event boundaries.
internal interface IMatchParticipantStateAccess : IProgramRuntimeOperations
{
    IReadOnlyList<CharacterState> Players { get; }
    CharacterState At(int seat);
}

internal sealed class MatchParticipantStateAccess(IReadOnlyList<CharacterState> players) : IMatchParticipantStateAccess
{
    public IReadOnlyList<CharacterState> Players => players;
    public CharacterState At(int seat) => players[seat];
}
