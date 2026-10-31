#nullable enable
namespace CardGame.Core.NativeOwners;
internal sealed class ShanheMuluProgramHost(Func<RuntimeEntries.CoreGameEngineShanheMuluEntries> getCoreGameEngineShanheMuluRuntime) : IShanheMuluProgramHost
{
    public SkillProgramStepOutcome ReviveFollowers(ProgramSkillFrame frame) => getCoreGameEngineShanheMuluRuntime().BeginShanheMuluRevival(frame);
    public void FreezeRoundDamage(ProgramSkillFrame frame) => getCoreGameEngineShanheMuluRuntime().FreezeShanheElephantRoundDamage(frame);
}
