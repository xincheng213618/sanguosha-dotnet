#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal interface ICoreCurrentUseTargetCandidateRefreshResolutionStackOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    ref readonly global::CardGame.Core.FrameStore _resolutionStack { get; }
}

internal interface ICoreCurrentUseTargetCandidateRefreshCoreRuntimeOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void ReplaceRuntimeFrame(long expectedFrameId, global::CardGame.Core.ResolutionFrame next);
}

internal interface ICoreCurrentUseTargetCandidateRefreshCoreProgramCardUseRolesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void UpdateProgramRoleCardUse(global::CardGame.Core.CardUseFrame use, global::CardGame.Core.CardActionContext action);
}

internal interface ICoreCurrentUseTargetCandidateRefreshContentRegistryOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    ref readonly global::CardGame.Core.ContentRegistry _contentRegistry { get; }
}

internal interface ICoreCurrentUseTargetCandidateRefreshCoreDesignatedExtraTargetsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.CardActionContext CloneDesignatedExtraTargetAction(global::CardGame.Core.CardActionContext action, global::System.Collections.Generic.IReadOnlyList<int> targets);
}

internal interface ICoreCurrentUseTargetCandidateRefreshCoreTableOrderOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    int RelativeTableOrder(int from, int target);
}

internal interface ICoreCurrentUseTargetCandidateRefreshCoreProgramTargetWindowProtocolsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.ProgramTargetWindowProtocols TargetWindowProtocols { get; }
}

internal interface ICoreCurrentUseTargetCandidateRefreshCoreCardActionsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.ProgramCardTriggerCandidate> CollectSharedCardActionCandidates(global::CardGame.Core.CardActionContext action, global::CardGame.Core.SkillProgramTriggerWindow window, global::System.Collections.Generic.IReadOnlyList<int> eventTargets, bool? cardUseCausedDamage, global::CardGame.Core.ProgramCardContinuation? continuation = default);
}
