#nullable enable
namespace CardGame.Core.NativeOwners;
internal sealed class XinPiProgramCapability(global::System.Func<global::CardGame.Core.RuntimeEntries.CharacterXinPiOlEntries> getCharacterXinPiOlRuntime) : IXinPiProgramHost
{
    public SkillProgramStepOutcome BeginXinPi(ProgramSkillFrame f, SkillProgramEffect e) => getCharacterXinPiOlRuntime().BeginXinPi(f, e);
}
