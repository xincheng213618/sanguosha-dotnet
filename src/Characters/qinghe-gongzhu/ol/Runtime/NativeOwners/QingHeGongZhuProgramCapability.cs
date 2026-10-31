#nullable enable
namespace CardGame.Core.NativeOwners;
internal sealed class QingHeGongZhuProgramCapability(global::System.Func<global::CardGame.Core.RuntimeEntries.CharacterQingheGongzhuOlEntries> getCharacterQingheGongzhuOlRuntime) : IQingHeGongZhuProgramHost
{
    public SkillProgramStepOutcome ChangjiEndingDamageChoice(ProgramSkillFrame frame, SkillProgramEffect effect) => getCharacterQingheGongzhuOlRuntime().ChangjiProgramEndingDamageChoice(frame, effect);
    public SkillProgramStepOutcome ZengouNullifyDodge(ProgramSkillFrame frame, SkillProgramEffect effect) => getCharacterQingheGongzhuOlRuntime().ZengouProgramNullifyDodge(frame, effect);
}
