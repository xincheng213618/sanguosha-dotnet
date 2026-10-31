#nullable enable
namespace CardGame.Core.NativeOwners;
internal sealed class MobileWuYiBenxiHost(global::System.Func<global::CardGame.Core.RuntimeEntries.CharacterWuYiMobileEntries> getCharacterWuYiMobileRuntime) : IMobileWuYiBenxiHost
{
    public SkillProgramStepOutcome Apply(ProgramSkillFrame frame) => getCharacterWuYiMobileRuntime().ApplyMobileWuYiBenxi(frame);
}
