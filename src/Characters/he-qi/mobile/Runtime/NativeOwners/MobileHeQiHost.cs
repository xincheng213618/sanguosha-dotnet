#nullable enable
namespace CardGame.Core.NativeOwners;
internal sealed class MobileHeQiHost(Func<RuntimeEntries.DeferredRuntime<EngineOwners.CharacterHeQiMobileRuntime>> getCharacterHeQiMobileRuntime) : IMobileHeQiHost
{
    public SkillProgramStepOutcome View(ProgramSkillFrame f) => getCharacterHeQiMobileRuntime().Value.BeginMobileHeQiView(f);
}
