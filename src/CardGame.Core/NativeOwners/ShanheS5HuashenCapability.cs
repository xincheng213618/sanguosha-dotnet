#nullable enable
namespace CardGame.Core.NativeOwners;
internal sealed class ShanheS5HuashenCapability(global::System.Func<global::CardGame.Core.RuntimeEntries.CoreGameEngineShanheS5HuashenEntries> getCoreGameEngineShanheS5HuashenRuntime) : IShanheS5HuashenHost
{
    public SkillProgramStepOutcome ChooseOrExchangeShanheS5Avatar(SkillProgramEffect effect, ProgramSkillFrame frame) => getCoreGameEngineShanheS5HuashenRuntime().ChooseOrExchangeShanheS5Avatar(effect, frame);
}
