#nullable enable
namespace CardGame.Core.NativeOwners;
internal sealed class LeCaoZhiFuleCapability(Func<RuntimeEntries.CharacterCaoZhiLeEntries> getCharacterCaoZhiLeRuntime) : ILeCaoZhiFuleHost
{
    public SkillProgramStepOutcome Initialize(ProgramSkillFrame frame) => getCharacterCaoZhiLeRuntime().InitializeLeCaoZhiFule(frame);
}
