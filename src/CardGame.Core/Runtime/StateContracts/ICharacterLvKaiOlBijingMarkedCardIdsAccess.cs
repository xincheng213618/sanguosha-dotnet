#nullable enable
namespace CardGame.Core.EngineStates.Contracts;
internal interface ICharacterLvKaiOlBijingMarkedCardIdsAccess : global::CardGame.Core.IProgramRuntimeOperations
{
    ref readonly global::System.Collections.Generic.Dictionary<int, int> _bijingMarkedCardIds { get; }
}
