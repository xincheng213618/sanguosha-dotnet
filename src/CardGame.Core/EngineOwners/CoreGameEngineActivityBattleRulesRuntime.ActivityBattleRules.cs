#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CoreGameEngineActivityBattleRulesRuntime
{
    internal IWenheBattleRulesFeature? WenheRules => _modeRuntimeOperations._modeRuntime.Find<IWenheBattleRulesFeature>();
    internal IGuanduBattleRulesFeature? GuanduRules => _modeRuntimeOperations._modeRuntime.Find<IGuanduBattleRulesFeature>();
    internal IDragonBoatBattleRulesFeature? DragonBoatRules => _modeRuntimeOperations._modeRuntime.Find<IDragonBoatBattleRulesFeature>();

    internal IModeBattleRulesHost CreateActivityBattleRulesHost() => _kernelCoreGameEngineActivityBattleRulesOperations.CreateActivityBattleRulesHost();
    internal IWenheRuleFlowHost CreateWenheRuleFlowHost() => _kernelCoreGameEngineActivityBattleRulesOperations.CreateWenheRuleFlowHost();
    internal IGuanduEventFlowHost CreateGuanduEventFlowHost() => _kernelCoreGameEngineActivityBattleRulesOperations.CreateGuanduEventFlowHost();
    internal bool DrainActivityRuleChildren(long frameId) => _coreRecoveryReplacementOperations.TryBeginQueuedRecoveryReplacement(frameId, PostEventContinuation.Program) || _coreCharacterStateProgramsOperations.TryBeginCharacterStateProgramWindow(frameId, CharacterStateContinuation.Program) || _coreHpChangeProgramsOperations.TryBeginHpChangedProgramWindow(frameId, PostEventContinuation.Program) || _coreCardMovementProgramsOperations.TryBeginCardsMovedProgramWindow(frameId) || _coreSkillOwnershipWindowsOperations.TryBeginAdvancedSkillsChanged(frameId);
}
