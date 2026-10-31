#nullable enable
namespace CardGame.Core.NativeOwners;
internal sealed class LeZhouYuPingluCapability(global::System.Func<global::CardGame.Core.RuntimeEntries.CharacterZhouYuLeEntries> getCharacterZhouYuLeRuntime) : ILeZhouYuPingluHost
{
    public SkillProgramStepOutcome Resolve(ProgramSkillFrame frame) => getCharacterZhouYuLeRuntime().BeginPinglu(frame);
}
