#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
[global::CardGame.Core.ProgramRuntimeModule, global::CardGame.Core.ProgramCapability]
internal sealed partial class CoreNativeDrawInvoiceRuntimePortsRuntime : global::CardGame.Core.INativeDrawInvoiceCompletions, global::CardGame.Core.INativeDrawInvoiceFacts
{
    private readonly ICoreNativeDrawInvoiceRuntimePortsCharacterFeiYiOlOperations _characterFeiYiOlOperations;
    private readonly ICoreNativeDrawInvoiceRuntimePortsCharacterLiuZhangOlOperations _characterLiuZhangOlOperations;
    private readonly ICoreNativeDrawInvoiceRuntimePortsCharacterMiHengLeOperations _characterMiHengLeOperations;
    private readonly ICoreNativeDrawInvoiceRuntimePortsCharacterPangJiOlOperations _characterPangJiOlOperations;
    private readonly ICoreNativeDrawInvoiceRuntimePortsCharacterTangTangOlOperations _characterTangTangOlOperations;
    private readonly ICoreNativeDrawInvoiceRuntimePortsCharacterTaoQianOlOperations _characterTaoQianOlOperations;
    private readonly ICoreNativeDrawInvoiceRuntimePortsCharacterWangCanOlOperations _characterWangCanOlOperations;
    private readonly ICoreNativeDrawInvoiceRuntimePortsCharacterYuanJiOlOperations _characterYuanJiOlOperations;
    private readonly ICoreNativeDrawInvoiceRuntimePortsCharacterZhaoZhongOlOperations _characterZhaoZhongOlOperations;
    private readonly ICoreNativeDrawInvoiceRuntimePortsCharacterZhouFeiLeOperations _characterZhouFeiLeOperations;
    private readonly ICoreNativeDrawInvoiceRuntimePortsModeRulesOperations _modeRulesOperations;
    private readonly ICoreNativeDrawInvoiceRuntimePortsCoreHellKingProgramsOperations _coreHellKingProgramsOperations;
    private readonly ICoreNativeDrawInvoiceRuntimePortsCoreTableSeatQueriesOperations _coreTableSeatQueriesOperations;
    private readonly global::CardGame.Core.EngineStates.Contracts.ICoreMatchOutcomeStatusWinnerAccess _coreMatchOutcomeStatusWinnerAccess;
    private readonly global::CardGame.Core.EngineStates.Contracts.ICoreMatchSetupSetupCompleteAccess _coreMatchSetupSetupCompleteAccess;
    internal CoreNativeDrawInvoiceRuntimePortsRuntime(ICoreNativeDrawInvoiceRuntimePortsCharacterFeiYiOlOperations characterFeiYiOlOperations, ICoreNativeDrawInvoiceRuntimePortsCharacterLiuZhangOlOperations characterLiuZhangOlOperations, ICoreNativeDrawInvoiceRuntimePortsCharacterMiHengLeOperations characterMiHengLeOperations, ICoreNativeDrawInvoiceRuntimePortsCharacterPangJiOlOperations characterPangJiOlOperations, ICoreNativeDrawInvoiceRuntimePortsCharacterTangTangOlOperations characterTangTangOlOperations, ICoreNativeDrawInvoiceRuntimePortsCharacterTaoQianOlOperations characterTaoQianOlOperations, ICoreNativeDrawInvoiceRuntimePortsCharacterWangCanOlOperations characterWangCanOlOperations, ICoreNativeDrawInvoiceRuntimePortsCharacterYuanJiOlOperations characterYuanJiOlOperations, ICoreNativeDrawInvoiceRuntimePortsCharacterZhaoZhongOlOperations characterZhaoZhongOlOperations, ICoreNativeDrawInvoiceRuntimePortsCharacterZhouFeiLeOperations characterZhouFeiLeOperations, ICoreNativeDrawInvoiceRuntimePortsModeRulesOperations modeRulesOperations, ICoreNativeDrawInvoiceRuntimePortsCoreHellKingProgramsOperations coreHellKingProgramsOperations, ICoreNativeDrawInvoiceRuntimePortsCoreTableSeatQueriesOperations coreTableSeatQueriesOperations, global::CardGame.Core.EngineStates.Contracts.ICoreMatchOutcomeStatusWinnerAccess coreMatchOutcomeStatusWinnerAccess, global::CardGame.Core.EngineStates.Contracts.ICoreMatchSetupSetupCompleteAccess coreMatchSetupSetupCompleteAccess)
    {
        this._characterFeiYiOlOperations = characterFeiYiOlOperations;
        this._characterLiuZhangOlOperations = characterLiuZhangOlOperations;
        this._characterMiHengLeOperations = characterMiHengLeOperations;
        this._characterPangJiOlOperations = characterPangJiOlOperations;
        this._characterTangTangOlOperations = characterTangTangOlOperations;
        this._characterTaoQianOlOperations = characterTaoQianOlOperations;
        this._characterWangCanOlOperations = characterWangCanOlOperations;
        this._characterYuanJiOlOperations = characterYuanJiOlOperations;
        this._characterZhaoZhongOlOperations = characterZhaoZhongOlOperations;
        this._characterZhouFeiLeOperations = characterZhouFeiLeOperations;
        this._modeRulesOperations = modeRulesOperations;
        this._coreHellKingProgramsOperations = coreHellKingProgramsOperations;
        this._coreTableSeatQueriesOperations = coreTableSeatQueriesOperations;
        this._coreMatchOutcomeStatusWinnerAccess = coreMatchOutcomeStatusWinnerAccess;
        this._coreMatchSetupSetupCompleteAccess = coreMatchSetupSetupCompleteAccess;
    }
}
