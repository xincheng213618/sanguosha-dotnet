#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal interface ICoreRuleQueriesCoreMatchSetupDefinitionsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    string? GetEffectiveFactionId(global::CardGame.Core.CharacterState player);
}

internal interface ICoreRuleQueriesCoreCardUseModulesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsCardUseForbidden(int actorSeat, global::CardGame.Core.CardKind effectiveKind, global::CardGame.Core.CardActionType actionType, bool ignoreIssuedPlayBan = false, long? existingUseFrameId = default);
}

internal interface ICoreRuleQueriesCoreActionQueriesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.RuleQueryEvaluation EvaluateSlashUseLimit(global::CardGame.Core.CharacterState player, int? excludedEquipmentId = default, global::CardGame.Core.EquipmentSlot? excludedEquipmentSlot = default);
}

internal interface ICoreRuleQueriesMatchCardTargetQueriesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.MatchCardTargetQueries Get();
}

internal interface ICoreRuleQueriesMatchHandLimitQueriesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.MatchHandLimitQueries Get();
}

internal interface ICoreRuleQueriesKernelCoreGameEngineRuleQueriesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.NativeOwners.DistanceQueryFacts CreateDistanceQueryFacts(global::CardGame.Core.CharacterState source, global::CardGame.Core.CharacterState target);
    global::CardGame.Core.NativeOwners.AttackRangeQueryFacts CreateAttackRangeQueryFacts(global::CardGame.Core.CharacterState player);
    global::CardGame.Core.NativeOwners.DrawCountQueryFacts CreateDrawCountQueryFacts(global::CardGame.Core.CharacterState player);
    global::CardGame.Core.NativeOwners.SlashLimitQueryFacts CreateSlashLimitQueryFacts(global::CardGame.Core.CharacterState player);
    global::CardGame.Core.NativeOwners.HandLimitQueryFacts CreateHandLimitQueryFacts(global::CardGame.Core.CharacterState player);
    global::CardGame.Core.NativeOwners.CardTargetCountQueryFacts CreateCardTargetCountQueryFacts(global::CardGame.Core.CharacterState player, global::CardGame.Core.CardKind effectiveCardKind, long? declaredUseFrameId);
    global::CardGame.Core.NativeOwners.CardUseDistanceQueryFacts CreateCardUseDistanceQueryFacts(global::CardGame.Core.CharacterState player, global::CardGame.Core.CardKind effectiveCardKind);
    global::CardGame.Core.NativeOwners.NumericContributionQueryFacts CreateNumericContributionQueryFacts(global::CardGame.Core.CharacterState player, global::CardGame.Core.SkillRuleQuery ruleQuery);
}

internal interface ICoreRuleQueriesCoreNativeCardAppearanceOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.Suit EffectiveSuit(global::CardGame.Core.CharacterState owner, global::CardGame.Core.Card card);
}

internal interface ICoreRuleQueriesProgramNumericContributionOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.ProgramNumericContributionRuntime Get();
}

internal interface ICoreRuleQueriesMatchTurnNumericQueriesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.MatchTurnNumericQueries Get();
}

internal interface ICoreRuleQueriesCharacterDaQiaoXiaoQiaoHappyGodOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsHappyQiaoShutuCardQuery(global::CardGame.Core.CharacterState actor, int? selectedPhysicalCardId);
}

internal interface ICoreRuleQueriesCoreProgramTargetRulesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.ProgramTargetRules TargetRules { get; }
}

internal interface ICoreRuleQueriesCoreDeckEquipmentProgramsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool HasBeneficiarySuitShield(int actor, int target, global::CardGame.Core.Suit? suit);
}

internal interface ICoreRuleQueriesCoreDirectedTurnCardPoliciesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsDirectedCardTargetProhibited(int actorSeat, int targetSeat, global::CardGame.Core.CardKind cardKind, long? cardUseFrameId = default);
}

internal interface ICoreRuleQueriesMatchDistanceQueriesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.MatchDistanceQueries Get();
}
