#nullable enable
namespace CardGame.Core.NativeOwners;
internal sealed class ShanheMuluProgramHost(Func<RuntimeEntries.DeferredRuntime<EngineOwners.CoreGameEngineShanheMuluRuntime>> getCoreGameEngineShanheMuluRuntime) : IShanheMuluProgramHost
{
    public SkillProgramStepOutcome ReviveFollowers(ProgramSkillFrame frame) => getCoreGameEngineShanheMuluRuntime().Value.BeginShanheMuluRevival(frame);
    public void FreezeRoundDamage(ProgramSkillFrame frame) => getCoreGameEngineShanheMuluRuntime().Value.FreezeShanheElephantRoundDamage(frame);
}
