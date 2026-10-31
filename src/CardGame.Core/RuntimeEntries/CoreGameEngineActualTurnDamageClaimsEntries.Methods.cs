#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineActualTurnDamageClaimsEntries
{
    internal SkillProgramStepOutcome BeginActualTurnDamageClaim(ProgramSkillFrame frame) => _resolve().BeginActualTurnDamageClaim(frame);
    internal bool CanRunPaidColorDamageClaim(ProgramTriggerCandidate candidate, SkillProgramTrigger trigger, ProgramSkillWindowContext context) => _resolve().CanRunPaidColorDamageClaim(candidate, trigger, context);
    internal IReadOnlyList<ActualTurnCardDamageEntityEvent> EligibleActualTurnDamageEntities(int owner) => _resolve().EligibleActualTurnDamageEntities(owner);
    internal bool MatchesActualTurnDamageEnding(ProgramSkillFrame root) => _resolve().MatchesActualTurnDamageEnding(root);
    internal bool MatchesDeclaredActualDamageUse(CardUseFrame use, int actor, int provider) => _resolve().MatchesDeclaredActualDamageUse(use, actor, provider);
    internal void ObserveActualTurnDamageEntities(IGameEvent payload) => _resolve().ObserveActualTurnDamageEntities(payload);
    internal bool TracksActualTurnDamageEntities { get => _resolve().TracksActualTurnDamageEntities; }
}
