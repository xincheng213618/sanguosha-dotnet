#nullable enable

namespace CardGame.Core.EngineOwners;
[Core.ProgramRuntimeModule, Core.ProgramCapability]
internal sealed partial class CoreProgramJudgmentRuntimePortsRuntime : IProgramJudgmentNativeProofs, IProgramJudgmentProducer
{
    private readonly RuntimeEntries.CharacterGuoJiaMobileGodEntries _characterGuoJiaMobileGodOperations;
    private readonly RuntimeEntries.CharacterGuoJiaMouEntries _characterGuoJiaMouOperations;
    private readonly RuntimeEntries.CharacterMaYuanYiOlEntries _characterMaYuanYiOlOperations;
    private readonly RuntimeEntries.CharacterQianZhaoOlEntries _characterQianZhaoOlOperations;
    private readonly RuntimeEntries.CharacterZhangFeiAnniversaryGodEntries _characterZhangFeiAnniversaryGodOperations;
    private readonly RuntimeEntries.CharacterZhangJiOlEntries _characterZhangJiOlOperations;
    private readonly RuntimeEntries.CharacterZhongHuiAnniversaryGodEntries _characterZhongHuiAnniversaryGodOperations;
    private readonly RuntimeEntries.CharacterZhouQunOlEntries _characterZhouQunOlOperations;
    private readonly RuntimeEntries.CoreActiveNativeAttackQueriesEntries _coreActiveNativeAttackQueriesOperations;
    private readonly RuntimeEntries.CoreGameEngineDamageJudgmentSuitPaymentsEntries _coreDamageJudgmentSuitPaymentsOperations;
    private readonly RuntimeEntries.CoreGameEngineProgramCardPaymentsEntries _coreProgramCardPaymentsOperations;
    private readonly RuntimeEntries.CoreGameEngineSourceHuojieEntries _coreSourceHuojieOperations;
    private readonly RuntimeEntries.CoreNativeJudgmentEntries _coreNativeJudgmentOperations;
    private readonly RuntimeEntries.CoreWushuangCombatProgramsEntries _coreWushuangCombatProgramsOperations;
    internal CoreProgramJudgmentRuntimePortsRuntime(RuntimeEntries.CharacterGuoJiaMobileGodEntries characterGuoJiaMobileGodOperations, RuntimeEntries.CharacterGuoJiaMouEntries characterGuoJiaMouOperations, RuntimeEntries.CharacterMaYuanYiOlEntries characterMaYuanYiOlOperations, RuntimeEntries.CharacterQianZhaoOlEntries characterQianZhaoOlOperations, RuntimeEntries.CharacterZhangFeiAnniversaryGodEntries characterZhangFeiAnniversaryGodOperations, RuntimeEntries.CharacterZhangJiOlEntries characterZhangJiOlOperations, RuntimeEntries.CharacterZhongHuiAnniversaryGodEntries characterZhongHuiAnniversaryGodOperations, RuntimeEntries.CharacterZhouQunOlEntries characterZhouQunOlOperations, RuntimeEntries.CoreActiveNativeAttackQueriesEntries coreActiveNativeAttackQueriesOperations, RuntimeEntries.CoreGameEngineDamageJudgmentSuitPaymentsEntries coreDamageJudgmentSuitPaymentsOperations, RuntimeEntries.CoreGameEngineProgramCardPaymentsEntries coreProgramCardPaymentsOperations, RuntimeEntries.CoreGameEngineSourceHuojieEntries coreSourceHuojieOperations, RuntimeEntries.CoreNativeJudgmentEntries coreNativeJudgmentOperations, RuntimeEntries.CoreWushuangCombatProgramsEntries coreWushuangCombatProgramsOperations)
    {
        this._characterGuoJiaMobileGodOperations = characterGuoJiaMobileGodOperations;
        this._characterGuoJiaMouOperations = characterGuoJiaMouOperations;
        this._characterMaYuanYiOlOperations = characterMaYuanYiOlOperations;
        this._characterQianZhaoOlOperations = characterQianZhaoOlOperations;
        this._characterZhangFeiAnniversaryGodOperations = characterZhangFeiAnniversaryGodOperations;
        this._characterZhangJiOlOperations = characterZhangJiOlOperations;
        this._characterZhongHuiAnniversaryGodOperations = characterZhongHuiAnniversaryGodOperations;
        this._characterZhouQunOlOperations = characterZhouQunOlOperations;
        this._coreActiveNativeAttackQueriesOperations = coreActiveNativeAttackQueriesOperations;
        this._coreDamageJudgmentSuitPaymentsOperations = coreDamageJudgmentSuitPaymentsOperations;
        this._coreProgramCardPaymentsOperations = coreProgramCardPaymentsOperations;
        this._coreSourceHuojieOperations = coreSourceHuojieOperations;
        this._coreNativeJudgmentOperations = coreNativeJudgmentOperations;
        this._coreWushuangCombatProgramsOperations = coreWushuangCombatProgramsOperations;
    }
}
