#nullable enable
namespace CardGame.Core.EngineStates.Contracts;
internal interface ICoreMatchParticipantsPlayersAccess : IProgramRuntimeOperations
{
    ref readonly List<CharacterState> _players { get; }
}
