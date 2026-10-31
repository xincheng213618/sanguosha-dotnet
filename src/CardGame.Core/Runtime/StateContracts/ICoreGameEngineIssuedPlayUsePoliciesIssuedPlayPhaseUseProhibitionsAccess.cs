#nullable enable
namespace CardGame.Core.EngineStates.Contracts;
internal interface ICoreGameEngineIssuedPlayUsePoliciesIssuedPlayPhaseUseProhibitionsAccess : IProgramRuntimeOperations
{
    ref readonly List<IssuedPlayPhaseUseProhibition> _issuedPlayPhaseUseProhibitions { get; }
}
