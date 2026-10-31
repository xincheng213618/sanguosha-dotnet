#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineTurnDefaultStatsValidationEntries
{
    internal bool ExactTurnDefaultStatDamageParent(ProgramSkillFrame f, scoped out DamageFrame damage, scoped out DamageTriggerWindowFrame window, scoped out GameEngine.IDamageAttempt attack) => _resolve().ExactTurnDefaultStatDamageParent(f, out damage, out window, out attack);
    internal bool ExactTurnDefaultStatPhaseParent(ProgramSkillFrame f) => _resolve().ExactTurnDefaultStatPhaseParent(f);
    internal bool IsTurnDefaultStatChoice(ProgramSkillFrame f, PendingDecision decision) => _resolve().IsTurnDefaultStatChoice(f, decision);
}
