#nullable enable
namespace CardGame.Core.NativeOwners;
internal sealed class HappySimaYiHost(Func<RuntimeEntries.DeferredRuntime<EngineOwners.CharacterSimaYiHappyGodRuntime>> getCharacterSimaYiHappyGodRuntime) : IHappySimaYiHost
{
    public SkillProgramStepOutcome Execute(ProgramSkillFrame f, SkillProgramEffectOp op) => op == SkillProgramEffectOp.GainHappySimaRenjie ? getCharacterSimaYiHappyGodRuntime().Value.GainHappySimaRen(f) : getCharacterSimaYiHappyGodRuntime().Value.BeginHappySimaFirstDraw(f);
}
