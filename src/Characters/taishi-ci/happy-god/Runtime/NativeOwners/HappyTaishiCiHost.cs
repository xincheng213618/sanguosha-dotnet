#nullable enable
namespace CardGame.Core.NativeOwners;
internal sealed class HappyTaishiCiHost(global::System.Func<global::CardGame.Core.RuntimeEntries.CharacterTaishiCiMobileGodEntries> getCharacterTaishiCiMobileGodRuntime, global::System.Func<global::CardGame.Core.RuntimeEntries.CharacterTaishiCiHappyGodEntries> getCharacterTaishiCiHappyGodRuntime) : IHappyTaishiCiHost
{
    public SkillProgramStepOutcome Execute(ProgramSkillFrame f, SkillProgramEffectOp op) => op switch
    {
        SkillProgramEffectOp.ResolveHappyTaishiCiChongwei => getCharacterTaishiCiMobileGodRuntime().ResolveShenTscPowei(f),
        SkillProgramEffectOp.RecoverHappyTaishiCiDulie => getCharacterTaishiCiHappyGodRuntime().RecoverHappyTsc(f),
        _ => getCharacterTaishiCiHappyGodRuntime().BeginHappyTsc(f, op)};
}
