#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
[global::CardGame.Core.ProgramRuntimeModule]
internal sealed partial class CoreGameEngineHeartSlashDamageRuntime
{
    private readonly ICoreHeartSlashDamageCharacterWuJingOlOperations _characterWuJingOlOperations;
    private readonly ICoreHeartSlashDamageResolutionStackOperations _resolutionStackOperations;
    private readonly ICoreHeartSlashDamageCoreCardAttackStateOperations _coreCardAttackStateOperations;
    private readonly ICoreHeartSlashDamageCorePlayCardHistoryOperations _corePlayCardHistoryOperations;
    private readonly global::CardGame.Core.EngineStates.Contracts.ICoreMatchParticipantsPlayersAccess _coreMatchParticipantsPlayersAccess;
    internal CoreGameEngineHeartSlashDamageRuntime(ICoreHeartSlashDamageCharacterWuJingOlOperations characterWuJingOlOperations, ICoreHeartSlashDamageResolutionStackOperations resolutionStackOperations, ICoreHeartSlashDamageCoreCardAttackStateOperations coreCardAttackStateOperations, ICoreHeartSlashDamageCorePlayCardHistoryOperations corePlayCardHistoryOperations, global::CardGame.Core.EngineStates.Contracts.ICoreMatchParticipantsPlayersAccess coreMatchParticipantsPlayersAccess)
    {
        this._characterWuJingOlOperations = characterWuJingOlOperations;
        this._resolutionStackOperations = resolutionStackOperations;
        this._coreCardAttackStateOperations = coreCardAttackStateOperations;
        this._corePlayCardHistoryOperations = corePlayCardHistoryOperations;
        this._coreMatchParticipantsPlayersAccess = coreMatchParticipantsPlayersAccess;
    }
}
