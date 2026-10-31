#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
[global::CardGame.Core.ProgramRuntimeModule, global::CardGame.Core.ProgramCapability]
internal sealed partial class CharacterLiDianMobileRuntime : global::CardGame.Core.IMobileLiDianWangxiHost
{
    private readonly ICharacterLiDianMobileContentRegistryOperations _contentRegistryOperations;
    private readonly ICharacterLiDianMobileResolutionStackOperations _resolutionStackOperations;
    private readonly ICharacterLiDianMobileCoreCardMovementProgramsOperations _coreCardMovementProgramsOperations;
    private readonly ICharacterLiDianMobileCoreCharacterStateProgramsOperations _coreCharacterStateProgramsOperations;
    private readonly ICharacterLiDianMobileCoreHpChangeProgramsOperations _coreHpChangeProgramsOperations;
    private readonly ICharacterLiDianMobileCoreProgramLifecycleOperations _coreProgramLifecycleOperations;
    private readonly ICharacterLiDianMobileCoreProgramNativeDrawOperations _coreProgramNativeDrawOperations;
    private readonly ICharacterLiDianMobileCoreRecoveryReplacementDamageRidesOperations _coreRecoveryReplacementDamageRidesOperations;
    private readonly ICharacterLiDianMobileCoreRuntimeOperations _coreRuntimeOperations;
    private readonly ICharacterLiDianMobileCoreSkillOwnershipWindowsOperations _coreSkillOwnershipWindowsOperations;
    private readonly ICharacterLiDianMobileCoreTableOrderOperations _coreTableOrderOperations;
    private readonly ICharacterLiDianMobileCoreRecoveryReplacementOperations _coreRecoveryReplacementOperations;
    private readonly ICharacterLiDianMobileCoreTableSeatQueriesOperations _coreTableSeatQueriesOperations;
    private readonly global::CardGame.Core.EngineStates.Contracts.ICoreActualTurnProgressCurrentSeatTurnNumberAccess _coreActualTurnProgressCurrentSeatTurnNumberAccess;
    private readonly global::CardGame.Core.EngineStates.Contracts.ICoreCardMovementJournalCardMovementsAccess _coreCardMovementJournalCardMovementsAccess;
    private readonly global::CardGame.Core.EngineStates.Contracts.ICoreMatchParticipantsPlayersAccess _coreMatchParticipantsPlayersAccess;
    internal CharacterLiDianMobileRuntime(ICharacterLiDianMobileContentRegistryOperations contentRegistryOperations, ICharacterLiDianMobileResolutionStackOperations resolutionStackOperations, ICharacterLiDianMobileCoreCardMovementProgramsOperations coreCardMovementProgramsOperations, ICharacterLiDianMobileCoreCharacterStateProgramsOperations coreCharacterStateProgramsOperations, ICharacterLiDianMobileCoreHpChangeProgramsOperations coreHpChangeProgramsOperations, ICharacterLiDianMobileCoreProgramLifecycleOperations coreProgramLifecycleOperations, ICharacterLiDianMobileCoreProgramNativeDrawOperations coreProgramNativeDrawOperations, ICharacterLiDianMobileCoreRecoveryReplacementDamageRidesOperations coreRecoveryReplacementDamageRidesOperations, ICharacterLiDianMobileCoreRuntimeOperations coreRuntimeOperations, ICharacterLiDianMobileCoreSkillOwnershipWindowsOperations coreSkillOwnershipWindowsOperations, ICharacterLiDianMobileCoreTableOrderOperations coreTableOrderOperations, ICharacterLiDianMobileCoreRecoveryReplacementOperations coreRecoveryReplacementOperations, ICharacterLiDianMobileCoreTableSeatQueriesOperations coreTableSeatQueriesOperations, global::CardGame.Core.EngineStates.Contracts.ICoreActualTurnProgressCurrentSeatTurnNumberAccess coreActualTurnProgressCurrentSeatTurnNumberAccess, global::CardGame.Core.EngineStates.Contracts.ICoreCardMovementJournalCardMovementsAccess coreCardMovementJournalCardMovementsAccess, global::CardGame.Core.EngineStates.Contracts.ICoreMatchParticipantsPlayersAccess coreMatchParticipantsPlayersAccess)
    {
        this._contentRegistryOperations = contentRegistryOperations;
        this._resolutionStackOperations = resolutionStackOperations;
        this._coreCardMovementProgramsOperations = coreCardMovementProgramsOperations;
        this._coreCharacterStateProgramsOperations = coreCharacterStateProgramsOperations;
        this._coreHpChangeProgramsOperations = coreHpChangeProgramsOperations;
        this._coreProgramLifecycleOperations = coreProgramLifecycleOperations;
        this._coreProgramNativeDrawOperations = coreProgramNativeDrawOperations;
        this._coreRecoveryReplacementDamageRidesOperations = coreRecoveryReplacementDamageRidesOperations;
        this._coreRuntimeOperations = coreRuntimeOperations;
        this._coreSkillOwnershipWindowsOperations = coreSkillOwnershipWindowsOperations;
        this._coreTableOrderOperations = coreTableOrderOperations;
        this._coreRecoveryReplacementOperations = coreRecoveryReplacementOperations;
        this._coreTableSeatQueriesOperations = coreTableSeatQueriesOperations;
        this._coreActualTurnProgressCurrentSeatTurnNumberAccess = coreActualTurnProgressCurrentSeatTurnNumberAccess;
        this._coreCardMovementJournalCardMovementsAccess = coreCardMovementJournalCardMovementsAccess;
        this._coreMatchParticipantsPlayersAccess = coreMatchParticipantsPlayersAccess;
    }
}
