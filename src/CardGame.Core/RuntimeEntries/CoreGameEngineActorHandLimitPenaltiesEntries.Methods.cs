#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineActorHandLimitPenaltiesEntries
{
    internal IEnumerable<RuleQueryContribution> ActorHandLimitPenaltyContributions(CharacterState player) => _resolve().ActorHandLimitPenaltyContributions(player);
    internal bool CanOfferActorHandLimitPenalty(ProgramTriggerCandidate candidate, SkillProgramTrigger trigger, ProgramSkillWindowContext context) => _resolve().CanOfferActorHandLimitPenalty(candidate, trigger, context);
    internal void ExpireActorHandLimitPenalties(TurnEndedEvent ended) => _resolve().ExpireActorHandLimitPenalties(ended);
    internal void ExpireActorHandLimitPenaltiesAfterGameEnded(GameEndedEvent ended) => _resolve().ExpireActorHandLimitPenaltiesAfterGameEnded(ended);
    internal void ExpireAllActorHandLimitPenalties(string reason) => _resolve().ExpireAllActorHandLimitPenalties(reason);
    internal void GrantProgramActorHandLimitPenalty(ProgramSkillFrame supplied, int amount) => _resolve().GrantProgramActorHandLimitPenalty(supplied, amount);
    internal bool TryGetActorHandLimitPenaltyUse(int ownerSeat, ProgramSkillWindowContext context, scoped out CardUseFrame use, scoped out ProgramCardTriggerWindowFrame window) => _resolve().TryGetActorHandLimitPenaltyUse(ownerSeat, context, out use, out window);
    internal bool HasActorHandLimitPenaltyCapability { get => _resolve().HasActorHandLimitPenaltyCapability; }
}
