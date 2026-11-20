namespace CardGame.Core;

internal interface IMatchRoleQueries : IProgramSelectionQuery
{
    Role Role(int seat);
    int CountLiving(Role role);
}

// Roles remain on the original participants and are read at the point of use.
internal sealed class MatchRoleQueries(IReadOnlyList<CharacterState> players) : IMatchRoleQueries
{
    public Role Role(int seat) => players[seat].Role;

    public int CountLiving(Role role) => players.Count(player => player.IsAlive && player.Role == role);
}
