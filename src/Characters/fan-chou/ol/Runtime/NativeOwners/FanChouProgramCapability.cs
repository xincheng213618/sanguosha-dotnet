#nullable enable
namespace CardGame.Core.NativeOwners;
internal sealed class FanChouProgramCapability(Func<RuntimeEntries.CharacterFanChouOlEntries> getCharacterFanChouOlRuntime) : IFanChouProgramHost
{
    public SkillProgramStepOutcome BeginFanChou(ProgramSkillFrame f, SkillProgramEffect e) => getCharacterFanChouOlRuntime().BeginFanChou(f, e);
}
