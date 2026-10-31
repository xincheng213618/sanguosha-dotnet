#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
[global::CardGame.Core.ProgramRuntimeModule]
internal sealed partial class CharacterDuanWeiOlRuntime
{
    private readonly global::CardGame.Core.EngineOwners.IKernelFrameStoreAccess _resolutionStackOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineAlcoholIdentityPoliciesEntries _coreAlcoholIdentityPoliciesOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineAlternatingBenefitsEntries _coreAlternatingBenefitsOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineAlternativeViewAsCostsEntries _coreAlternativeViewAsCostsOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineCardAttackStateEntries _coreCardAttackStateOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CorePhysicalOwnedCardQueriesEntries _corePhysicalOwnedCardQueriesOperations;
    internal CharacterDuanWeiOlRuntime(global::CardGame.Core.EngineOwners.IKernelFrameStoreAccess resolutionStackOperations, global::CardGame.Core.RuntimeEntries.CoreGameEngineAlcoholIdentityPoliciesEntries coreAlcoholIdentityPoliciesOperations, global::CardGame.Core.RuntimeEntries.CoreGameEngineAlternatingBenefitsEntries coreAlternatingBenefitsOperations, global::CardGame.Core.RuntimeEntries.CoreGameEngineAlternativeViewAsCostsEntries coreAlternativeViewAsCostsOperations, global::CardGame.Core.RuntimeEntries.CoreGameEngineCardAttackStateEntries coreCardAttackStateOperations, global::CardGame.Core.RuntimeEntries.CorePhysicalOwnedCardQueriesEntries corePhysicalOwnedCardQueriesOperations)
    {
        this._resolutionStackOperations = resolutionStackOperations;
        this._coreAlcoholIdentityPoliciesOperations = coreAlcoholIdentityPoliciesOperations;
        this._coreAlternatingBenefitsOperations = coreAlternatingBenefitsOperations;
        this._coreAlternativeViewAsCostsOperations = coreAlternativeViewAsCostsOperations;
        this._coreCardAttackStateOperations = coreCardAttackStateOperations;
        this._corePhysicalOwnedCardQueriesOperations = corePhysicalOwnedCardQueriesOperations;
    }
}
