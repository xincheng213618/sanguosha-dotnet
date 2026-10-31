#nullable enable
namespace CardGame.Core.EngineStates.Contracts;
internal interface ICharacterLvKaiOlBijingMarkedCardIdsAccess : IProgramRuntimeOperations
{
    ref readonly Dictionary<int, int> _bijingMarkedCardIds { get; }
}
