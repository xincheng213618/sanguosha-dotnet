#nullable enable
namespace CardGame.Core.NativeOwners;
internal sealed class HuaManProgramCapability(Func<RuntimeEntries.CharacterHuaManOlEntries> getCharacterHuaManOlRuntime) : IHuaManProgramHost
{
    public SkillProgramStepOutcome ExecuteHuaMan(ProgramSkillFrame f, SkillProgramEffectOp op) => getCharacterHuaManOlRuntime().ExecuteHuaMan(f, op);
}
