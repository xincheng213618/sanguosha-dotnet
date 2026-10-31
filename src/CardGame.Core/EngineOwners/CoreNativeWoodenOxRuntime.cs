#nullable enable

namespace CardGame.Core.EngineOwners;
[Core.ProgramRuntimeModule]
internal sealed partial class CoreNativeWoodenOxRuntime
{
    private readonly RuntimeEntries.CharacterHuCheErMouEntries _characterHuCheErMouOperations;
    private readonly RuntimeEntries.CoreMatchLogEntries _coreMatchLogOperations;
    private readonly RuntimeEntries.CorePhysicalMovementFacadesEntries _corePhysicalMovementFacadesOperations;
    private readonly RuntimeEntries.CorePhysicalOwnedCardQueriesEntries _corePhysicalOwnedCardQueriesOperations;
    private readonly EngineStates.Contracts.ICoreCurrentTurnStatisticsWoodenOxUsedThisTurnAccess _coreCurrentTurnStatisticsWoodenOxUsedThisTurnAccess;
    private readonly EngineStates.Contracts.ICoreMatchParticipantsPlayersAccess _coreMatchParticipantsPlayersAccess;
    internal CoreNativeWoodenOxRuntime(RuntimeEntries.CharacterHuCheErMouEntries characterHuCheErMouOperations, RuntimeEntries.CoreMatchLogEntries coreMatchLogOperations, RuntimeEntries.CorePhysicalMovementFacadesEntries corePhysicalMovementFacadesOperations, RuntimeEntries.CorePhysicalOwnedCardQueriesEntries corePhysicalOwnedCardQueriesOperations, EngineStates.Contracts.ICoreCurrentTurnStatisticsWoodenOxUsedThisTurnAccess coreCurrentTurnStatisticsWoodenOxUsedThisTurnAccess, EngineStates.Contracts.ICoreMatchParticipantsPlayersAccess coreMatchParticipantsPlayersAccess)
    {
        this._characterHuCheErMouOperations = characterHuCheErMouOperations;
        this._coreMatchLogOperations = coreMatchLogOperations;
        this._corePhysicalMovementFacadesOperations = corePhysicalMovementFacadesOperations;
        this._corePhysicalOwnedCardQueriesOperations = corePhysicalOwnedCardQueriesOperations;
        this._coreCurrentTurnStatisticsWoodenOxUsedThisTurnAccess = coreCurrentTurnStatisticsWoodenOxUsedThisTurnAccess;
        this._coreMatchParticipantsPlayersAccess = coreMatchParticipantsPlayersAccess;
    }
}
