#nullable enable
namespace CardGame.Core.EngineStates.Contracts;
internal interface ICharacterZhugeGuoOlHasIssuedZhugeGuoStateAccess : IProgramRuntimeOperations
{
    ref bool _hasIssuedZhugeGuoState { get; }
}
