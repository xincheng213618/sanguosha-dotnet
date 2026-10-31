#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineRoundDistinctBasicRequestsEntries
{
    internal global::CardGame.Core.ProgramMultiCardViewAsSelection RequireRoundDistinctBasicRequestedSlash(global::CardGame.Core.ProgramSkillFrame frame, global::CardGame.Core.PendingDecision decision, global::CardGame.Core.PromptChoice selected, int actorSeat, global::System.Collections.Generic.IReadOnlyList<int> targetSeats, string? resultBind = default) => _resolve().RequireRoundDistinctBasicRequestedSlash(frame, decision, selected, actorSeat, targetSeats, resultBind);
    internal global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.PromptChoice> RoundDistinctBasicRequestedSlashChoices(global::CardGame.Core.ProgramSkillFrame frame, int actorSeat, global::System.Collections.Generic.IReadOnlyList<int> targetSeats, string? resultBind = default) => _resolve().RoundDistinctBasicRequestedSlashChoices(frame, actorSeat, targetSeats, resultBind);
    internal bool RoundDistinctBasicRequestedSlashTarget(global::CardGame.Core.CharacterState actor, global::CardGame.Core.ProgramMultiCardViewAsSelection selection, int targetSeat) => _resolve().RoundDistinctBasicRequestedSlashTarget(actor, selection, targetSeat);
}
