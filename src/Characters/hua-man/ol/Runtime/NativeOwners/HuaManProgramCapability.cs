#nullable enable
namespace CardGame.Core.NativeOwners;
internal sealed class HuaManProgramCapability(global::System.Func<global::CardGame.Core.RuntimeEntries.CharacterHuaManOlEntries> getCharacterHuaManOlRuntime) : IHuaManProgramHost
{
    public SkillProgramStepOutcome ExecuteHuaMan(ProgramSkillFrame f, SkillProgramEffectOp op) => getCharacterHuaManOlRuntime().ExecuteHuaMan(f, op);
}
