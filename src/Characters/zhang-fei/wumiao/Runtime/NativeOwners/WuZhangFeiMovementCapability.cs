#nullable enable
namespace CardGame.Core.NativeOwners;
internal sealed class WuZhangFeiMovementCapability(global::System.Func<global::CardGame.Core.RuntimeEntries.CharacterZhangFeiWumiaoEntries> getCharacterZhangFeiWumiaoRuntime) : IWuZhangFeiMovementHost
{
    public SkillProgramStepOutcome ResolveMovementDamage(ProgramSkillFrame frame) => getCharacterZhangFeiWumiaoRuntime().ResolveWuZishengMovement(frame);
}
