#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineWushuangAuditAEntries
{
    internal bool AllowsWushuangBeigeCost(global::CardGame.Core.ProgramTriggerLegalityContext gate) => _resolve().AllowsWushuangBeigeCost(gate);
    internal bool AllowsWushuangTiaoxinPhase(global::CardGame.Core.ProgramActivationLegalityContext gate) => _resolve().AllowsWushuangTiaoxinPhase(gate);
    internal void RecordWushuangTiaoxinExpansion(global::CardGame.Core.ProgramSkillFrame supplied) => _resolve().RecordWushuangTiaoxinExpansion(supplied);
}
