#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
[global::CardGame.Core.ProgramRuntimeModule, global::CardGame.Core.ProgramCapability]
internal sealed partial class CharacterXuanGongZhuOlRuntime : global::CardGame.Core.IXuanGongZhuProgramHost
{
    private readonly ICharacterXuanGongZhuOlResolutionStackOperations _resolutionStackOperations;
    private readonly ICharacterXuanGongZhuOlCoreDyingCursorRuntimeOperations _coreDyingCursorRuntimeOperations;
    private readonly ICharacterXuanGongZhuOlCoreAlternatingBenefitsOperations _coreAlternatingBenefitsOperations;
    private readonly ICharacterXuanGongZhuOlCoreIdentityConcealmentOperations _coreIdentityConcealmentOperations;
    private readonly ICharacterXuanGongZhuOlCoreObserverSubtreesOperations2 _coreObserverSubtreesOperations;
    private readonly ICharacterXuanGongZhuOlCorePrivateOfferReturnsOperations _corePrivateOfferReturnsOperations;
    private readonly ICharacterXuanGongZhuOlCoreQimeiProgramsOperations _coreQimeiProgramsOperations;
    private readonly ICharacterXuanGongZhuOlCoreTypedRecoveryDamageCursorOperations _coreTypedRecoveryDamageCursorOperations;
    private readonly ICharacterXuanGongZhuOlCoreZhuijiProgramsOperations _coreZhuijiProgramsOperations;
    private readonly ICharacterXuanGongZhuOlCorePendingDecisionPolicyPipelineOperations _corePendingDecisionPolicyPipelineOperations;
    private readonly ICharacterXuanGongZhuOlCharacterXuanGongZhuOlOperations _characterXuanGongZhuOlOperations;
    private readonly ICharacterXuanGongZhuOlCoreObserverSubtreesOperations _coreObserverSubtreesOperations2;
    private readonly global::CardGame.Core.EngineStates.Contracts.ICoreCardMovementJournalCardMovementsAccess _coreCardMovementJournalCardMovementsAccess;
    internal CharacterXuanGongZhuOlRuntime(ICharacterXuanGongZhuOlResolutionStackOperations resolutionStackOperations, ICharacterXuanGongZhuOlCoreDyingCursorRuntimeOperations coreDyingCursorRuntimeOperations, ICharacterXuanGongZhuOlCoreAlternatingBenefitsOperations coreAlternatingBenefitsOperations, ICharacterXuanGongZhuOlCoreIdentityConcealmentOperations coreIdentityConcealmentOperations, ICharacterXuanGongZhuOlCoreObserverSubtreesOperations2 coreObserverSubtreesOperations, ICharacterXuanGongZhuOlCorePrivateOfferReturnsOperations corePrivateOfferReturnsOperations, ICharacterXuanGongZhuOlCoreQimeiProgramsOperations coreQimeiProgramsOperations, ICharacterXuanGongZhuOlCoreTypedRecoveryDamageCursorOperations coreTypedRecoveryDamageCursorOperations, ICharacterXuanGongZhuOlCoreZhuijiProgramsOperations coreZhuijiProgramsOperations, ICharacterXuanGongZhuOlCorePendingDecisionPolicyPipelineOperations corePendingDecisionPolicyPipelineOperations, ICharacterXuanGongZhuOlCharacterXuanGongZhuOlOperations characterXuanGongZhuOlOperations, ICharacterXuanGongZhuOlCoreObserverSubtreesOperations coreObserverSubtreesOperations2, global::CardGame.Core.EngineStates.Contracts.ICoreCardMovementJournalCardMovementsAccess coreCardMovementJournalCardMovementsAccess)
    {
        this._resolutionStackOperations = resolutionStackOperations;
        this._coreDyingCursorRuntimeOperations = coreDyingCursorRuntimeOperations;
        this._coreAlternatingBenefitsOperations = coreAlternatingBenefitsOperations;
        this._coreIdentityConcealmentOperations = coreIdentityConcealmentOperations;
        this._coreObserverSubtreesOperations = coreObserverSubtreesOperations;
        this._corePrivateOfferReturnsOperations = corePrivateOfferReturnsOperations;
        this._coreQimeiProgramsOperations = coreQimeiProgramsOperations;
        this._coreTypedRecoveryDamageCursorOperations = coreTypedRecoveryDamageCursorOperations;
        this._coreZhuijiProgramsOperations = coreZhuijiProgramsOperations;
        this._corePendingDecisionPolicyPipelineOperations = corePendingDecisionPolicyPipelineOperations;
        this._characterXuanGongZhuOlOperations = characterXuanGongZhuOlOperations;
        this._coreObserverSubtreesOperations2 = coreObserverSubtreesOperations2;
        this._coreCardMovementJournalCardMovementsAccess = coreCardMovementJournalCardMovementsAccess;
    }
}
