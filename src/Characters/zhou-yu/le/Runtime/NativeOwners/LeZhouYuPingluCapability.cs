#nullable enable
namespace CardGame.Core.NativeOwners;
internal sealed class LeZhouYuPingluCapability(Func<RuntimeEntries.CharacterZhouYuLeEntries> getCharacterZhouYuLeRuntime) : ILeZhouYuPingluHost
{
    public SkillProgramStepOutcome Resolve(ProgramSkillFrame frame) => getCharacterZhouYuLeRuntime().BeginPinglu(frame);
}
