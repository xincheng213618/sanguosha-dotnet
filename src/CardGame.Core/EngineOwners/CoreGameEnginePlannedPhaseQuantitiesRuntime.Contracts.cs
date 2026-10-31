#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal interface ICorePlannedPhaseQuantitiesContentRegistryOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    ref readonly global::CardGame.Core.ContentRegistry _contentRegistry { get; }
}

internal interface ICorePlannedPhaseQuantitiesCoreProgramLifecycleOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    int GetProgramPlannedNormalDrawCount(global::CardGame.Core.ProgramSkillFrame frame, bool allowReplacedNoOp = false);
    void AdjustProgramNormalDraw(global::CardGame.Core.ProgramSkillFrame frame, int amount);
}

internal interface ICorePlannedPhaseQuantitiesCoreRuleQueriesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.RuleQueryContribution> CollectNumericRuleContributions(global::CardGame.Core.CharacterState player, global::CardGame.Core.SkillRuleQuery query, global::CardGame.Core.CardKind? effectiveCardKind = default);
}

internal interface ICorePlannedPhaseQuantitiesResolutionStackOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    ref readonly global::CardGame.Core.FrameStore _resolutionStack { get; }
}
