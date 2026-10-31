#nullable enable
namespace CardGame.Core.NativeOwners;
internal sealed class LeZouShiCapability(Func<RuntimeEntries.CharacterZouShiLeEntries> getCharacterZouShiLeRuntime) : ILeZouShiHost
{
    public SkillProgramStepOutcome InitializeYunzheng(ProgramSkillFrame frame) => getCharacterZouShiLeRuntime().InitializeYunzheng(frame);
    public SkillProgramStepOutcome ResolveHuoxin(ProgramSkillFrame frame) => getCharacterZouShiLeRuntime().BeginHuoxin(frame);
}
