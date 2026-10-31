#nullable enable
namespace CardGame.Core.EngineStates;
internal sealed class CoreGameEngineIssuedPlayUsePoliciesState : global::CardGame.Core.EngineStates.Contracts.ICoreGameEngineIssuedPlayUsePoliciesIssuedPlayPhaseUseProhibitionsAccess
{
    // Issued phase facts survive source loss/suppression until this actual phase ends.
    private global::System.Collections.Generic.List<global::CardGame.Core.IssuedPlayPhaseUseProhibition> _issuedPlayPhaseUseProhibitions = [];
    internal ref readonly global::System.Collections.Generic.List<global::CardGame.Core.IssuedPlayPhaseUseProhibition> ReferenceToIssuedPlayPhaseUseProhibitions => ref _issuedPlayPhaseUseProhibitions;

    ref readonly global::System.Collections.Generic.List<global::CardGame.Core.IssuedPlayPhaseUseProhibition> global::CardGame.Core.EngineStates.Contracts.ICoreGameEngineIssuedPlayUsePoliciesIssuedPlayPhaseUseProhibitionsAccess._issuedPlayPhaseUseProhibitions => ref _issuedPlayPhaseUseProhibitions;
}
