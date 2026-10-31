#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal interface ICoreProgramPhaseBoundaryRuntimePortsCorePrepDiscardEndingPromisesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void AppendPrepDiscardEndingItems(global::System.Collections.Generic.List<global::CardGame.Core.TurnEndingBoundaryItem> items);
    void ConsumeSkippedPrepDiscardEnding(global::CardGame.Core.TurnEndingBoundaryFrame frame);
}

internal interface ICoreProgramPhaseBoundaryRuntimePortsProgramPipelineHooksOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    ref readonly global::CardGame.Core.ProgramPipelineHooks _programPipelineHooks { get; }
}

internal interface ICoreProgramPhaseBoundaryRuntimePortsCharacterLvQianOlOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.WeiluPhaseKey? CaptureWeiluStartingPhase(global::CardGame.Core.CharacterState owner);
}

internal interface ICoreProgramPhaseBoundaryRuntimePortsCoreTableOrderOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    int RelativeTableOrder(int from, int target);
}

internal interface ICoreProgramPhaseBoundaryRuntimePortsCharacterYanPuOlOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void AppendYanPuEndingItems(global::System.Collections.Generic.List<global::CardGame.Core.TurnEndingBoundaryItem> items);
}

internal interface ICoreProgramPhaseBoundaryRuntimePortsCoreEarnedActualEndingBenefitsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void AppendEarnedActualEndingItems(global::System.Collections.Generic.List<global::CardGame.Core.TurnEndingBoundaryItem> items);
    void ConsumeSkippedEarnedActualEnding(global::CardGame.Core.TurnEndingBoundaryFrame frame);
}

internal interface ICoreProgramPhaseBoundaryRuntimePortsCharacterRuanYuOlOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void AppendRuanYuXingzuoEndingItems(global::System.Collections.Generic.List<global::CardGame.Core.TurnEndingBoundaryItem> items);
}

internal interface ICoreProgramPhaseBoundaryRuntimePortsKernelCoreMainOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void CompleteGame();
}

internal interface ICoreProgramPhaseBoundaryRuntimePortsCoreActualTurnsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void FinalizeEndTurn(global::CardGame.Core.CharacterState previous);
}

internal interface ICoreProgramPhaseBoundaryRuntimePortsCharacterZhangJiaoAnniversaryGodOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool HasAnniversaryTianjieDeadOwnerEnding(int actualOwner);
    bool IsAnniversaryTianjieDeadOwnerEndingCandidate(global::CardGame.Core.ProgramTriggerCandidate candidate, int actualOwner);
}

internal interface ICoreProgramPhaseBoundaryRuntimePortsCharacterGuoTuOlOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.ProgramSkillWindowContext CreateQushiEndingContext(global::CardGame.Core.TurnEndingBoundaryFrame parent, global::CardGame.Core.ProgramTriggerCandidate c, global::CardGame.Core.ProgramSkillWindowContext context);
    void AppendQushiEndingItems(global::System.Collections.Generic.List<global::CardGame.Core.TurnEndingBoundaryItem> items);
}

internal interface ICoreProgramPhaseBoundaryRuntimePortsKernelCoreGameEngineRuleProgressionOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void AdvanceRulesAndPublishState();
}

internal interface ICoreProgramPhaseBoundaryRuntimePortsCharacterLuZhiWeiOlOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.QingzhongPlayPhaseKey? CaptureQingzhongStartingPhase(global::CardGame.Core.CharacterState owner);
}

internal interface ICoreProgramPhaseBoundaryRuntimePortsCharacterWenYangOlOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void AppendWenYangEndingItems(global::System.Collections.Generic.List<global::CardGame.Core.TurnEndingBoundaryItem> items);
}

internal interface ICoreProgramPhaseBoundaryRuntimePortsCoreConvertingGiftsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void AppendGiftRetentionEndingItems(global::System.Collections.Generic.List<global::CardGame.Core.TurnEndingBoundaryItem> items);
    bool ConsumeGiftRetentionEndingItem(global::CardGame.Core.TurnEndingBoundaryFrame frame, global::CardGame.Core.TurnEndingBoundaryItem item);
}

internal interface ICoreProgramPhaseBoundaryRuntimePortsCoreFixedDistanceOneDebtOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void AppendFixedDistanceOneEndingItems(global::System.Collections.Generic.List<global::CardGame.Core.TurnEndingBoundaryItem> items);
    void ConsumeSkippedFixedDistanceDebt(global::CardGame.Core.TurnEndingBoundaryFrame f);
}

internal interface ICoreProgramPhaseBoundaryRuntimePortsCoreWenheActivityProgramsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool HasWenheDeadOwnerEnding(int actualOwner);
    bool IsWenheDeadOwnerEndingCandidate(global::CardGame.Core.ProgramTriggerCandidate candidate, int actualOwner);
}

internal interface ICoreProgramPhaseBoundaryRuntimePortsCoreNeighborDiscardOpportunitiesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    int PreviousLivingSeatFor(int owner);
}

internal interface ICoreProgramPhaseBoundaryRuntimePortsCharacterLiuBaOlOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void AppendLiuBaCoinEndingItems(global::System.Collections.Generic.List<global::CardGame.Core.TurnEndingBoundaryItem> items);
}

internal interface ICoreProgramPhaseBoundaryRuntimePortsCharacterLiWanOlOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void AppendLianjuEndingItems(global::System.Collections.Generic.List<global::CardGame.Core.TurnEndingBoundaryItem> items);
}

internal interface ICoreProgramPhaseBoundaryRuntimePortsCoreTreatedWoundedOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool UsesTreatedWounded { get; }

    bool IsRuleWounded(global::CardGame.Core.CharacterState player);
}
