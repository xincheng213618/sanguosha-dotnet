#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineActualTurnDamageClaimsEntries
{
    internal global::CardGame.Core.SkillProgramStepOutcome BeginActualTurnDamageClaim(global::CardGame.Core.ProgramSkillFrame frame) => _resolve().BeginActualTurnDamageClaim(frame);
    internal bool CanRunPaidColorDamageClaim(global::CardGame.Core.ProgramTriggerCandidate candidate, global::CardGame.Core.SkillProgramTrigger trigger, global::CardGame.Core.ProgramSkillWindowContext context) => _resolve().CanRunPaidColorDamageClaim(candidate, trigger, context);
    internal global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.ActualTurnCardDamageEntityEvent> EligibleActualTurnDamageEntities(int owner) => _resolve().EligibleActualTurnDamageEntities(owner);
    internal bool MatchesActualTurnDamageEnding(global::CardGame.Core.ProgramSkillFrame root) => _resolve().MatchesActualTurnDamageEnding(root);
    internal bool MatchesDeclaredActualDamageUse(global::CardGame.Core.CardUseFrame use, int actor, int provider) => _resolve().MatchesDeclaredActualDamageUse(use, actor, provider);
    internal void ObserveActualTurnDamageEntities(global::CardGame.Core.IGameEvent payload) => _resolve().ObserveActualTurnDamageEntities(payload);
    internal bool TracksActualTurnDamageEntities { get => _resolve().TracksActualTurnDamageEntities; }
}
