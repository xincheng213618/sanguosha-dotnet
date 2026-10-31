#nullable enable
namespace CardGame.Core.NativeOwners;
internal sealed class YintianCapability(global::System.Func<global::CardGame.Core.RuntimeEntries.CharacterZhaoZhongOlEntries> getCharacterZhaoZhongOlRuntime) : IYintianHost
{
    public SkillProgramStepOutcome Execute(ProgramSkillFrame frame) => getCharacterZhaoZhongOlRuntime().ResolveYintianFirstRecovery(frame);
}
