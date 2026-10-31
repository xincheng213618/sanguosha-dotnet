#nullable enable
namespace CardGame.Core.NativeOwners;
internal sealed class MouZhuRongCapability(Func<RuntimeEntries.CharacterZhuRongMouMobileEntries> getCharacterZhuRongMouMobileRuntime) : IMouZhuRongHost, IMouZhuRongLierenTargets
{
    public IReadOnlyList<int> OtherTargets(int owner) => getCharacterZhuRongMouMobileRuntime().MouZhuRongLierenOtherTargets(owner);
    public void RecordLieren(ProgramSkillFrame f) => getCharacterZhuRongMouMobileRuntime().RecordMouZhuRongLieren(f);
    public SkillProgramStepOutcome SelectLierenTarget(ProgramSkillFrame f) => getCharacterZhuRongMouMobileRuntime().SelectMouZhuRongLierenTarget(f);
    public SkillProgramStepOutcome GiftAssault(ProgramSkillFrame f) => getCharacterZhuRongMouMobileRuntime().GiftMouZhuRongOutsideAssault(f);
}
