#nullable enable
namespace CardGame.Core.NativeOwners;
internal sealed class HappySimaYiHost(global::System.Func<global::CardGame.Core.RuntimeEntries.CharacterSimaYiHappyGodEntries> getCharacterSimaYiHappyGodRuntime) : IHappySimaYiHost
{
    public SkillProgramStepOutcome Execute(ProgramSkillFrame f, SkillProgramEffectOp op) => op == SkillProgramEffectOp.GainHappySimaRenjie ? getCharacterSimaYiHappyGodRuntime().GainHappySimaRen(f) : getCharacterSimaYiHappyGodRuntime().BeginHappySimaFirstDraw(f);
}
