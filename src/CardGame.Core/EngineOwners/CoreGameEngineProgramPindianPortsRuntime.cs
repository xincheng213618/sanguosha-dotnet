#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
[global::CardGame.Core.ProgramRuntimeModule, global::CardGame.Core.ProgramCapability]
internal sealed partial class CoreGameEngineProgramPindianPortsRuntime : global::CardGame.Core.IProgramPindianPolicyQueries, global::CardGame.Core.IProgramPindianProducer
{
    private readonly ICoreProgramPindianPortsCharacterLuKangWumiaoOperations _characterLuKangWumiaoOperations;
    private readonly ICoreProgramPindianPortsCorePindianOperations _corePindianOperations;
    private readonly ICoreProgramPindianPortsCoreProgramCardPoliciesOperations _coreProgramCardPoliciesOperations;
    private readonly global::CardGame.Core.EngineStates.Contracts.ICoreMatchParticipantsPlayersAccess _coreMatchParticipantsPlayersAccess;
    internal CoreGameEngineProgramPindianPortsRuntime(ICoreProgramPindianPortsCharacterLuKangWumiaoOperations characterLuKangWumiaoOperations, ICoreProgramPindianPortsCorePindianOperations corePindianOperations, ICoreProgramPindianPortsCoreProgramCardPoliciesOperations coreProgramCardPoliciesOperations, global::CardGame.Core.EngineStates.Contracts.ICoreMatchParticipantsPlayersAccess coreMatchParticipantsPlayersAccess)
    {
        this._characterLuKangWumiaoOperations = characterLuKangWumiaoOperations;
        this._corePindianOperations = corePindianOperations;
        this._coreProgramCardPoliciesOperations = coreProgramCardPoliciesOperations;
        this._coreMatchParticipantsPlayersAccess = coreMatchParticipantsPlayersAccess;
    }
}
