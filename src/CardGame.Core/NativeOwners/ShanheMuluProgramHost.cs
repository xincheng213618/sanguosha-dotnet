#nullable enable
namespace CardGame.Core.NativeOwners;
internal sealed class ShanheMuluProgramHost(global::System.Func<global::CardGame.Core.RuntimeEntries.CoreGameEngineShanheMuluEntries> getCoreGameEngineShanheMuluRuntime) : IShanheMuluProgramHost
{
    public SkillProgramStepOutcome ReviveFollowers(ProgramSkillFrame frame) => getCoreGameEngineShanheMuluRuntime().BeginShanheMuluRevival(frame);
    public void FreezeRoundDamage(ProgramSkillFrame frame) => getCoreGameEngineShanheMuluRuntime().FreezeShanheElephantRoundDamage(frame);
}
