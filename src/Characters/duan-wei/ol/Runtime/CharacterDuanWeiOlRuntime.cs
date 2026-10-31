#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
[global::CardGame.Core.ProgramRuntimeModule]
internal sealed partial class CharacterDuanWeiOlRuntime
{
    private readonly ICharacterDuanWeiOlResolutionStackOperations _resolutionStackOperations;
    private readonly ICharacterDuanWeiOlCoreAlcoholIdentityPoliciesOperations _coreAlcoholIdentityPoliciesOperations;
    private readonly ICharacterDuanWeiOlCoreAlternatingBenefitsOperations _coreAlternatingBenefitsOperations;
    private readonly ICharacterDuanWeiOlCoreAlternativeViewAsCostsOperations _coreAlternativeViewAsCostsOperations;
    private readonly ICharacterDuanWeiOlCoreCardAttackStateOperations _coreCardAttackStateOperations;
    private readonly ICharacterDuanWeiOlCorePhysicalOwnedCardQueriesOperations _corePhysicalOwnedCardQueriesOperations;
    internal CharacterDuanWeiOlRuntime(ICharacterDuanWeiOlResolutionStackOperations resolutionStackOperations, ICharacterDuanWeiOlCoreAlcoholIdentityPoliciesOperations coreAlcoholIdentityPoliciesOperations, ICharacterDuanWeiOlCoreAlternatingBenefitsOperations coreAlternatingBenefitsOperations, ICharacterDuanWeiOlCoreAlternativeViewAsCostsOperations coreAlternativeViewAsCostsOperations, ICharacterDuanWeiOlCoreCardAttackStateOperations coreCardAttackStateOperations, ICharacterDuanWeiOlCorePhysicalOwnedCardQueriesOperations corePhysicalOwnedCardQueriesOperations)
    {
        this._resolutionStackOperations = resolutionStackOperations;
        this._coreAlcoholIdentityPoliciesOperations = coreAlcoholIdentityPoliciesOperations;
        this._coreAlternatingBenefitsOperations = coreAlternatingBenefitsOperations;
        this._coreAlternativeViewAsCostsOperations = coreAlternativeViewAsCostsOperations;
        this._coreCardAttackStateOperations = coreCardAttackStateOperations;
        this._corePhysicalOwnedCardQueriesOperations = corePhysicalOwnedCardQueriesOperations;
    }
}
