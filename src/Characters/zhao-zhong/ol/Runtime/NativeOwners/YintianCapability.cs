#nullable enable
namespace CardGame.Core.NativeOwners;
internal sealed class YintianCapability(Func<RuntimeEntries.CharacterZhaoZhongOlEntries> getCharacterZhaoZhongOlRuntime) : IYintianHost
{
    public SkillProgramStepOutcome Execute(ProgramSkillFrame frame) => getCharacterZhaoZhongOlRuntime().ResolveYintianFirstRecovery(frame);
}
