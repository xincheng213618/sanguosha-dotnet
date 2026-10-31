#nullable enable
namespace CardGame.Core.NativeOwners;
internal sealed class HappyTaishiCiHost(Func<RuntimeEntries.CharacterTaishiCiMobileGodEntries> getCharacterTaishiCiMobileGodRuntime, Func<RuntimeEntries.DeferredRuntime<EngineOwners.CharacterTaishiCiHappyGodRuntime>> getCharacterTaishiCiHappyGodRuntime) : IHappyTaishiCiHost
{
    public SkillProgramStepOutcome Execute(ProgramSkillFrame f, SkillProgramEffectOp op) => op switch
    {
        SkillProgramEffectOp.ResolveHappyTaishiCiChongwei => getCharacterTaishiCiMobileGodRuntime().ResolveShenTscPowei(f),
        SkillProgramEffectOp.RecoverHappyTaishiCiDulie => getCharacterTaishiCiHappyGodRuntime().Value.RecoverHappyTsc(f),
        _ => getCharacterTaishiCiHappyGodRuntime().Value.BeginHappyTsc(f, op)};
}
