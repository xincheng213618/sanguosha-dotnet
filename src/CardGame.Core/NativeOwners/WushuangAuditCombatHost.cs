#nullable enable
namespace CardGame.Core.NativeOwners;
internal sealed class WushuangAuditCombatHost(Func<RuntimeEntries.DeferredRuntime<EngineOwners.CoreGameEngineWushuangAuditCombatRuntime>> getCoreGameEngineWushuangAuditCombatRuntime) : IWushuangAuditCombatHost
{
    public SkillProgramStepOutcome Execute(ProgramSkillFrame frame, SkillProgramEffectOp op) => op == SkillProgramEffectOp.ClaimWushuangEndedCampHand ? getCoreGameEngineWushuangAuditCombatRuntime().Value.ClaimWushuangCampHand(frame) : getCoreGameEngineWushuangAuditCombatRuntime().Value.CaptureWushuangWangong(frame);
}
