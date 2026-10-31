#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
[global::CardGame.Core.ProgramRuntimeModule, global::CardGame.Core.ProgramCapability]
internal sealed partial class CoreProgramJudgmentRuntimePortsRuntime : global::CardGame.Core.IProgramJudgmentNativeProofs, global::CardGame.Core.IProgramJudgmentProducer
{
    private readonly global::CardGame.Core.RuntimeEntries.CharacterGuoJiaMobileGodEntries _characterGuoJiaMobileGodOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CharacterGuoJiaMouEntries _characterGuoJiaMouOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CharacterMaYuanYiOlEntries _characterMaYuanYiOlOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CharacterQianZhaoOlEntries _characterQianZhaoOlOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CharacterZhangFeiAnniversaryGodEntries _characterZhangFeiAnniversaryGodOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CharacterZhangJiOlEntries _characterZhangJiOlOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CharacterZhongHuiAnniversaryGodEntries _characterZhongHuiAnniversaryGodOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CharacterZhouQunOlEntries _characterZhouQunOlOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreActiveNativeAttackQueriesEntries _coreActiveNativeAttackQueriesOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineDamageJudgmentSuitPaymentsEntries _coreDamageJudgmentSuitPaymentsOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineProgramCardPaymentsEntries _coreProgramCardPaymentsOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineSourceHuojieEntries _coreSourceHuojieOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreNativeJudgmentEntries _coreNativeJudgmentOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreWushuangCombatProgramsEntries _coreWushuangCombatProgramsOperations;
    internal CoreProgramJudgmentRuntimePortsRuntime(global::CardGame.Core.RuntimeEntries.CharacterGuoJiaMobileGodEntries characterGuoJiaMobileGodOperations, global::CardGame.Core.RuntimeEntries.CharacterGuoJiaMouEntries characterGuoJiaMouOperations, global::CardGame.Core.RuntimeEntries.CharacterMaYuanYiOlEntries characterMaYuanYiOlOperations, global::CardGame.Core.RuntimeEntries.CharacterQianZhaoOlEntries characterQianZhaoOlOperations, global::CardGame.Core.RuntimeEntries.CharacterZhangFeiAnniversaryGodEntries characterZhangFeiAnniversaryGodOperations, global::CardGame.Core.RuntimeEntries.CharacterZhangJiOlEntries characterZhangJiOlOperations, global::CardGame.Core.RuntimeEntries.CharacterZhongHuiAnniversaryGodEntries characterZhongHuiAnniversaryGodOperations, global::CardGame.Core.RuntimeEntries.CharacterZhouQunOlEntries characterZhouQunOlOperations, global::CardGame.Core.RuntimeEntries.CoreActiveNativeAttackQueriesEntries coreActiveNativeAttackQueriesOperations, global::CardGame.Core.RuntimeEntries.CoreGameEngineDamageJudgmentSuitPaymentsEntries coreDamageJudgmentSuitPaymentsOperations, global::CardGame.Core.RuntimeEntries.CoreGameEngineProgramCardPaymentsEntries coreProgramCardPaymentsOperations, global::CardGame.Core.RuntimeEntries.CoreGameEngineSourceHuojieEntries coreSourceHuojieOperations, global::CardGame.Core.RuntimeEntries.CoreNativeJudgmentEntries coreNativeJudgmentOperations, global::CardGame.Core.RuntimeEntries.CoreWushuangCombatProgramsEntries coreWushuangCombatProgramsOperations)
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
