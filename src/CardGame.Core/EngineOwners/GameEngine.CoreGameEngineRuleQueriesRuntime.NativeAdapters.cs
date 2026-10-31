#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core;
public sealed partial class GameEngine
{
    [ProgramCapability]
    private sealed class CoreRuleQueriesKernelCoreGameEngineRuleQueriesAdapter(GameEngine engine) : global::CardGame.Core.EngineOwners.ICoreRuleQueriesKernelCoreGameEngineRuleQueriesOperations
    {
        public global::CardGame.Core.NativeOwners.DistanceQueryFacts CreateDistanceQueryFacts(global::CardGame.Core.CharacterState source, global::CardGame.Core.CharacterState target) => engine.ComposeNativeDistanceQueryFacts(source, target);
        public global::CardGame.Core.NativeOwners.AttackRangeQueryFacts CreateAttackRangeQueryFacts(global::CardGame.Core.CharacterState player) => engine.ComposeNativeAttackRangeQueryFacts(player);
        public global::CardGame.Core.NativeOwners.DrawCountQueryFacts CreateDrawCountQueryFacts(global::CardGame.Core.CharacterState player) => engine.ComposeNativeDrawCountQueryFacts(player);
        public global::CardGame.Core.NativeOwners.SlashLimitQueryFacts CreateSlashLimitQueryFacts(global::CardGame.Core.CharacterState player) => engine.ComposeNativeSlashLimitQueryFacts(player);
        public global::CardGame.Core.NativeOwners.HandLimitQueryFacts CreateHandLimitQueryFacts(global::CardGame.Core.CharacterState player) => engine.ComposeNativeHandLimitQueryFacts(player);
        public global::CardGame.Core.NativeOwners.CardTargetCountQueryFacts CreateCardTargetCountQueryFacts(global::CardGame.Core.CharacterState player, global::CardGame.Core.CardKind effectiveCardKind, long? declaredUseFrameId) => engine.ComposeNativeCardTargetCountQueryFacts(player, effectiveCardKind, declaredUseFrameId);
        public global::CardGame.Core.NativeOwners.CardUseDistanceQueryFacts CreateCardUseDistanceQueryFacts(global::CardGame.Core.CharacterState player, global::CardGame.Core.CardKind effectiveCardKind) => engine.ComposeNativeCardUseDistanceQueryFacts(player, effectiveCardKind);
        public global::CardGame.Core.NativeOwners.NumericContributionQueryFacts CreateNumericContributionQueryFacts(global::CardGame.Core.CharacterState player, global::CardGame.Core.SkillRuleQuery ruleQuery) => engine.ComposeNativeNumericContributionQueryFacts(player, ruleQuery);
    }
}
