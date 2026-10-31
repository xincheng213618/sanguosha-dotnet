#nullable enable
namespace CardGame.Core.NativeOwners;
internal sealed class LeCaiWenJiShuangjiaCapability(Func<RuntimeEntries.CharacterCaiWenJiLeEntries> getCharacterCaiWenJiLeRuntime) : ILeCaiWenJiShuangjiaHost
{
    public SkillProgramStepOutcome InitializeShuangjia(ProgramSkillFrame frame) => getCharacterCaiWenJiLeRuntime().InitializeShuangjia(frame);
}
