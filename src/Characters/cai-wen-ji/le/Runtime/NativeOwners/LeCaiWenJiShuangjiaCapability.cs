#nullable enable
namespace CardGame.Core.NativeOwners;
internal sealed class LeCaiWenJiShuangjiaCapability(global::System.Func<global::CardGame.Core.RuntimeEntries.CharacterCaiWenJiLeEntries> getCharacterCaiWenJiLeRuntime) : ILeCaiWenJiShuangjiaHost
{
    public SkillProgramStepOutcome InitializeShuangjia(ProgramSkillFrame frame) => getCharacterCaiWenJiLeRuntime().InitializeShuangjia(frame);
}
