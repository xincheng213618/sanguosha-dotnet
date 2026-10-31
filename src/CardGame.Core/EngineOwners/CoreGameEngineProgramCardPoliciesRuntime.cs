#nullable enable

namespace CardGame.Core.EngineOwners;
[Core.ProgramRuntimeModule]
internal sealed partial class CoreGameEngineProgramCardPoliciesRuntime
{
    private readonly RuntimeEntries.CharacterCaiWenJiLeEntries _characterCaiWenJiLeOperations;
    private readonly RuntimeEntries.CharacterCaoPiMouEntries _characterCaoPiMouOperations;
    private readonly RuntimeEntries.CharacterHanSongAnniversaryEntries _characterHanSongAnniversaryOperations;
    private readonly RuntimeEntries.CharacterHuangGaiMouMobileEntries _characterHuangGaiMouMobileOperations;
    private readonly RuntimeEntries.CharacterLuZhiMouOlEntries _characterLuZhiMouOlOperations;
    private readonly RuntimeEntries.CharacterLvYiMobileShiEntries _characterLvYiMobileShiOperations;
    private readonly RuntimeEntries.CharacterSunCeMobileGodEntries _characterSunCeMobileGodOperations;
    private readonly RuntimeEntries.CharacterSunShaoMobileShiEntries _characterSunShaoMobileShiOperations;
    private readonly RuntimeEntries.CharacterSunYuanOlEntries _characterSunYuanOlOperations;
    private readonly RuntimeEntries.CharacterXiahouBaMobileShiEntries _characterXiahouBaMobileShiOperations;
    private readonly RuntimeEntries.CharacterXiahouHuiAnniversaryEntries _characterXiahouHuiAnniversaryOperations;
    private readonly RuntimeEntries.CharacterXiaoQiaoBoundaryMobileEntries _characterXiaoQiaoBoundaryMobileOperations;
    private readonly RuntimeEntries.CharacterZhangFeiAnniversaryGodEntries _characterZhangFeiAnniversaryGodOperations;
    private readonly RuntimeEntries.CharacterZhongHuiMobileShiEntries _characterZhongHuiMobileShiOperations;
    private readonly RuntimeEntries.CoreGameEngineActionQueriesEntries _coreActionQueriesOperations;
    private readonly RuntimeEntries.CoreGameEngineLordSkillProjectionsEntries _coreLordSkillProjectionsOperations;
    private readonly RuntimeEntries.CoreGameEngineNationalSkillsEntries _coreNationalSkillsOperations;
    private readonly RuntimeEntries.CoreGameEngineProgramAlcoholQuotaEntries _coreProgramAlcoholQuotaOperations;
    private readonly RuntimeEntries.CoreGameEngineShanheS5QuyiEntries _coreShanheS5QuyiOperations;
    private readonly RuntimeEntries.CoreGameEngineShanheTrickResponseEntries _coreShanheTrickResponseOperations;
    private readonly RuntimeEntries.CoreGameEngineWushuangSkillsBAuditEntries _coreWushuangSkillsBAuditOperations;
    private readonly RuntimeEntries.CoreLeastHandMarkerDrawProgramsEntries _coreLeastHandMarkerDrawProgramsOperations;
    private readonly RuntimeEntries.CoreNativeCardAppearanceEntries _coreNativeCardAppearanceOperations;
    private readonly ICoreProgramCardPoliciesCoreNativeRuleQueriesOperations _coreNativeRuleQueriesOperations;
    private readonly RuntimeEntries.CoreNativeSkillContextQueriesEntries _coreNativeSkillContextQueriesOperations;
    private readonly RuntimeEntries.CoreShanheBossProgramsEntries _coreShanheBossProgramsOperations;
    private readonly EngineStates.Contracts.ICoreMatchParticipantsPlayersAccess _coreMatchParticipantsPlayersAccess;
    internal CoreGameEngineProgramCardPoliciesRuntime(RuntimeEntries.CharacterCaiWenJiLeEntries characterCaiWenJiLeOperations, RuntimeEntries.CharacterCaoPiMouEntries characterCaoPiMouOperations, RuntimeEntries.CharacterHanSongAnniversaryEntries characterHanSongAnniversaryOperations, RuntimeEntries.CharacterHuangGaiMouMobileEntries characterHuangGaiMouMobileOperations, RuntimeEntries.CharacterLuZhiMouOlEntries characterLuZhiMouOlOperations, RuntimeEntries.CharacterLvYiMobileShiEntries characterLvYiMobileShiOperations, RuntimeEntries.CharacterSunCeMobileGodEntries characterSunCeMobileGodOperations, RuntimeEntries.CharacterSunShaoMobileShiEntries characterSunShaoMobileShiOperations, RuntimeEntries.CharacterSunYuanOlEntries characterSunYuanOlOperations, RuntimeEntries.CharacterXiahouBaMobileShiEntries characterXiahouBaMobileShiOperations, RuntimeEntries.CharacterXiahouHuiAnniversaryEntries characterXiahouHuiAnniversaryOperations, RuntimeEntries.CharacterXiaoQiaoBoundaryMobileEntries characterXiaoQiaoBoundaryMobileOperations, RuntimeEntries.CharacterZhangFeiAnniversaryGodEntries characterZhangFeiAnniversaryGodOperations, RuntimeEntries.CharacterZhongHuiMobileShiEntries characterZhongHuiMobileShiOperations, RuntimeEntries.CoreGameEngineActionQueriesEntries coreActionQueriesOperations, RuntimeEntries.CoreGameEngineLordSkillProjectionsEntries coreLordSkillProjectionsOperations, RuntimeEntries.CoreGameEngineNationalSkillsEntries coreNationalSkillsOperations, RuntimeEntries.CoreGameEngineProgramAlcoholQuotaEntries coreProgramAlcoholQuotaOperations, RuntimeEntries.CoreGameEngineShanheS5QuyiEntries coreShanheS5QuyiOperations, RuntimeEntries.CoreGameEngineShanheTrickResponseEntries coreShanheTrickResponseOperations, RuntimeEntries.CoreGameEngineWushuangSkillsBAuditEntries coreWushuangSkillsBAuditOperations, RuntimeEntries.CoreLeastHandMarkerDrawProgramsEntries coreLeastHandMarkerDrawProgramsOperations, RuntimeEntries.CoreNativeCardAppearanceEntries coreNativeCardAppearanceOperations, ICoreProgramCardPoliciesCoreNativeRuleQueriesOperations coreNativeRuleQueriesOperations, RuntimeEntries.CoreNativeSkillContextQueriesEntries coreNativeSkillContextQueriesOperations, RuntimeEntries.CoreShanheBossProgramsEntries coreShanheBossProgramsOperations, EngineStates.Contracts.ICoreMatchParticipantsPlayersAccess coreMatchParticipantsPlayersAccess)
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
