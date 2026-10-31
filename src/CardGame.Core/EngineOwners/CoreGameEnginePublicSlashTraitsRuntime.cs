#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
[global::CardGame.Core.ProgramRuntimeModule]
internal sealed partial class CoreGameEnginePublicSlashTraitsRuntime
{
    private readonly ICorePublicSlashTraitsCharacterMaChaoAnniversaryGodOperations _characterMaChaoAnniversaryGodOperations;
    private readonly ICorePublicSlashTraitsModeRuntimeOperations _modeRuntimeOperations;
    private readonly ICorePublicSlashTraitsCoreAdvancedWeaponsOperations _coreAdvancedWeaponsOperations;
    private readonly ICorePublicSlashTraitsCoreDirectedCardPolicyQueriesOperations _coreDirectedCardPolicyQueriesOperations;
    private readonly ICorePublicSlashTraitsCoreNationalSkillsOperations _coreNationalSkillsOperations;
    private readonly ICorePublicSlashTraitsCoreProgramCardPoliciesOperations _coreProgramCardPoliciesOperations;
    private readonly ICorePublicSlashTraitsCoreNativeEquipmentAndFactionQueriesOperations _coreNativeEquipmentAndFactionQueriesOperations;
    private readonly ICorePublicSlashTraitsCoreShenwuNpcProgramsOperations _coreShenwuNpcProgramsOperations;
    private readonly global::CardGame.Core.EngineStates.Contracts.ICoreMatchParticipantsPlayersAccess _coreMatchParticipantsPlayersAccess;
    internal CoreGameEnginePublicSlashTraitsRuntime(ICorePublicSlashTraitsCharacterMaChaoAnniversaryGodOperations characterMaChaoAnniversaryGodOperations, ICorePublicSlashTraitsModeRuntimeOperations modeRuntimeOperations, ICorePublicSlashTraitsCoreAdvancedWeaponsOperations coreAdvancedWeaponsOperations, ICorePublicSlashTraitsCoreDirectedCardPolicyQueriesOperations coreDirectedCardPolicyQueriesOperations, ICorePublicSlashTraitsCoreNationalSkillsOperations coreNationalSkillsOperations, ICorePublicSlashTraitsCoreProgramCardPoliciesOperations coreProgramCardPoliciesOperations, ICorePublicSlashTraitsCoreNativeEquipmentAndFactionQueriesOperations coreNativeEquipmentAndFactionQueriesOperations, ICorePublicSlashTraitsCoreShenwuNpcProgramsOperations coreShenwuNpcProgramsOperations, global::CardGame.Core.EngineStates.Contracts.ICoreMatchParticipantsPlayersAccess coreMatchParticipantsPlayersAccess)
    {
        this._characterMaChaoAnniversaryGodOperations = characterMaChaoAnniversaryGodOperations;
        this._modeRuntimeOperations = modeRuntimeOperations;
        this._coreAdvancedWeaponsOperations = coreAdvancedWeaponsOperations;
        this._coreDirectedCardPolicyQueriesOperations = coreDirectedCardPolicyQueriesOperations;
        this._coreNationalSkillsOperations = coreNationalSkillsOperations;
        this._coreProgramCardPoliciesOperations = coreProgramCardPoliciesOperations;
        this._coreNativeEquipmentAndFactionQueriesOperations = coreNativeEquipmentAndFactionQueriesOperations;
        this._coreShenwuNpcProgramsOperations = coreShenwuNpcProgramsOperations;
        this._coreMatchParticipantsPlayersAccess = coreMatchParticipantsPlayersAccess;
    }
}
