#nullable enable
namespace CardGame.Core.NativeOwners;
internal sealed class ShenJwHost(Func<RuntimeEntries.CharacterJiangWeiAnniversaryGodEntries> getCharacterJiangWeiAnniversaryGodRuntime) : IShenJiangWeiHost
{
    public SkillProgramStepOutcome Execute(ProgramSkillFrame f, SkillProgramEffectOp op) => getCharacterJiangWeiAnniversaryGodRuntime().BeginShenJw(f, op);
}
