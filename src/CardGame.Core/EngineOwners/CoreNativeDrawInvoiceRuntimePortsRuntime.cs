#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
[global::CardGame.Core.ProgramRuntimeModule, global::CardGame.Core.ProgramCapability]
internal sealed partial class CoreNativeDrawInvoiceRuntimePortsRuntime : global::CardGame.Core.INativeDrawInvoiceCompletions, global::CardGame.Core.INativeDrawInvoiceFacts
{
    private readonly global::CardGame.Core.RuntimeEntries.CharacterFeiYiOlEntries _characterFeiYiOlOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CharacterLiuZhangOlEntries _characterLiuZhangOlOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CharacterMiHengLeEntries _characterMiHengLeOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CharacterPangJiOlEntries _characterPangJiOlOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CharacterTangTangOlEntries _characterTangTangOlOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CharacterTaoQianOlEntries _characterTaoQianOlOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CharacterWangCanOlEntries _characterWangCanOlOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CharacterYuanJiOlEntries _characterYuanJiOlOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CharacterZhaoZhongOlEntries _characterZhaoZhongOlOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CharacterZhouFeiLeEntries _characterZhouFeiLeOperations;
    private readonly global::CardGame.Core.EngineOwners.IKernelMatchModeRulesAccess _modeRulesOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreHellKingProgramsEntries _coreHellKingProgramsOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreTableSeatQueriesEntries _coreTableSeatQueriesOperations;
    private readonly global::CardGame.Core.EngineStates.Contracts.ICoreMatchOutcomeStatusWinnerAccess _coreMatchOutcomeStatusWinnerAccess;
    private readonly global::CardGame.Core.EngineStates.Contracts.ICoreMatchSetupSetupCompleteAccess _coreMatchSetupSetupCompleteAccess;
    internal CoreNativeDrawInvoiceRuntimePortsRuntime(global::CardGame.Core.RuntimeEntries.CharacterFeiYiOlEntries characterFeiYiOlOperations, global::CardGame.Core.RuntimeEntries.CharacterLiuZhangOlEntries characterLiuZhangOlOperations, global::CardGame.Core.RuntimeEntries.CharacterMiHengLeEntries characterMiHengLeOperations, global::CardGame.Core.RuntimeEntries.CharacterPangJiOlEntries characterPangJiOlOperations, global::CardGame.Core.RuntimeEntries.CharacterTangTangOlEntries characterTangTangOlOperations, global::CardGame.Core.RuntimeEntries.CharacterTaoQianOlEntries characterTaoQianOlOperations, global::CardGame.Core.RuntimeEntries.CharacterWangCanOlEntries characterWangCanOlOperations, global::CardGame.Core.RuntimeEntries.CharacterYuanJiOlEntries characterYuanJiOlOperations, global::CardGame.Core.RuntimeEntries.CharacterZhaoZhongOlEntries characterZhaoZhongOlOperations, global::CardGame.Core.RuntimeEntries.CharacterZhouFeiLeEntries characterZhouFeiLeOperations, global::CardGame.Core.EngineOwners.IKernelMatchModeRulesAccess modeRulesOperations, global::CardGame.Core.RuntimeEntries.CoreHellKingProgramsEntries coreHellKingProgramsOperations, global::CardGame.Core.RuntimeEntries.CoreTableSeatQueriesEntries coreTableSeatQueriesOperations, global::CardGame.Core.EngineStates.Contracts.ICoreMatchOutcomeStatusWinnerAccess coreMatchOutcomeStatusWinnerAccess, global::CardGame.Core.EngineStates.Contracts.ICoreMatchSetupSetupCompleteAccess coreMatchSetupSetupCompleteAccess)
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
