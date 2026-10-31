#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineWushuangDyingExitEntries
{
    internal bool IsWushuangDyingExitCandidate(global::CardGame.Core.ProgramTriggerCandidate c, int victim) => _resolve().IsWushuangDyingExitCandidate(c, victim);
    internal bool IsWushuangDyingExitContext(global::CardGame.Core.ProgramTriggerCandidate c, global::CardGame.Core.ProgramSkillWindowContext context) => _resolve().IsWushuangDyingExitContext(c, context);
    internal bool IsWushuangDyingExitReturn(global::CardGame.Core.ProgramSkillFrame f) => _resolve().IsWushuangDyingExitReturn(f);
}
