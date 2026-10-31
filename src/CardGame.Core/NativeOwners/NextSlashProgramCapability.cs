#nullable enable
namespace CardGame.Core.NativeOwners;
internal sealed class NextSlashProgramCapability(global::System.Func<global::CardGame.Core.RuntimeEntries.CoreGameEngineNextSlashProgramsEntries> getCoreGameEngineNextSlashProgramsRuntime) : INextSlashProgramHost
{
    public void RecoverToMaximum(ProgramSkillFrame frame) => getCoreGameEngineNextSlashProgramsRuntime().RecoverProgramToMaximum(frame);
    public void DrawRecoveryReceipt(ProgramSkillFrame frame) => getCoreGameEngineNextSlashProgramsRuntime().DrawProgramRecoveryReceipt(frame);
    public void ReserveNextSlashDamage(ProgramSkillFrame frame) => getCoreGameEngineNextSlashProgramsRuntime().ReserveProgramNextSlashDamage(frame);
}
