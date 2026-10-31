#nullable enable
namespace CardGame.Core.NativeOwners;
internal sealed class LeCaiWenJiBeifenCapability(Func<RuntimeEntries.CharacterCaiWenJiLeEntries> getCharacterCaiWenJiLeRuntime) : ILeCaiWenJiBeifenHost
{
    public SkillProgramStepOutcome Resolve(ProgramSkillFrame frame) => getCharacterCaiWenJiLeRuntime().BeginBeifen(frame);
}
