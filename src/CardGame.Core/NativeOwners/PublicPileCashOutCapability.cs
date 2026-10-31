#nullable enable
namespace CardGame.Core.NativeOwners;
internal sealed class PublicPileCashOutCapability(global::System.Func<global::CardGame.Core.RuntimeEntries.CoreGameEnginePublicPileCashOutEntries> getCoreGameEnginePublicPileCashOutRuntime) : IPublicPileCashOutHost
{
    public SkillProgramStepOutcome ExecutePublicPileCashOut(SkillProgramEffect effect, ProgramSkillFrame frame) => getCoreGameEnginePublicPileCashOutRuntime().BeginPublicPileCashOut(effect, frame);
}
