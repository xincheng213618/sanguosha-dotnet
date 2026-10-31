#nullable enable
namespace CardGame.Core.NativeOwners;
internal sealed class XinPiProgramCapability(Func<RuntimeEntries.CharacterXinPiOlEntries> getCharacterXinPiOlRuntime) : IXinPiProgramHost
{
    public SkillProgramStepOutcome BeginXinPi(ProgramSkillFrame f, SkillProgramEffect e) => getCharacterXinPiOlRuntime().BeginXinPi(f, e);
}
