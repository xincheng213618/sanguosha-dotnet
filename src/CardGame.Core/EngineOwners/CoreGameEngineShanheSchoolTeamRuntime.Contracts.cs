#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal interface ICoreShanheSchoolTeamCoreBoundCardMovementContinuationOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.SkillProgramStepOutcome AwaitProgramBoundCardMovements(long frameId, int ownerSeat);
}

internal interface ICoreShanheSchoolTeamCoreProgramLifecycleOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.ProgramSkillFrame GetActiveProgramFrame(long frameId);
    void ChangeProgramParticipantMaximumHp(global::CardGame.Core.ProgramSkillFrame frame, int recipientSeat, int amount, string? sourceSkillId = default);
    void CompleteProgramDrawCards(long frameId, int targetSeat, string? resultBind, global::CardGame.Core.SkillProgramCardSetVisibility visibility, global::CardGame.Core.CardMoveReason reason, long fixedRecipientSequenceBefore, global::System.Collections.Generic.IReadOnlyList<int> drawn);
}

internal interface ICoreShanheSchoolTeamResolutionStackOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    ref readonly global::CardGame.Core.FrameStore _resolutionStack { get; }
}

internal interface ICoreShanheSchoolTeamCoreHpChangeProgramsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.HpChangeContext? RecordHpChange(long? parentFrameId, int? sourceSeat, int targetSeat, int before, int after, global::CardGame.Core.HpChangeKind kind);
}

internal interface ICoreShanheSchoolTeamCoreNationalSkillsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::System.Collections.Generic.IReadOnlyList<string> EnabledContentSkillIds(global::CardGame.Core.CharacterState player);
}

internal interface ICoreShanheSchoolTeamCoreShanheSchoolsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool HasShanheSchoolEffect(int ownerSeat, int schoolId);
}

internal interface ICoreShanheSchoolTeamCoreProgramEventHistoryOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::System.Collections.Generic.IReadOnlyList<T> ProgramEventHistory<T>()
        where T : global::CardGame.Core.IGameEvent;
}

internal interface ICoreShanheSchoolTeamKernelCoreGameEngineRuleProgressionOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void AdvanceEventRulesAndQueueFact(global::CardGame.Core.IGameEvent payload);
}

internal interface ICoreShanheSchoolTeamCoreRuntimeOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void ReplaceRuntimeTop(global::CardGame.Core.ResolutionFrame next);
    void AdvanceRuntimeProgram(long frameId);
}

internal interface ICoreShanheSchoolTeamCoreTeamActivityProgramsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool OtherRevealedPublicTeamAlly(global::CardGame.Core.CharacterState owner, global::CardGame.Core.CharacterState target);
}

internal interface ICoreShanheSchoolTeamShanheMarketOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    ref global::CardGame.Core.Campaigns.ShanheCampaignSession? _shanheMarket { get; }
}

internal interface ICoreShanheSchoolTeamCoreShanheMarketApplicationOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool DrainShanheMarketChildren(global::CardGame.Core.ProgramSkillFrame frame);
}

internal interface ICoreShanheSchoolTeamCoreProgramSkillChangesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void CommitProgramParticipantSkillGrants(global::CardGame.Core.ProgramSkillFrame frame, int recipientSeat, global::System.Collections.Generic.IReadOnlyList<string> skillIds, string sourceId, global::CardGame.Core.SkillGrantTurnExpiry? turnExpiry, global::CardGame.Core.SkillGrantNextActualTurnStartExpiry? nextStartExpiry);
}

internal interface ICoreShanheSchoolTeamCoreProgramNativeDrawOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.SkillProgramStepOutcome BeginOperationProgramDraw(global::CardGame.Core.ProgramSkillFrame frame, int recipientSeat, int requestedCount, global::CardGame.Core.CardMoveReason reason);
}
