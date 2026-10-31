#nullable enable
namespace CardGame.Core.NativeOwners;
internal sealed class MouGuquCapability(global::System.Func<global::CardGame.Core.RuntimeEntries.CharacterZhouYuOlMouEntries> getCharacterZhouYuOlMouRuntime) : IMouZhouYuGuquHost
{
    public SkillProgramStepOutcome Resolve(ProgramSkillFrame frame) => getCharacterZhouYuOlMouRuntime().BeginMouGuqu(frame);
}
