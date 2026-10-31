#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineShanheChilianEntries
{
    internal global::System.Collections.Generic.IReadOnlyList<int> ChilianLiangyinTargets(int ownerSeat, string bindingId) => _resolve().ChilianLiangyinTargets(ownerSeat, bindingId);
    internal void CollectChilianLiangyinCandidates(global::CardGame.Core.CardMovementBatchContext batch, global::System.Collections.Generic.List<global::CardGame.Core.ProgramTriggerCandidate> candidates) => _resolve().CollectChilianLiangyinCandidates(batch, candidates);
    internal global::CardGame.Core.ProgramSkillWindowContext CreateChilianLiangyinContext(global::CardGame.Core.CardsMovedTriggerWindowFrame frame, global::CardGame.Core.ProgramTriggerCandidate candidate) => _resolve().CreateChilianLiangyinContext(frame, candidate);
    internal bool IsIssuedChilianLiangyinCandidate(global::CardGame.Core.ProgramTriggerCandidate candidate, global::CardGame.Core.ProgramSkillWindowContext context) => _resolve().IsIssuedChilianLiangyinCandidate(candidate, context);
    internal global::CardGame.Core.ProgramSkillWindowContext? TryCreateChilianLiangyinContext(global::CardGame.Core.CardsMovedTriggerWindowFrame frame, global::CardGame.Core.ProgramTriggerCandidate candidate) => _resolve().TryCreateChilianLiangyinContext(frame, candidate);
}
