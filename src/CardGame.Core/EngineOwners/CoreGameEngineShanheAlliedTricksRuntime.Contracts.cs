#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal interface ICoreShanheAlliedTricksCoreNativeHarvestOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void BeginHarvestSelection(global::CardGame.Core.NativeOwners.GroupCardHandle group);
}

internal interface ICoreShanheAlliedTricksResolutionStackOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    ref readonly global::CardGame.Core.FrameStore _resolutionStack { get; }
}

internal interface ICoreShanheAlliedTricksCorePendingDecisionPolicyPipelineOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.PendingDecision? _pendingDecision { get; set; }
}

internal interface ICoreShanheAlliedTricksCoreNativeCardContinuationCursorsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.NativeOwners.GroupCardHandle? ActiveGroupCard { get; set; }
}

internal interface ICoreShanheAlliedTricksCoreProgramCardPoliciesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool HasCardPolicy(global::CardGame.Core.CharacterState owner, global::CardGame.Core.SkillProgramCardPolicyKind kind, global::CardGame.Core.CardKind? effectiveKind = default);
}

internal interface ICoreShanheAlliedTricksCoreGroupTargetNullificationOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.CardKind GroupNullificationCardKind(global::CardGame.Core.NativeOwners.GroupCardHandle group);
}

internal interface ICoreShanheAlliedTricksCoreShanheGuideSkillsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsShanheFriendlySeat(int owner, int seat);
}

internal interface ICoreShanheAlliedTricksCoreNativePromptBoundaryOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void ClearPendingDecision();
}
