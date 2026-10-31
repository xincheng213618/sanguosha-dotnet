#nullable enable
namespace CardGame.Core.NativeOwners;
internal sealed class HappyTaishiCiHost(Func<RuntimeEntries.CharacterTaishiCiMobileGodEntries> getCharacterTaishiCiMobileGodRuntime, Func<RuntimeEntries.CharacterTaishiCiHappyGodEntries> getCharacterTaishiCiHappyGodRuntime) : IHappyTaishiCiHost
{
    public SkillProgramStepOutcome Execute(ProgramSkillFrame f, SkillProgramEffectOp op) => op switch
    {
        SkillProgramEffectOp.ResolveHappyTaishiCiChongwei => getCharacterTaishiCiMobileGodRuntime().ResolveShenTscPowei(f),
        SkillProgramEffectOp.RecoverHappyTaishiCiDulie => getCharacterTaishiCiHappyGodRuntime().RecoverHappyTsc(f),
        _ => getCharacterTaishiCiHappyGodRuntime().BeginHappyTsc(f, op)};
}
