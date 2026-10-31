#nullable enable
namespace CardGame.Core.EngineStates;
internal sealed class CoreMatchParticipantsState : Contracts.ICoreMatchParticipantsPlayersAccess
{
    private List<CharacterState> _players = [];
    internal ref readonly List<CharacterState> ReferenceToPlayers => ref _players;

    ref readonly List<CharacterState> Contracts.ICoreMatchParticipantsPlayersAccess._players => ref _players;
}
