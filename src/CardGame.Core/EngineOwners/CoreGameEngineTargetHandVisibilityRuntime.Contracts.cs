#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal interface ICoreTargetHandVisibilityCoreTableSeatQueriesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsValidPlayerSeat(int seat);
}

internal interface ICoreTargetHandVisibilityCoreWushuangCombatTacticsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool WushuangHasVisibleTargetHand(int source, global::CardGame.Core.CardKind kind);
}

internal interface ICoreTargetHandVisibilityCoreNativeCardContinuationCursorsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.TargetCardSelectionFrame? ActiveTargetCardSelection { get; }
}

internal interface ICoreTargetHandVisibilityCorePendingDecisionPolicyPipelineOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.PendingDecision? _pendingDecision { get; set; }
}

internal interface ICoreTargetHandVisibilityCoreProgramShanheSourceZhaoranOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool HasShanheSourceZhaoranPublicHand(int ownerSeat);
}

internal interface ICoreTargetHandVisibilityCoreProgramCardPoliciesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool HasCardPolicy(global::CardGame.Core.CharacterState owner, global::CardGame.Core.SkillProgramCardPolicyKind kind, global::CardGame.Core.CardKind? effectiveKind = default);
}

internal interface ICoreTargetHandVisibilityCoreNativePromptBoundaryOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.PromptId CreatePromptId();
}

internal interface ICoreTargetHandVisibilityCorePhysicalOwnedCardQueriesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.Card> GetHand(global::CardGame.Core.CharacterState player);
}
