#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CoreGameEnginePassiveRulePoliciesRuntime
{
    internal bool HasPublicEnemyParticipantDamageModifiers => _contentRegistryOperations._contentRegistry.ProgramDependencies.HasPublicEnemyParticipantDamageModifiers;

    internal bool HasEligiblePublicEnemyDamageObserver(global::CardGame.Core.GameEngine.IDamageAttempt attack, IReadOnlyList<int> propagatedTargets) => HasPublicEnemyParticipantDamageModifiers && !attack.IsSourceLess && _coreTableSeatQueriesOperations.IsValidPlayerSeat(attack.SourceSeat) && _coreMatchParticipantsPlayersAccess._players.Any(player => player.IsAlive && _coreTeamActivityProgramsOperations.PublicTeamEnemy(player.Seat, _coreMatchParticipantsPlayersAccess._players[attack.SourceSeat]) && propagatedTargets.Prepend(attack.TargetSeat).Any(seat => _coreTableSeatQueriesOperations.IsValidPlayerSeat(seat) && seat != attack.SourceSeat && _coreTeamActivityProgramsOperations.PublicTeamEnemy(player.Seat, _coreMatchParticipantsPlayersAccess._players[seat])) && _coreNationalSkillsOperations.GetSkillBindingShard(player).ProgramInstances.Any(instance => instance.Program.DamageModifiers.Any(modifier => modifier.SourceScope == SkillProgramDamageModifierSourceScope.AllLivingSkillOwners && modifier.Condition == SkillProgramDamageModifierCondition.BothParticipantsPublicEnemiesOfOwner)));
    internal bool HasPerOccurrenceSourceDamageModifiers(int sourceSeat) => _coreTableSeatQueriesOperations.IsValidPlayerSeat(sourceSeat) && _coreMatchParticipantsPlayersAccess._players[sourceSeat].IsAlive && _coreNationalSkillsOperations.GetSkillBindingShard(_coreMatchParticipantsPlayersAccess._players[sourceSeat]).ProgramInstances.Any(instance => instance.Program.DamageModifiers.Any(modifier => modifier.SourceScope == SkillProgramDamageModifierSourceScope.DamageSource && modifier.Condition is SkillProgramDamageModifierCondition.SourceHandBelowHp or SkillProgramDamageModifierCondition.SourceHpAtMost or SkillProgramDamageModifierCondition.SourceToTargetDistanceGreaterThan or SkillProgramDamageModifierCondition.SourceRoundIsOdd or SkillProgramDamageModifierCondition.SourceRoundIsEven));
    internal static bool MatchesPerOccurrenceSourceDamageModifier(SkillProgramDamageModifier modifier, bool sourceHasHp, int sourceHp, int sourceHandCount, int sourceToTargetDistance, int roundNumber) => global::CardGame.Core.RuntimeSkills.RuntimeRules_Core_PassiveRulePolicies.MatchesPerOccurrenceSourceDamageModifier(modifier, sourceHasHp, sourceHp, sourceHandCount, sourceToTargetDistance, roundNumber);
    [ProgramDistanceRule(ProgramDistanceStage.DistanceProjection, 60)]
    internal RuleQueryEvaluation ApplyPersistentOutgoingDistancePolicy(CharacterState source, CharacterState target, RuleQueryEvaluation original)
    {
        if (source.Seat == target.Seat)
            return original;
        var policies = _coreProgramCardPoliciesOperations.CardPolicies(source, SkillProgramCardPolicyKind.OutgoingDistanceOne).ToArray();
        if (policies.Length == 0)
            return original;
        // Apply after seat distance, both players' equipment and other directional
        // adjustments. Setting only the outgoing contribution would still let an
        // enemy's defensive horse increase the final distance.
        return RuleQueryService.Evaluate(SkillRuleQuery.OutgoingDistance, new(1, int.MaxValue), [new RuleQueryBaseTerm("distance:before-persistent-fixed-policy", global::CardGame.Core.EngineOwners.CoreGameEngineRuleQueriesRuntime.ConvertRuleValue(original))], policies.Select(binding => (RuleQueryContribution)new FiniteRuleQueryContribution($"skill:{(binding.Source.SkillId)}:{(binding.Source.SkillInstanceId)}:policy:{(binding.Policy.Id)}", SkillRuleOperation.Set, 1)).ToArray());
    }

    internal bool ApplyDelayedJudgmentOutcomePolicies(JudgmentFrame frame, bool succeeded)
    {
        var kind = frame.Continuation switch
        {
            JudgmentContinuationKind.Indulgence => CardKind.Indulgence,
            JudgmentContinuationKind.SupplyShortage => CardKind.SupplyShortage,
            _ => (CardKind? )null
        };
        // This changes the final success fact once, after all card/suit/rank
        // replacements. Multiple enabled copies express the same inversion;
        // they must not toggle the result back again.
        return kind is { } delayed && _coreProgramCardPoliciesOperations.CardPolicies(_coreMatchParticipantsPlayersAccess._players[frame.TargetSeat], SkillProgramCardPolicyKind.ReverseDelayedJudgment, delayed).Any() ? !succeeded : succeeded;
    }
}
