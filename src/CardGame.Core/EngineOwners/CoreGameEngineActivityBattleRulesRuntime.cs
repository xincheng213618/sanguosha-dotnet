#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
[global::CardGame.Core.ProgramRuntimeModule]
internal sealed partial class CoreGameEngineActivityBattleRulesRuntime
{
    private readonly ICoreActivityBattleRulesModeRuntimeOperations _modeRuntimeOperations;
    private readonly ICoreActivityBattleRulesCoreCardMovementProgramsOperations _coreCardMovementProgramsOperations;
    private readonly ICoreActivityBattleRulesCoreCharacterStateProgramsOperations _coreCharacterStateProgramsOperations;
    private readonly ICoreActivityBattleRulesCoreHpChangeProgramsOperations _coreHpChangeProgramsOperations;
    private readonly ICoreActivityBattleRulesCoreSkillOwnershipWindowsOperations _coreSkillOwnershipWindowsOperations;
    private readonly ICoreActivityBattleRulesCoreRecoveryReplacementOperations _coreRecoveryReplacementOperations;
    private readonly ICoreActivityBattleRulesKernelCoreGameEngineActivityBattleRulesOperations _kernelCoreGameEngineActivityBattleRulesOperations;
    internal CoreGameEngineActivityBattleRulesRuntime(ICoreActivityBattleRulesModeRuntimeOperations modeRuntimeOperations, ICoreActivityBattleRulesCoreCardMovementProgramsOperations coreCardMovementProgramsOperations, ICoreActivityBattleRulesCoreCharacterStateProgramsOperations coreCharacterStateProgramsOperations, ICoreActivityBattleRulesCoreHpChangeProgramsOperations coreHpChangeProgramsOperations, ICoreActivityBattleRulesCoreSkillOwnershipWindowsOperations coreSkillOwnershipWindowsOperations, ICoreActivityBattleRulesCoreRecoveryReplacementOperations coreRecoveryReplacementOperations, ICoreActivityBattleRulesKernelCoreGameEngineActivityBattleRulesOperations kernelCoreGameEngineActivityBattleRulesOperations)
    {
        this._modeRuntimeOperations = modeRuntimeOperations;
        this._coreCardMovementProgramsOperations = coreCardMovementProgramsOperations;
        this._coreCharacterStateProgramsOperations = coreCharacterStateProgramsOperations;
        this._coreHpChangeProgramsOperations = coreHpChangeProgramsOperations;
        this._coreSkillOwnershipWindowsOperations = coreSkillOwnershipWindowsOperations;
        this._coreRecoveryReplacementOperations = coreRecoveryReplacementOperations;
        this._kernelCoreGameEngineActivityBattleRulesOperations = kernelCoreGameEngineActivityBattleRulesOperations;
    }
}
