#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
[global::CardGame.Core.ProgramRuntimeModule]
internal sealed partial class CoreGameEngineProgramCardPoliciesRuntime
{
    private readonly global::CardGame.Core.RuntimeEntries.CharacterCaiWenJiLeEntries _characterCaiWenJiLeOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CharacterCaoPiMouEntries _characterCaoPiMouOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CharacterHanSongAnniversaryEntries _characterHanSongAnniversaryOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CharacterHuangGaiMouMobileEntries _characterHuangGaiMouMobileOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CharacterLuZhiMouOlEntries _characterLuZhiMouOlOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CharacterLvYiMobileShiEntries _characterLvYiMobileShiOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CharacterSunCeMobileGodEntries _characterSunCeMobileGodOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CharacterSunShaoMobileShiEntries _characterSunShaoMobileShiOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CharacterSunYuanOlEntries _characterSunYuanOlOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CharacterXiahouBaMobileShiEntries _characterXiahouBaMobileShiOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CharacterXiahouHuiAnniversaryEntries _characterXiahouHuiAnniversaryOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CharacterXiaoQiaoBoundaryMobileEntries _characterXiaoQiaoBoundaryMobileOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CharacterZhangFeiAnniversaryGodEntries _characterZhangFeiAnniversaryGodOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CharacterZhongHuiMobileShiEntries _characterZhongHuiMobileShiOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineActionQueriesEntries _coreActionQueriesOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineLordSkillProjectionsEntries _coreLordSkillProjectionsOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineNationalSkillsEntries _coreNationalSkillsOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineProgramAlcoholQuotaEntries _coreProgramAlcoholQuotaOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineShanheS5QuyiEntries _coreShanheS5QuyiOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineShanheTrickResponseEntries _coreShanheTrickResponseOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineWushuangSkillsBAuditEntries _coreWushuangSkillsBAuditOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreLeastHandMarkerDrawProgramsEntries _coreLeastHandMarkerDrawProgramsOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreNativeCardAppearanceEntries _coreNativeCardAppearanceOperations;
    private readonly ICoreProgramCardPoliciesCoreNativeRuleQueriesOperations _coreNativeRuleQueriesOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreNativeSkillContextQueriesEntries _coreNativeSkillContextQueriesOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreShanheBossProgramsEntries _coreShanheBossProgramsOperations;
    private readonly global::CardGame.Core.EngineStates.Contracts.ICoreMatchParticipantsPlayersAccess _coreMatchParticipantsPlayersAccess;
    internal CoreGameEngineProgramCardPoliciesRuntime(global::CardGame.Core.RuntimeEntries.CharacterCaiWenJiLeEntries characterCaiWenJiLeOperations, global::CardGame.Core.RuntimeEntries.CharacterCaoPiMouEntries characterCaoPiMouOperations, global::CardGame.Core.RuntimeEntries.CharacterHanSongAnniversaryEntries characterHanSongAnniversaryOperations, global::CardGame.Core.RuntimeEntries.CharacterHuangGaiMouMobileEntries characterHuangGaiMouMobileOperations, global::CardGame.Core.RuntimeEntries.CharacterLuZhiMouOlEntries characterLuZhiMouOlOperations, global::CardGame.Core.RuntimeEntries.CharacterLvYiMobileShiEntries characterLvYiMobileShiOperations, global::CardGame.Core.RuntimeEntries.CharacterSunCeMobileGodEntries characterSunCeMobileGodOperations, global::CardGame.Core.RuntimeEntries.CharacterSunShaoMobileShiEntries characterSunShaoMobileShiOperations, global::CardGame.Core.RuntimeEntries.CharacterSunYuanOlEntries characterSunYuanOlOperations, global::CardGame.Core.RuntimeEntries.CharacterXiahouBaMobileShiEntries characterXiahouBaMobileShiOperations, global::CardGame.Core.RuntimeEntries.CharacterXiahouHuiAnniversaryEntries characterXiahouHuiAnniversaryOperations, global::CardGame.Core.RuntimeEntries.CharacterXiaoQiaoBoundaryMobileEntries characterXiaoQiaoBoundaryMobileOperations, global::CardGame.Core.RuntimeEntries.CharacterZhangFeiAnniversaryGodEntries characterZhangFeiAnniversaryGodOperations, global::CardGame.Core.RuntimeEntries.CharacterZhongHuiMobileShiEntries characterZhongHuiMobileShiOperations, global::CardGame.Core.RuntimeEntries.CoreGameEngineActionQueriesEntries coreActionQueriesOperations, global::CardGame.Core.RuntimeEntries.CoreGameEngineLordSkillProjectionsEntries coreLordSkillProjectionsOperations, global::CardGame.Core.RuntimeEntries.CoreGameEngineNationalSkillsEntries coreNationalSkillsOperations, global::CardGame.Core.RuntimeEntries.CoreGameEngineProgramAlcoholQuotaEntries coreProgramAlcoholQuotaOperations, global::CardGame.Core.RuntimeEntries.CoreGameEngineShanheS5QuyiEntries coreShanheS5QuyiOperations, global::CardGame.Core.RuntimeEntries.CoreGameEngineShanheTrickResponseEntries coreShanheTrickResponseOperations, global::CardGame.Core.RuntimeEntries.CoreGameEngineWushuangSkillsBAuditEntries coreWushuangSkillsBAuditOperations, global::CardGame.Core.RuntimeEntries.CoreLeastHandMarkerDrawProgramsEntries coreLeastHandMarkerDrawProgramsOperations, global::CardGame.Core.RuntimeEntries.CoreNativeCardAppearanceEntries coreNativeCardAppearanceOperations, ICoreProgramCardPoliciesCoreNativeRuleQueriesOperations coreNativeRuleQueriesOperations, global::CardGame.Core.RuntimeEntries.CoreNativeSkillContextQueriesEntries coreNativeSkillContextQueriesOperations, global::CardGame.Core.RuntimeEntries.CoreShanheBossProgramsEntries coreShanheBossProgramsOperations, global::CardGame.Core.EngineStates.Contracts.ICoreMatchParticipantsPlayersAccess coreMatchParticipantsPlayersAccess)
    {
        this._characterCaiWenJiLeOperations = characterCaiWenJiLeOperations;
        this._characterCaoPiMouOperations = characterCaoPiMouOperations;
        this._characterHanSongAnniversaryOperations = characterHanSongAnniversaryOperations;
        this._characterHuangGaiMouMobileOperations = characterHuangGaiMouMobileOperations;
        this._characterLuZhiMouOlOperations = characterLuZhiMouOlOperations;
        this._characterLvYiMobileShiOperations = characterLvYiMobileShiOperations;
        this._characterSunCeMobileGodOperations = characterSunCeMobileGodOperations;
        this._characterSunShaoMobileShiOperations = characterSunShaoMobileShiOperations;
        this._characterSunYuanOlOperations = characterSunYuanOlOperations;
        this._characterXiahouBaMobileShiOperations = characterXiahouBaMobileShiOperations;
        this._characterXiahouHuiAnniversaryOperations = characterXiahouHuiAnniversaryOperations;
        this._characterXiaoQiaoBoundaryMobileOperations = characterXiaoQiaoBoundaryMobileOperations;
        this._characterZhangFeiAnniversaryGodOperations = characterZhangFeiAnniversaryGodOperations;
        this._characterZhongHuiMobileShiOperations = characterZhongHuiMobileShiOperations;
        this._coreActionQueriesOperations = coreActionQueriesOperations;
        this._coreLordSkillProjectionsOperations = coreLordSkillProjectionsOperations;
        this._coreNationalSkillsOperations = coreNationalSkillsOperations;
        this._coreProgramAlcoholQuotaOperations = coreProgramAlcoholQuotaOperations;
        this._coreShanheS5QuyiOperations = coreShanheS5QuyiOperations;
        this._coreShanheTrickResponseOperations = coreShanheTrickResponseOperations;
        this._coreWushuangSkillsBAuditOperations = coreWushuangSkillsBAuditOperations;
        this._coreLeastHandMarkerDrawProgramsOperations = coreLeastHandMarkerDrawProgramsOperations;
        this._coreNativeCardAppearanceOperations = coreNativeCardAppearanceOperations;
        this._coreNativeRuleQueriesOperations = coreNativeRuleQueriesOperations;
        this._coreNativeSkillContextQueriesOperations = coreNativeSkillContextQueriesOperations;
        this._coreShanheBossProgramsOperations = coreShanheBossProgramsOperations;
        this._coreMatchParticipantsPlayersAccess = coreMatchParticipantsPlayersAccess;
    }
}
