#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal interface ICoreProgramPhaseSchedulingRuntimePortsCoreGrantedEntityPhasesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsReturningGrantedEntityPhase(global::CardGame.Core.ProgramSkillFrame f);
    global::CardGame.Core.ProgramSkillFrame FreezeGrantedEntityPhaseEnd(global::CardGame.Core.ProgramSkillFrame f);
}

internal interface ICoreProgramPhaseSchedulingRuntimePortsCharacterDuFuRenOlOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void ConsumeDuFuRenPhase(global::CardGame.Core.ActualDiscardRecoveryPhaseKind kind, int ownerSeat);
}

internal interface ICoreProgramPhaseSchedulingRuntimePortsCoreAfterTurnEndedProgramsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool MatchesAfterTurnEndedChild(global::CardGame.Core.DeferredTurnEndFrame parent, global::CardGame.Core.ProgramSkillFrame child, global::CardGame.Core.ProgramSkillWindowContext context);
}

internal interface ICoreProgramPhaseSchedulingRuntimePortsCharacterZhangYiOlOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void ResumeZhangYiDianjunExtraPlay(long programFrameId);
}

internal interface ICoreProgramPhaseSchedulingRuntimePortsProgramPipelineHooksOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    ref readonly global::CardGame.Core.ProgramPipelineHooks _programPipelineHooks { get; }
}

internal interface ICoreProgramPhaseSchedulingRuntimePortsCoreActualTurnSkillGrantsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void ExpireActualTurnSkillGrants(int turnNumber, int turnOwnerSeat);
}

internal interface ICoreProgramPhaseSchedulingRuntimePortsCorePhysicalDrawFacadesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::System.Collections.Generic.IReadOnlyList<int> DrawCards(global::CardGame.Core.CharacterState player, int count, bool log, global::CardGame.Core.CardMoveReason? reason = default, global::CardGame.Core.CardConversionSource? directProducer = default);
}

internal interface ICoreProgramPhaseSchedulingRuntimePortsCoreProgramLifecycleOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool TryBeginDrawPhaseProgramWindow(global::CardGame.Core.CharacterState owner, bool skipPlayPhaseAfterDraw);
    void CompleteDetachedProgramBinding(global::CardGame.Core.ProgramSkillFrame frame, bool completed);
}

internal interface ICoreProgramPhaseSchedulingRuntimePortsCharacterZhangShiPingOlOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool TryResumeZhangShiPingHongjiPhase();
    bool TryCancelZhangShiPingHongjiPhase();
}

internal interface ICoreProgramPhaseSchedulingRuntimePortsCoreActualTurnsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void EnterPlayPhase(global::CardGame.Core.CharacterState current);
    void CompleteTurnStartAfterDraw(global::CardGame.Core.CharacterState current, global::CardGame.Core.GameEngine.DelayedTurnEffects delayedEffects, bool afterNormalDrawProgramsCompleted = false, bool drawPhaseEndedProgramsCompleted = false, bool actualDrawCompletionCompleted = false);
    void ExpireActualTurnEndEffects(global::CardGame.Core.CharacterState previous);
}

internal interface ICoreProgramPhaseSchedulingRuntimePortsCorePlayPhaseSkillGrantsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void ExpirePlayPhaseSkillGrantsForEndedTurn(int turnNumber);
}

internal interface ICoreProgramPhaseSchedulingRuntimePortsCharacterChengGongYingOlOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void ConsumeChengGongYingDraw(int actorSeat);
}

internal interface ICoreProgramPhaseSchedulingRuntimePortsCharacterCaoXiOlOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    int FreezeAndCaptureCaoXiNativeDrawBase(global::CardGame.Core.CharacterState player, bool replaced = false);
}

internal interface ICoreProgramPhaseSchedulingRuntimePortsProgramPhaseScheduleOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    ref global::CardGame.Core.ProgramPhaseSchedule? _programPhaseSchedule { get; }
}

internal interface ICoreProgramPhaseSchedulingRuntimePortsCoreNationalSkillsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool HasRuntimeSkillInstance(global::CardGame.Core.CharacterState player, string skillId, string skillInstanceId);
}
