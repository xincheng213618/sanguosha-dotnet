#nullable enable
namespace CardGame.Core.NativeOwners;
internal sealed class SoleIncomingEffectCapability(Func<RuntimeEntries.DeferredRuntime<EngineOwners.CoreGameEngineSoleIncomingEffectRepeatRuntime>> getCoreGameEngineSoleIncomingEffectRepeatRuntime) : ISoleIncomingEffectRepeatHost
{
    public void IssueSoleIncomingEffect(ProgramSkillFrame frame, int maximumDrawCount) => getCoreGameEngineSoleIncomingEffectRepeatRuntime().Value.IssueSoleIncomingEffect(frame, maximumDrawCount);
}
