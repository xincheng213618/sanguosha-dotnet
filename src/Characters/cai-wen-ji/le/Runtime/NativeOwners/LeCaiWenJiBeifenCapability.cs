#nullable enable
namespace CardGame.Core.NativeOwners;
internal sealed class LeCaiWenJiBeifenCapability(global::System.Func<global::CardGame.Core.RuntimeEntries.CharacterCaiWenJiLeEntries> getCharacterCaiWenJiLeRuntime) : ILeCaiWenJiBeifenHost
{
    public SkillProgramStepOutcome Resolve(ProgramSkillFrame frame) => getCharacterCaiWenJiLeRuntime().BeginBeifen(frame);
}
