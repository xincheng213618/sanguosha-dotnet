#nullable enable
namespace CardGame.Core.NativeOwners;
internal sealed class WeiZiProgramCapability(Func<RuntimeEntries.CharacterWeiZiOlEntries> getCharacterWeiZiOlRuntime) : IWeiZiProgramHost
{
    public SkillProgramStepOutcome YuanziResolve(ProgramSkillFrame frame, SkillProgramEffect effect) => getCharacterWeiZiOlRuntime().YuanziProgramResolve(frame, effect);
    public SkillProgramStepOutcome YuanziDamageDraw(ProgramSkillFrame frame, SkillProgramEffect effect) => getCharacterWeiZiOlRuntime().YuanziProgramDamageDraw(frame, effect);
    public SkillProgramStepOutcome LiejieSourceDiscard(ProgramSkillFrame frame, SkillProgramEffect effect) => getCharacterWeiZiOlRuntime().LiejieProgramSourceDiscard(frame, effect);
}
