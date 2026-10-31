#nullable enable
namespace CardGame.Core.NativeOwners;
internal sealed class TurnRedSlashProgramCapability(global::System.Func<global::CardGame.Core.RuntimeEntries.CoreGameEngineTurnRedSlashBenefitsEntries> getCoreGameEngineTurnRedSlashBenefitsRuntime) : ITurnRedSlashProgramHost
{
    public void GrantTurnRedSlashBenefits(ProgramSkillFrame frame) => getCoreGameEngineTurnRedSlashBenefitsRuntime().GrantTurnRedSlashBenefits(frame);
}
