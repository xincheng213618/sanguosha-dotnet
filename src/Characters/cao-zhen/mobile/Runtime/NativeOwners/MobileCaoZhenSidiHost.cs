#nullable enable
namespace CardGame.Core.NativeOwners;
internal sealed class MobileCaoZhenSidiHost(Func<RuntimeEntries.CharacterCaoZhenMobileEntries> getCharacterCaoZhenMobileRuntime) : IMobileCaoZhenSidiHost
{
    public SkillProgramStepOutcome Consume(ProgramSkillFrame f, string bind) => getCharacterCaoZhenMobileRuntime().ConsumeMobileCaoZhenSidi(f, bind);
}
