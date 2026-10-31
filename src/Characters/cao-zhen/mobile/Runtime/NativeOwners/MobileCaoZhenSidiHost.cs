#nullable enable
namespace CardGame.Core.NativeOwners;
internal sealed class MobileCaoZhenSidiHost(Func<RuntimeEntries.DeferredRuntime<EngineOwners.CharacterCaoZhenMobileRuntime>> getCharacterCaoZhenMobileRuntime) : IMobileCaoZhenSidiHost
{
    public SkillProgramStepOutcome Consume(ProgramSkillFrame f, string bind) => getCharacterCaoZhenMobileRuntime().Value.ConsumeMobileCaoZhenSidi(f, bind);
}
