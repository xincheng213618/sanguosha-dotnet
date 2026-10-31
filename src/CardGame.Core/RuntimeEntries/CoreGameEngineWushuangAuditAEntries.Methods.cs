#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineWushuangAuditAEntries
{
    internal bool AllowsWushuangBeigeCost(ProgramTriggerLegalityContext gate) => _resolve().AllowsWushuangBeigeCost(gate);
    internal bool AllowsWushuangTiaoxinPhase(ProgramActivationLegalityContext gate) => _resolve().AllowsWushuangTiaoxinPhase(gate);
    internal void RecordWushuangTiaoxinExpansion(ProgramSkillFrame supplied) => _resolve().RecordWushuangTiaoxinExpansion(supplied);
}
