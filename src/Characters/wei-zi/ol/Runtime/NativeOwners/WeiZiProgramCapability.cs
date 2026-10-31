#nullable enable
namespace CardGame.Core.NativeOwners;
internal sealed class WeiZiProgramCapability(Func<RuntimeEntries.DeferredRuntime<EngineOwners.CharacterWeiZiOlRuntime>> getCharacterWeiZiOlRuntime) : IWeiZiProgramHost
{
    public SkillProgramStepOutcome YuanziResolve(ProgramSkillFrame frame, SkillProgramEffect effect) => getCharacterWeiZiOlRuntime().Value.YuanziProgramResolve(frame, effect);
    public SkillProgramStepOutcome YuanziDamageDraw(ProgramSkillFrame frame, SkillProgramEffect effect) => getCharacterWeiZiOlRuntime().Value.YuanziProgramDamageDraw(frame, effect);
    public SkillProgramStepOutcome LiejieSourceDiscard(ProgramSkillFrame frame, SkillProgramEffect effect) => getCharacterWeiZiOlRuntime().Value.LiejieProgramSourceDiscard(frame, effect);
}
