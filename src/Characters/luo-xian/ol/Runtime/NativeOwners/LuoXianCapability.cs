#nullable enable
namespace CardGame.Core.NativeOwners;
internal sealed class LuoXianCapability(global::System.Func<global::CardGame.Core.RuntimeEntries.CharacterLuoXianOlEntries> getCharacterLuoXianOlRuntime) : ILuoXianProgramHost
{
    public SkillProgramStepOutcome ResolveDailiTurnEnd(ProgramSkillFrame frame) => getCharacterLuoXianOlRuntime().ResolveDailiTurnEnd(frame);
}
