#nullable enable
namespace CardGame.Core.NativeOwners;
internal sealed class TengFangLanProgramCapability(global::System.Func<global::CardGame.Core.RuntimeEntries.CharacterTengFangLanOlEntries> getCharacterTengFangLanOlRuntime) : ITengFangLanProgramHost
{
    public SkillProgramStepOutcome LuochongResolve(ProgramSkillFrame frame, SkillProgramEffect effect) => getCharacterTengFangLanOlRuntime().LuochongProgramResolve(frame, effect);
    public SkillProgramStepOutcome AichenRemoveOption(ProgramSkillFrame frame, SkillProgramEffect effect) => getCharacterTengFangLanOlRuntime().AichenProgramRemoveOption(frame, effect);
}
