#nullable enable
namespace CardGame.Core.EngineStates.Contracts;
internal interface ICoreGameEnginePrivateWhirlHasIssuedPrivateWhirlAccess : IProgramRuntimeOperations
{
    ref bool _hasIssuedPrivateWhirl { get; }
}
