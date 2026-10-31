#nullable enable
namespace CardGame.Core.NativeOwners;
internal sealed class MobileShiXieHost(global::System.Func<global::CardGame.Core.RuntimeEntries.CharacterShiXieMobileEntries> getCharacterShiXieMobileRuntime) : IMobileShiXieHost
{
    public SkillProgramStepOutcome Change(ProgramSkillFrame frame, SkillProgramEffectOp op) => getCharacterShiXieMobileRuntime().ChangeMobileShiXieDistance(frame, op);
}
