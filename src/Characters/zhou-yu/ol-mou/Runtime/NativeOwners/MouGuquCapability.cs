#nullable enable
namespace CardGame.Core.NativeOwners;
internal sealed class MouGuquCapability(Func<RuntimeEntries.CharacterZhouYuOlMouEntries> getCharacterZhouYuOlMouRuntime) : IMouZhouYuGuquHost
{
    public SkillProgramStepOutcome Resolve(ProgramSkillFrame frame) => getCharacterZhouYuOlMouRuntime().BeginMouGuqu(frame);
}
