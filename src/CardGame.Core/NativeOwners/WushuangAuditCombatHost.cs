#nullable enable
namespace CardGame.Core.NativeOwners;
internal sealed class WushuangAuditCombatHost(Func<RuntimeEntries.CoreGameEngineWushuangAuditCombatEntries> getCoreGameEngineWushuangAuditCombatRuntime) : IWushuangAuditCombatHost
{
    public SkillProgramStepOutcome Execute(ProgramSkillFrame frame, SkillProgramEffectOp op) => op == SkillProgramEffectOp.ClaimWushuangEndedCampHand ? getCoreGameEngineWushuangAuditCombatRuntime().ClaimWushuangCampHand(frame) : getCoreGameEngineWushuangAuditCombatRuntime().CaptureWushuangWangong(frame);
}
