#nullable enable
namespace CardGame.Core.NativeOwners;
internal sealed class WuZhangFeiMovementCapability(Func<RuntimeEntries.CharacterZhangFeiWumiaoEntries> getCharacterZhangFeiWumiaoRuntime) : IWuZhangFeiMovementHost
{
    public SkillProgramStepOutcome ResolveMovementDamage(ProgramSkillFrame frame) => getCharacterZhangFeiWumiaoRuntime().ResolveWuZishengMovement(frame);
}
