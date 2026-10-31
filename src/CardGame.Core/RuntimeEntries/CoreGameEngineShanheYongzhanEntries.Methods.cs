#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineShanheYongzhanEntries
{
    internal bool IsShanheYongzhanDyingExitCandidate(global::CardGame.Core.ProgramTriggerCandidate candidate, int victim) => _resolve().IsShanheYongzhanDyingExitCandidate(candidate, victim);
    internal bool IsShanheYongzhanDyingExitContext(global::CardGame.Core.ProgramTriggerCandidate candidate, global::CardGame.Core.ProgramSkillWindowContext context) => _resolve().IsShanheYongzhanDyingExitContext(candidate, context);
    internal bool IsShanheYongzhanDyingExitIdentity(global::CardGame.Core.ProgramTriggerCandidate candidate, int victim) => _resolve().IsShanheYongzhanDyingExitIdentity(candidate, victim);
    internal bool IsShanheYongzhanDyingExitReturn(global::CardGame.Core.ProgramSkillFrame frame) => _resolve().IsShanheYongzhanDyingExitReturn(frame);
}
