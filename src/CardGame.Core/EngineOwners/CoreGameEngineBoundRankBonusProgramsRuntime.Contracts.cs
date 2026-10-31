#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal interface ICoreBoundRankBonusProgramsCoreDyingCursorRuntimeOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.DyingFrame? ActiveDying { get; }
}

internal interface ICoreBoundRankBonusProgramsCardZonesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    ref readonly global::CardGame.Core.CardZoneStore _cardZones { get; }
}

internal interface ICoreBoundRankBonusProgramsCoreBoundCardMovementContinuationOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.SkillProgramStepOutcome AwaitProgramBoundCardMovements(long frameId, int ownerSeat);
}

internal interface ICoreBoundRankBonusProgramsCoreProgramLifecycleOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.ProgramSkillFrame GetActiveProgramFrame(long frameId);
    void RevealProgramTopCards(long frameId, int ownerSeat, int amount, global::CardGame.Core.SkillProgramNumberExpression? numberExpression, string resultBind, global::CardGame.Core.SkillProgramCardSetVisibility visibility);
    global::CardGame.Core.ProgramSkillCardSetBinding GetProgramCardSet(global::CardGame.Core.ProgramSkillFrame frame, string bind);
    global::CardGame.Core.SkillProgramStepOutcome MoveProgramBoundCards(long frameId, int ownerSeat, string sourceBind, string? exceptBind, global::CardGame.Core.SkillProgramCardDestination destination, global::CardGame.Core.CardZoneKind? destinationZone, global::CardGame.Core.CardMoveReason reason, global::System.Collections.Generic.IReadOnlyList<int>? bottomOrder = default);
    void CancelProgramBindingAndCleanup(global::CardGame.Core.ProgramSkillFrame frame, string reason);
    void RecoverProgramTargetTo(long frameId, int ownerSeat, int targetSeat, global::CardGame.Core.SkillProgramNumberExpression expression, int minimumValue, bool clampToMaxHp);
}

internal interface ICoreBoundRankBonusProgramsResolutionStackOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    ref readonly global::CardGame.Core.FrameStore _resolutionStack { get; }
}

internal interface ICoreBoundRankBonusProgramsCoreTableSeatQueriesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsValidPlayerSeat(int seat);
}

internal interface ICoreBoundRankBonusProgramsCoreProgramPersistentStateOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool GetProgramBooleanState(global::CardGame.Core.ProgramSkillFrame frame, string stateId);
}

internal interface ICoreBoundRankBonusProgramsCoreProgramSkillChangesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void SetProgramBooleanState(global::CardGame.Core.ProgramSkillFrame frame, string stateId, bool value);
}
