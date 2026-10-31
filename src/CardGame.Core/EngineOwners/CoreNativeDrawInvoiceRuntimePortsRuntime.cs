#nullable enable

namespace CardGame.Core.EngineOwners;
[Core.ProgramRuntimeModule, Core.ProgramCapability]
internal sealed partial class CoreNativeDrawInvoiceRuntimePortsRuntime : INativeDrawInvoiceCompletions, INativeDrawInvoiceFacts
{
    private readonly RuntimeEntries.CharacterFeiYiOlEntries _characterFeiYiOlOperations;
    private readonly RuntimeEntries.CharacterLiuZhangOlEntries _characterLiuZhangOlOperations;
    private readonly RuntimeEntries.CharacterMiHengLeEntries _characterMiHengLeOperations;
    private readonly RuntimeEntries.CharacterPangJiOlEntries _characterPangJiOlOperations;
    private readonly RuntimeEntries.CharacterTangTangOlEntries _characterTangTangOlOperations;
    private readonly RuntimeEntries.CharacterTaoQianOlEntries _characterTaoQianOlOperations;
    private readonly RuntimeEntries.CharacterWangCanOlEntries _characterWangCanOlOperations;
    private readonly RuntimeEntries.CharacterYuanJiOlEntries _characterYuanJiOlOperations;
    private readonly RuntimeEntries.CharacterZhaoZhongOlEntries _characterZhaoZhongOlOperations;
    private readonly RuntimeEntries.CharacterZhouFeiLeEntries _characterZhouFeiLeOperations;
    private readonly IKernelMatchModeRulesAccess _modeRulesOperations;
    private readonly RuntimeEntries.CoreHellKingProgramsEntries _coreHellKingProgramsOperations;
    private readonly RuntimeEntries.CoreTableSeatQueriesEntries _coreTableSeatQueriesOperations;
    private readonly EngineStates.Contracts.ICoreMatchOutcomeStatusWinnerAccess _coreMatchOutcomeStatusWinnerAccess;
    private readonly EngineStates.Contracts.ICoreMatchSetupSetupCompleteAccess _coreMatchSetupSetupCompleteAccess;
    internal CoreNativeDrawInvoiceRuntimePortsRuntime(RuntimeEntries.CharacterFeiYiOlEntries characterFeiYiOlOperations, RuntimeEntries.CharacterLiuZhangOlEntries characterLiuZhangOlOperations, RuntimeEntries.CharacterMiHengLeEntries characterMiHengLeOperations, RuntimeEntries.CharacterPangJiOlEntries characterPangJiOlOperations, RuntimeEntries.CharacterTangTangOlEntries characterTangTangOlOperations, RuntimeEntries.CharacterTaoQianOlEntries characterTaoQianOlOperations, RuntimeEntries.CharacterWangCanOlEntries characterWangCanOlOperations, RuntimeEntries.CharacterYuanJiOlEntries characterYuanJiOlOperations, RuntimeEntries.CharacterZhaoZhongOlEntries characterZhaoZhongOlOperations, RuntimeEntries.CharacterZhouFeiLeEntries characterZhouFeiLeOperations, IKernelMatchModeRulesAccess modeRulesOperations, RuntimeEntries.CoreHellKingProgramsEntries coreHellKingProgramsOperations, RuntimeEntries.CoreTableSeatQueriesEntries coreTableSeatQueriesOperations, EngineStates.Contracts.ICoreMatchOutcomeStatusWinnerAccess coreMatchOutcomeStatusWinnerAccess, EngineStates.Contracts.ICoreMatchSetupSetupCompleteAccess coreMatchSetupSetupCompleteAccess)
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
