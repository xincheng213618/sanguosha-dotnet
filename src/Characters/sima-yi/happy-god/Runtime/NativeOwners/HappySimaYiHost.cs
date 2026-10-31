#nullable enable
namespace CardGame.Core.NativeOwners;
internal sealed class HappySimaYiHost(Func<RuntimeEntries.CharacterSimaYiHappyGodEntries> getCharacterSimaYiHappyGodRuntime) : IHappySimaYiHost
{
    public SkillProgramStepOutcome Execute(ProgramSkillFrame f, SkillProgramEffectOp op) => op == SkillProgramEffectOp.GainHappySimaRenjie ? getCharacterSimaYiHappyGodRuntime().GainHappySimaRen(f) : getCharacterSimaYiHappyGodRuntime().BeginHappySimaFirstDraw(f);
}
