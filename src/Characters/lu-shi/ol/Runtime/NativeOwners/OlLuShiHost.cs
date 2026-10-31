#nullable enable
namespace CardGame.Core.NativeOwners;
internal sealed class OlLuShiHost(global::System.Func<global::CardGame.Core.RuntimeEntries.CharacterLuShiOlEntries> getCharacterLuShiOlRuntime) : IOlLuShiHost
{
    public SkillProgramStepOutcome Begin(ProgramSkillFrame f, SkillProgramEffectOp op) => getCharacterLuShiOlRuntime().BeginOlLuShi(f, op);
}
