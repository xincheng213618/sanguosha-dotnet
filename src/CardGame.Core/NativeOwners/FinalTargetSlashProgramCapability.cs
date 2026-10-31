#nullable enable
namespace CardGame.Core.NativeOwners;
internal sealed class FinalTargetSlashProgramCapability(Func<RuntimeEntries.CoreGameEngineFinalTargetSlashEntries> getCoreGameEngineFinalTargetSlashRuntime) : IFinalTargetSlashProgramHost
{
    public void IssueFinalTargetSlashReceipt(ProgramSkillFrame frame, SkillProgramEffect effect) => getCoreGameEngineFinalTargetSlashRuntime().IssueFinalTargetSlashReceipt(frame, effect);
}
