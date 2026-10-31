#nullable enable
namespace CardGame.Core.NativeOwners;
internal sealed class MobileWuYiBenxiHost(Func<RuntimeEntries.CharacterWuYiMobileEntries> getCharacterWuYiMobileRuntime) : IMobileWuYiBenxiHost
{
    public SkillProgramStepOutcome Apply(ProgramSkillFrame frame) => getCharacterWuYiMobileRuntime().ApplyMobileWuYiBenxi(frame);
}
