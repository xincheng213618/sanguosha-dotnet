#nullable enable
namespace CardGame.Core.NativeOwners;
internal sealed class RuanYuCapability(Func<RuntimeEntries.CharacterRuanYuOlEntries> getCharacterRuanYuOlRuntime) : IRuanYuHost
{
    public SkillProgramStepOutcome Resolve(ProgramSkillFrame f, SkillProgramEffectOp op) => getCharacterRuanYuOlRuntime().BeginRuanYu(f, op);
}
