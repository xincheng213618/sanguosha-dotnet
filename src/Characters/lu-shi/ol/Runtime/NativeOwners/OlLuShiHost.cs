#nullable enable
namespace CardGame.Core.NativeOwners;
internal sealed class OlLuShiHost(Func<RuntimeEntries.CharacterLuShiOlEntries> getCharacterLuShiOlRuntime) : IOlLuShiHost
{
    public SkillProgramStepOutcome Begin(ProgramSkillFrame f, SkillProgramEffectOp op) => getCharacterLuShiOlRuntime().BeginOlLuShi(f, op);
}
