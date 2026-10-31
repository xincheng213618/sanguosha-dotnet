#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
[global::CardGame.Core.ProgramRuntimeModule, global::CardGame.Core.ProgramCapability]
internal sealed partial class CoreGameEngineProgramCategoryTargetLedgerRuntime : global::CardGame.Core.IControlProgramEffectHost
{
    private readonly ICoreProgramCategoryTargetLedgerCardZonesOperations _cardZonesOperations;
    private readonly ICoreProgramCategoryTargetLedgerSkillRuntimeStateOperations _skillRuntimeStateOperations;
    private readonly ICoreProgramCategoryTargetLedgerCoreProgramCardUseRolesOperations _coreProgramCardUseRolesOperations;
    private readonly ICoreProgramCategoryTargetLedgerCoreProgramDamageReductionOperations _coreProgramDamageReductionOperations;
    private readonly ICoreProgramCategoryTargetLedgerCoreProgramLifecycleOperations _coreProgramLifecycleOperations;
    private readonly ICoreProgramCategoryTargetLedgerKernelCoreGameEngineRuleProgressionOperations _kernelCoreGameEngineRuleProgressionOperations;
    private readonly global::CardGame.Core.EngineStates.Contracts.ICoreMatchParticipantsPlayersAccess _coreMatchParticipantsPlayersAccess;
    internal CoreGameEngineProgramCategoryTargetLedgerRuntime(ICoreProgramCategoryTargetLedgerCardZonesOperations cardZonesOperations, ICoreProgramCategoryTargetLedgerSkillRuntimeStateOperations skillRuntimeStateOperations, ICoreProgramCategoryTargetLedgerCoreProgramCardUseRolesOperations coreProgramCardUseRolesOperations, ICoreProgramCategoryTargetLedgerCoreProgramDamageReductionOperations coreProgramDamageReductionOperations, ICoreProgramCategoryTargetLedgerCoreProgramLifecycleOperations coreProgramLifecycleOperations, ICoreProgramCategoryTargetLedgerKernelCoreGameEngineRuleProgressionOperations kernelCoreGameEngineRuleProgressionOperations, global::CardGame.Core.EngineStates.Contracts.ICoreMatchParticipantsPlayersAccess coreMatchParticipantsPlayersAccess)
    {
        this._cardZonesOperations = cardZonesOperations;
        this._skillRuntimeStateOperations = skillRuntimeStateOperations;
        this._coreProgramCardUseRolesOperations = coreProgramCardUseRolesOperations;
        this._coreProgramDamageReductionOperations = coreProgramDamageReductionOperations;
        this._coreProgramLifecycleOperations = coreProgramLifecycleOperations;
        this._kernelCoreGameEngineRuleProgressionOperations = kernelCoreGameEngineRuleProgressionOperations;
        this._coreMatchParticipantsPlayersAccess = coreMatchParticipantsPlayersAccess;
    }
}
