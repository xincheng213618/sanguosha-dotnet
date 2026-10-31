#nullable enable
namespace CardGame.Core.NativeOwners;
internal sealed class LeZouShiCapability(global::System.Func<global::CardGame.Core.RuntimeEntries.CharacterZouShiLeEntries> getCharacterZouShiLeRuntime) : ILeZouShiHost
{
    public SkillProgramStepOutcome InitializeYunzheng(ProgramSkillFrame frame) => getCharacterZouShiLeRuntime().InitializeYunzheng(frame);
    public SkillProgramStepOutcome ResolveHuoxin(ProgramSkillFrame frame) => getCharacterZouShiLeRuntime().BeginHuoxin(frame);
}
