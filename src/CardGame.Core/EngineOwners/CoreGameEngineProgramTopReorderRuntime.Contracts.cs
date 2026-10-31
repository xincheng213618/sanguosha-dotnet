#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal interface ICoreProgramTopReorderCorePhysicalOwnedCardQueriesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.Card> GetJudgment(global::CardGame.Core.CharacterState player);
}

internal interface ICoreProgramTopReorderCoreProgramSkillChangesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void SetProgramBooleanState(global::CardGame.Core.ProgramSkillFrame frame, string stateId, bool value);
}

internal interface ICoreProgramTopReorderResolutionStackOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    ref readonly global::CardGame.Core.FrameStore _resolutionStack { get; }
}

internal interface ICoreProgramTopReorderContentRegistryOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    ref readonly global::CardGame.Core.ContentRegistry _contentRegistry { get; }
}

internal interface ICoreProgramTopReorderCorePrivateOfferReturnsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.SkillProgramEffect? GetPausedPrivateOfferEffect(global::CardGame.Core.ProgramSkillFrame f);
}

internal interface ICoreProgramTopReorderCorePendingDecisionPolicyPipelineOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.PendingDecision? _pendingDecision { get; set; }
}

internal interface ICoreProgramTopReorderCoreProgramLifecycleOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.ProgramSkillFrame GetActiveProgramFrame(long frameId);
    void CancelProgramBindingAndCleanup(global::CardGame.Core.ProgramSkillFrame frame, string reason);
}

internal interface ICoreProgramTopReorderCoreExactTopReorderProgramsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void ValidateExactTopReorder(global::CardGame.Core.ProgramSkillFrame frame);
}

internal interface ICoreProgramTopReorderCharacterDaQiaoLeOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsZixiQinJudgmentCard(int id);
}

internal interface ICoreProgramTopReorderKernelCoreMainOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.CommandResult Accept(global::System.Action operation);
}

internal interface ICoreProgramTopReorderCoreSnapshotValueProjectionOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.CardSnapshot ToSnapshot(global::CardGame.Core.Card card);
}

internal interface ICoreProgramTopReorderKernelCoreGameEngineRuleProgressionOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void AdvanceRulesAndPublishState();
}

internal interface ICoreProgramTopReorderCoreActualTurnsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.CardKind GetJudgmentEffectiveCardKind(global::CardGame.Core.Card card);
    int GetTurnDrawCount(global::CardGame.Core.CharacterState current);
}

internal interface ICoreProgramTopReorderKernelCoreGameEngineRuntimeOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void AdvanceToHumanBoundary();
}

internal interface ICoreProgramTopReorderCorePhysicalDrawFacadesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool EnsureDrawPile();
}

internal interface ICoreProgramTopReorderCoreNativePromptBoundaryOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.PromptId CreatePromptId();
    void ClearPendingDecision();
}

internal interface ICoreProgramTopReorderCoreRuntimeOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void ReplaceRuntimeTop(global::CardGame.Core.ResolutionFrame next);
    void AdvanceRuntimeProgram(long frameId);
}

internal interface ICoreProgramTopReorderCharacterZhugeGuoOlOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void MarkYuhuaTopPlacement(global::CardGame.Core.ProgramSkillFrame supplied, global::CardGame.Core.ProgramTopReorder state);
}

internal interface ICoreProgramTopReorderCorePopulationTopReorderProgramsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void ValidatePopulationTopReorder(global::CardGame.Core.ProgramSkillFrame frame);
}

internal interface ICoreProgramTopReorderCoreNationalSkillsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool HasRuntimeSkillInstance(global::CardGame.Core.CharacterState player, string skillId, string skillInstanceId);
}

internal interface ICoreProgramTopReorderCardZonesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    ref readonly global::CardGame.Core.CardZoneStore _cardZones { get; }
}
