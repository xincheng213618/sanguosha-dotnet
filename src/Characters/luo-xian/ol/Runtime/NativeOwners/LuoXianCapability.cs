#nullable enable
namespace CardGame.Core.NativeOwners;
internal sealed class LuoXianCapability(Func<RuntimeEntries.CharacterLuoXianOlEntries> getCharacterLuoXianOlRuntime) : ILuoXianProgramHost
{
    public SkillProgramStepOutcome ResolveDailiTurnEnd(ProgramSkillFrame frame) => getCharacterLuoXianOlRuntime().ResolveDailiTurnEnd(frame);
}
