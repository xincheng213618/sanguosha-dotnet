#nullable enable
namespace CardGame.Core.NativeOwners;
internal sealed class LeCaoZhiFuleCapability(global::System.Func<global::CardGame.Core.RuntimeEntries.CharacterCaoZhiLeEntries> getCharacterCaoZhiLeRuntime) : ILeCaoZhiFuleHost
{
    public SkillProgramStepOutcome Initialize(ProgramSkillFrame frame) => getCharacterCaoZhiLeRuntime().InitializeLeCaoZhiFule(frame);
}
