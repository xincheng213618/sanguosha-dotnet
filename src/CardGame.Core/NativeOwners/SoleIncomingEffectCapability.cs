#nullable enable
namespace CardGame.Core.NativeOwners;
internal sealed class SoleIncomingEffectCapability(Func<RuntimeEntries.CoreGameEngineSoleIncomingEffectRepeatEntries> getCoreGameEngineSoleIncomingEffectRepeatRuntime) : ISoleIncomingEffectRepeatHost
{
    public void IssueSoleIncomingEffect(ProgramSkillFrame frame, int maximumDrawCount) => getCoreGameEngineSoleIncomingEffectRepeatRuntime().IssueSoleIncomingEffect(frame, maximumDrawCount);
}
