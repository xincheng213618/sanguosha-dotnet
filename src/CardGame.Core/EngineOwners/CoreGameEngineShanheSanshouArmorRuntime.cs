#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
[global::CardGame.Core.ProgramRuntimeModule]
internal sealed partial class CoreGameEngineShanheSanshouArmorRuntime
{
    private readonly ICoreShanheSanshouArmorCharacterHuCheErMouOperations _characterHuCheErMouOperations;
    private readonly ICoreShanheSanshouArmorCoreDirectedCardPolicyQueriesOperations _coreDirectedCardPolicyQueriesOperations;
    private readonly ICoreShanheSanshouArmorCoreEquipmentSkillsOperations _coreEquipmentSkillsOperations;
    private readonly ICoreShanheSanshouArmorCoreExtendedViewAsOperations _coreExtendedViewAsOperations;
    private readonly ICoreShanheSanshouArmorCoreProgramAttackAttemptOperations _coreProgramAttackAttemptOperations;
    internal CoreGameEngineShanheSanshouArmorRuntime(ICoreShanheSanshouArmorCharacterHuCheErMouOperations characterHuCheErMouOperations, ICoreShanheSanshouArmorCoreDirectedCardPolicyQueriesOperations coreDirectedCardPolicyQueriesOperations, ICoreShanheSanshouArmorCoreEquipmentSkillsOperations coreEquipmentSkillsOperations, ICoreShanheSanshouArmorCoreExtendedViewAsOperations coreExtendedViewAsOperations, ICoreShanheSanshouArmorCoreProgramAttackAttemptOperations coreProgramAttackAttemptOperations)
    {
        this._characterHuCheErMouOperations = characterHuCheErMouOperations;
        this._coreDirectedCardPolicyQueriesOperations = coreDirectedCardPolicyQueriesOperations;
        this._coreEquipmentSkillsOperations = coreEquipmentSkillsOperations;
        this._coreExtendedViewAsOperations = coreExtendedViewAsOperations;
        this._coreProgramAttackAttemptOperations = coreProgramAttackAttemptOperations;
    }
}
