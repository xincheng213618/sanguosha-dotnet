#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineActorHandLimitPenaltiesEntries
{
    internal global::System.Collections.Generic.IEnumerable<global::CardGame.Core.RuleQueryContribution> ActorHandLimitPenaltyContributions(global::CardGame.Core.CharacterState player) => _resolve().ActorHandLimitPenaltyContributions(player);
    internal bool CanOfferActorHandLimitPenalty(global::CardGame.Core.ProgramTriggerCandidate candidate, global::CardGame.Core.SkillProgramTrigger trigger, global::CardGame.Core.ProgramSkillWindowContext context) => _resolve().CanOfferActorHandLimitPenalty(candidate, trigger, context);
    internal void ExpireActorHandLimitPenalties(global::CardGame.Core.TurnEndedEvent ended) => _resolve().ExpireActorHandLimitPenalties(ended);
    internal void ExpireActorHandLimitPenaltiesAfterGameEnded(global::CardGame.Core.GameEndedEvent ended) => _resolve().ExpireActorHandLimitPenaltiesAfterGameEnded(ended);
    internal void ExpireAllActorHandLimitPenalties(string reason) => _resolve().ExpireAllActorHandLimitPenalties(reason);
    internal void GrantProgramActorHandLimitPenalty(global::CardGame.Core.ProgramSkillFrame supplied, int amount) => _resolve().GrantProgramActorHandLimitPenalty(supplied, amount);
    internal bool TryGetActorHandLimitPenaltyUse(int ownerSeat, global::CardGame.Core.ProgramSkillWindowContext context, scoped out global::CardGame.Core.CardUseFrame use, scoped out global::CardGame.Core.ProgramCardTriggerWindowFrame window) => _resolve().TryGetActorHandLimitPenaltyUse(ownerSeat, context, out use, out window);
    internal bool HasActorHandLimitPenaltyCapability { get => _resolve().HasActorHandLimitPenaltyCapability; }
}
