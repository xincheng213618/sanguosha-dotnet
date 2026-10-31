#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineShanheYongzhanEntries
{
    internal bool IsShanheYongzhanDyingExitCandidate(ProgramTriggerCandidate candidate, int victim) => _resolve().IsShanheYongzhanDyingExitCandidate(candidate, victim);
    internal bool IsShanheYongzhanDyingExitContext(ProgramTriggerCandidate candidate, ProgramSkillWindowContext context) => _resolve().IsShanheYongzhanDyingExitContext(candidate, context);
    internal bool IsShanheYongzhanDyingExitIdentity(ProgramTriggerCandidate candidate, int victim) => _resolve().IsShanheYongzhanDyingExitIdentity(candidate, victim);
    internal bool IsShanheYongzhanDyingExitReturn(ProgramSkillFrame frame) => _resolve().IsShanheYongzhanDyingExitReturn(frame);
}
