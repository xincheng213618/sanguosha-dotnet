#nullable enable
namespace CardGame.Core.NativeOwners;
internal sealed class RuanHuiCapability(Func<RuntimeEntries.CharacterRuanHuiOlEntries> getCharacterRuanHuiOlRuntime) : IRuanHuiHost
{
    public SkillProgramStepOutcome Resolve(ProgramSkillFrame f, SkillProgramEffectOp op) => getCharacterRuanHuiOlRuntime().BeginRuanHui(f, op);
}
