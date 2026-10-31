#nullable enable
namespace CardGame.Core.NativeOwners;
internal sealed class TengFangLanProgramCapability(Func<RuntimeEntries.DeferredRuntime<EngineOwners.CharacterTengFangLanOlRuntime>> getCharacterTengFangLanOlRuntime) : ITengFangLanProgramHost
{
    public SkillProgramStepOutcome LuochongResolve(ProgramSkillFrame frame, SkillProgramEffect effect) => getCharacterTengFangLanOlRuntime().Value.LuochongProgramResolve(frame, effect);
    public SkillProgramStepOutcome AichenRemoveOption(ProgramSkillFrame frame, SkillProgramEffect effect) => getCharacterTengFangLanOlRuntime().Value.AichenProgramRemoveOption(frame, effect);
}
