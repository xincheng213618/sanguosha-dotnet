#nullable enable
namespace CardGame.Core.NativeOwners;
internal sealed class PublicPileCashOutCapability(Func<RuntimeEntries.CoreGameEnginePublicPileCashOutEntries> getCoreGameEnginePublicPileCashOutRuntime) : IPublicPileCashOutHost
{
    public SkillProgramStepOutcome ExecutePublicPileCashOut(SkillProgramEffect effect, ProgramSkillFrame frame) => getCoreGameEnginePublicPileCashOutRuntime().BeginPublicPileCashOut(effect, frame);
}
