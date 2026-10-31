#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineRoundDistinctBasicRequestsEntries
{
    internal ProgramMultiCardViewAsSelection RequireRoundDistinctBasicRequestedSlash(ProgramSkillFrame frame, PendingDecision decision, PromptChoice selected, int actorSeat, IReadOnlyList<int> targetSeats, string? resultBind = default) => _resolve().RequireRoundDistinctBasicRequestedSlash(frame, decision, selected, actorSeat, targetSeats, resultBind);
    internal IReadOnlyList<PromptChoice> RoundDistinctBasicRequestedSlashChoices(ProgramSkillFrame frame, int actorSeat, IReadOnlyList<int> targetSeats, string? resultBind = default) => _resolve().RoundDistinctBasicRequestedSlashChoices(frame, actorSeat, targetSeats, resultBind);
    internal bool RoundDistinctBasicRequestedSlashTarget(CharacterState actor, ProgramMultiCardViewAsSelection selection, int targetSeat) => _resolve().RoundDistinctBasicRequestedSlashTarget(actor, selection, targetSeat);
}
