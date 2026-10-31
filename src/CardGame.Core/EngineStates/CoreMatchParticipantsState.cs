#nullable enable
namespace CardGame.Core.EngineStates;
internal sealed class CoreMatchParticipantsState : global::CardGame.Core.EngineStates.Contracts.ICoreMatchParticipantsPlayersAccess
{
    private global::System.Collections.Generic.List<global::CardGame.Core.CharacterState> _players = [];
    internal ref readonly global::System.Collections.Generic.List<global::CardGame.Core.CharacterState> ReferenceToPlayers => ref _players;

    ref readonly global::System.Collections.Generic.List<global::CardGame.Core.CharacterState> global::CardGame.Core.EngineStates.Contracts.ICoreMatchParticipantsPlayersAccess._players => ref _players;
}
