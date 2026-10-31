#nullable enable
namespace CardGame.Core.NativeOwners;
internal sealed class ShanheS5HuashenCapability(Func<RuntimeEntries.DeferredRuntime<EngineOwners.CoreGameEngineShanheS5HuashenRuntime>> getCoreGameEngineShanheS5HuashenRuntime) : IShanheS5HuashenHost
{
    public SkillProgramStepOutcome ChooseOrExchangeShanheS5Avatar(SkillProgramEffect effect, ProgramSkillFrame frame) => getCoreGameEngineShanheS5HuashenRuntime().Value.ChooseOrExchangeShanheS5Avatar(effect, frame);
}
