#nullable enable
namespace CardGame.Core.NativeOwners;
internal sealed class ShenJwHost(global::System.Func<global::CardGame.Core.RuntimeEntries.CharacterJiangWeiAnniversaryGodEntries> getCharacterJiangWeiAnniversaryGodRuntime) : IShenJiangWeiHost
{
    public SkillProgramStepOutcome Execute(ProgramSkillFrame f, SkillProgramEffectOp op) => getCharacterJiangWeiAnniversaryGodRuntime().BeginShenJw(f, op);
}
