#nullable enable
namespace CardGame.Core.NativeOwners;
internal sealed class ShanheS5HuashenCapability(Func<RuntimeEntries.CoreGameEngineShanheS5HuashenEntries> getCoreGameEngineShanheS5HuashenRuntime) : IShanheS5HuashenHost
{
    public SkillProgramStepOutcome ChooseOrExchangeShanheS5Avatar(SkillProgramEffect effect, ProgramSkillFrame frame) => getCoreGameEngineShanheS5HuashenRuntime().ChooseOrExchangeShanheS5Avatar(effect, frame);
}
