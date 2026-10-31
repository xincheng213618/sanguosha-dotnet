#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineTurnDefaultStatsValidationEntries
{
    internal void AssertTurnDefaultStatAllocation(global::CardGame.Core.ProgramSkillFrame f, global::CardGame.Core.ProgramExecutionPlan plan) => _resolve().AssertTurnDefaultStatAllocation(f, plan);
    internal void AssertTurnDefaultStatState() => _resolve().AssertTurnDefaultStatState();
    internal bool ExactTurnDefaultStatDamageParent(global::CardGame.Core.ProgramSkillFrame f, scoped out global::CardGame.Core.DamageFrame damage, scoped out global::CardGame.Core.DamageTriggerWindowFrame window, scoped out global::CardGame.Core.GameEngine.IDamageAttempt attack) => _resolve().ExactTurnDefaultStatDamageParent(f, out damage, out window, out attack);
    internal bool ExactTurnDefaultStatPhaseParent(global::CardGame.Core.ProgramSkillFrame f) => _resolve().ExactTurnDefaultStatPhaseParent(f);
    internal bool IsTurnDefaultStatChoice(global::CardGame.Core.ProgramSkillFrame f, global::CardGame.Core.PendingDecision decision) => _resolve().IsTurnDefaultStatChoice(f, decision);
    internal bool TurnDefaultStatSourceMatches(global::CardGame.Core.CardConversionSource source, string hash, string stateId, global::CardGame.Core.SkillProgramEffectOp? requiredOp = default) => _resolve().TurnDefaultStatSourceMatches(source, hash, stateId, requiredOp);
    internal bool ValidTurnDefaultStatAllocation(global::CardGame.Core.ProgramSkillFrame f) => _resolve().ValidTurnDefaultStatAllocation(f);
}
