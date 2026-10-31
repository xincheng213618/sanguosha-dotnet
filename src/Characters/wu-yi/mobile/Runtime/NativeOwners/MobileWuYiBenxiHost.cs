#nullable enable
namespace CardGame.Core.NativeOwners;
internal sealed class MobileWuYiBenxiHost(Func<RuntimeEntries.DeferredRuntime<EngineOwners.CharacterWuYiMobileRuntime>> getCharacterWuYiMobileRuntime) : IMobileWuYiBenxiHost
{
    public SkillProgramStepOutcome Apply(ProgramSkillFrame frame) => getCharacterWuYiMobileRuntime().Value.ApplyMobileWuYiBenxi(frame);
}
