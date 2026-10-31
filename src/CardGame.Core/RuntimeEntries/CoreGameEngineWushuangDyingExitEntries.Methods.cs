#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineWushuangDyingExitEntries
{
    internal bool IsWushuangDyingExitCandidate(ProgramTriggerCandidate c, int victim) => _resolve().IsWushuangDyingExitCandidate(c, victim);
    internal bool IsWushuangDyingExitContext(ProgramTriggerCandidate c, ProgramSkillWindowContext context) => _resolve().IsWushuangDyingExitContext(c, context);
    internal bool IsWushuangDyingExitReturn(ProgramSkillFrame f) => _resolve().IsWushuangDyingExitReturn(f);
}
