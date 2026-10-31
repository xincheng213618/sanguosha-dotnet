#nullable enable
namespace CardGame.Core.NativeOwners;
internal sealed class SoleIncomingEffectCapability(global::System.Func<global::CardGame.Core.RuntimeEntries.CoreGameEngineSoleIncomingEffectRepeatEntries> getCoreGameEngineSoleIncomingEffectRepeatRuntime) : ISoleIncomingEffectRepeatHost
{
    public void IssueSoleIncomingEffect(ProgramSkillFrame frame, int maximumDrawCount) => getCoreGameEngineSoleIncomingEffectRepeatRuntime().IssueSoleIncomingEffect(frame, maximumDrawCount);
}
