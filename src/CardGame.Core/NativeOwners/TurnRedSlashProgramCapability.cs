#nullable enable
namespace CardGame.Core.NativeOwners;
internal sealed class TurnRedSlashProgramCapability(Func<RuntimeEntries.CoreGameEngineTurnRedSlashBenefitsEntries> getCoreGameEngineTurnRedSlashBenefitsRuntime) : ITurnRedSlashProgramHost
{
    public void GrantTurnRedSlashBenefits(ProgramSkillFrame frame) => getCoreGameEngineTurnRedSlashBenefitsRuntime().GrantTurnRedSlashBenefits(frame);
}
