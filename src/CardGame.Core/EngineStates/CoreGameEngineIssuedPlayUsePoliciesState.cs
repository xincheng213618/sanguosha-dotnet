#nullable enable
namespace CardGame.Core.EngineStates;
internal sealed class CoreGameEngineIssuedPlayUsePoliciesState : Contracts.ICoreGameEngineIssuedPlayUsePoliciesIssuedPlayPhaseUseProhibitionsAccess
{
    // Issued phase facts survive source loss/suppression until this actual phase ends.
    private List<IssuedPlayPhaseUseProhibition> _issuedPlayPhaseUseProhibitions = [];
    internal ref readonly List<IssuedPlayPhaseUseProhibition> ReferenceToIssuedPlayPhaseUseProhibitions => ref _issuedPlayPhaseUseProhibitions;

    ref readonly List<IssuedPlayPhaseUseProhibition> Contracts.ICoreGameEngineIssuedPlayUsePoliciesIssuedPlayPhaseUseProhibitionsAccess._issuedPlayPhaseUseProhibitions => ref _issuedPlayPhaseUseProhibitions;
}
