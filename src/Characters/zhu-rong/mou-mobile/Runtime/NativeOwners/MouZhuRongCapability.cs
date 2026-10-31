#nullable enable
namespace CardGame.Core.NativeOwners;
internal sealed class MouZhuRongCapability(Func<RuntimeEntries.DeferredRuntime<EngineOwners.CharacterZhuRongMouMobileRuntime>> getCharacterZhuRongMouMobileRuntime) : IMouZhuRongHost, IMouZhuRongLierenTargets
{
    public IReadOnlyList<int> OtherTargets(int owner) => getCharacterZhuRongMouMobileRuntime().Value.MouZhuRongLierenOtherTargets(owner);
    public void RecordLieren(ProgramSkillFrame f) => getCharacterZhuRongMouMobileRuntime().Value.RecordMouZhuRongLieren(f);
    public SkillProgramStepOutcome SelectLierenTarget(ProgramSkillFrame f) => getCharacterZhuRongMouMobileRuntime().Value.SelectMouZhuRongLierenTarget(f);
    public SkillProgramStepOutcome GiftAssault(ProgramSkillFrame f) => getCharacterZhuRongMouMobileRuntime().Value.GiftMouZhuRongOutsideAssault(f);
}
