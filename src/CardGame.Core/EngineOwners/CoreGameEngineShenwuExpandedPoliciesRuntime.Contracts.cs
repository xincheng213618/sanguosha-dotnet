#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal interface ICoreShenwuExpandedPoliciesCoreShenwuNpcProgramsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool ShenwuEnabled(global::CardGame.Core.CharacterState player, string skillId);
}

internal interface ICoreShenwuExpandedPoliciesCoreTableSeatQueriesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsValidPlayerSeat(int seat);
}

internal interface ICoreShenwuExpandedPoliciesCoreAfterTurnEndedProgramsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsActualAfterTurnEndedParent(global::CardGame.Core.DeferredTurnEndFrame parent);
}

internal interface ICoreShenwuExpandedPoliciesContentRegistryOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    ref readonly global::CardGame.Core.ContentRegistry _contentRegistry { get; }
}

internal interface ICoreShenwuExpandedPoliciesCoreNativeCardAppearanceOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.Suit EffectiveSuit(global::CardGame.Core.CharacterState owner, global::CardGame.Core.Card card);
}

internal interface ICoreShenwuExpandedPoliciesResolutionStackOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    ref readonly global::CardGame.Core.FrameStore _resolutionStack { get; }
}

internal interface ICoreShenwuExpandedPoliciesCoreProgramCardPoliciesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool HasCardPolicy(global::CardGame.Core.CharacterState owner, global::CardGame.Core.SkillProgramCardPolicyKind kind, global::CardGame.Core.CardKind? effectiveKind = default);
}

internal interface ICoreShenwuExpandedPoliciesCoreShenwuActivitiesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.IShenwuBattleFeature? ShenwuBattleRules { get; }
}

internal interface ICoreShenwuExpandedPoliciesCoreAlternatingBenefitsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::System.Collections.Generic.IEnumerable<global::CardGame.Core.IGameEvent> CompleteProgramEventHistory();
}
