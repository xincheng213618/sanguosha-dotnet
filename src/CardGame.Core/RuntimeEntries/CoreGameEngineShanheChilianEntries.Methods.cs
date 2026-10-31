#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineShanheChilianEntries
{
    internal IReadOnlyList<int> ChilianLiangyinTargets(int ownerSeat, string bindingId) => _resolve().ChilianLiangyinTargets(ownerSeat, bindingId);
    internal void CollectChilianLiangyinCandidates(CardMovementBatchContext batch, List<ProgramTriggerCandidate> candidates) => _resolve().CollectChilianLiangyinCandidates(batch, candidates);
    internal ProgramSkillWindowContext CreateChilianLiangyinContext(CardsMovedTriggerWindowFrame frame, ProgramTriggerCandidate candidate) => _resolve().CreateChilianLiangyinContext(frame, candidate);
    internal bool IsIssuedChilianLiangyinCandidate(ProgramTriggerCandidate candidate, ProgramSkillWindowContext context) => _resolve().IsIssuedChilianLiangyinCandidate(candidate, context);
    internal ProgramSkillWindowContext? TryCreateChilianLiangyinContext(CardsMovedTriggerWindowFrame frame, ProgramTriggerCandidate candidate) => _resolve().TryCreateChilianLiangyinContext(frame, candidate);
}
