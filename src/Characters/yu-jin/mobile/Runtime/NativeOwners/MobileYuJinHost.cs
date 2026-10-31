#nullable enable
namespace CardGame.Core.NativeOwners;
internal sealed class MobileYuJinHost(Func<RuntimeEntries.DeferredRuntime<EngineOwners.CharacterYuJinMobileRuntime>> getCharacterYuJinMobileRuntime) : IMobileYuJinHost
{
    public SkillProgramStepOutcome Transfer(ProgramSkillFrame frame, SkillProgramEffect effect) => getCharacterYuJinMobileRuntime().Value.BeginMobileYuJinTransfer(frame, effect);
}
