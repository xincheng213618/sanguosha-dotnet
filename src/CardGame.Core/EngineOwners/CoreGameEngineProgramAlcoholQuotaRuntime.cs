#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
[global::CardGame.Core.ProgramRuntimeModule]
internal sealed partial class CoreGameEngineProgramAlcoholQuotaRuntime
{
    private readonly ICoreProgramAlcoholQuotaCoreAlternatingBenefitsOperations _coreAlternatingBenefitsOperations;
    private readonly ICoreProgramAlcoholQuotaCoreNationalSkillsOperations _coreNationalSkillsOperations;
    private readonly ICoreProgramAlcoholQuotaCoreProgramCardPoliciesOperations _coreProgramCardPoliciesOperations;
    private readonly global::CardGame.Core.EngineStates.Contracts.ICoreActualTurnProgressTurnNumberAccess _coreActualTurnProgressTurnNumberAccess;
    internal CoreGameEngineProgramAlcoholQuotaRuntime(ICoreProgramAlcoholQuotaCoreAlternatingBenefitsOperations coreAlternatingBenefitsOperations, ICoreProgramAlcoholQuotaCoreNationalSkillsOperations coreNationalSkillsOperations, ICoreProgramAlcoholQuotaCoreProgramCardPoliciesOperations coreProgramCardPoliciesOperations, global::CardGame.Core.EngineStates.Contracts.ICoreActualTurnProgressTurnNumberAccess coreActualTurnProgressTurnNumberAccess)
    {
        this._coreAlternatingBenefitsOperations = coreAlternatingBenefitsOperations;
        this._coreNationalSkillsOperations = coreNationalSkillsOperations;
        this._coreProgramCardPoliciesOperations = coreProgramCardPoliciesOperations;
        this._coreActualTurnProgressTurnNumberAccess = coreActualTurnProgressTurnNumberAccess;
    }
}
