#nullable enable
namespace CardGame.Core.NativeOwners;
internal sealed class MouHuangYueyingMobileCapability(Func<RuntimeEntries.CharacterHuangYueyingMouMobileEntries> getCharacterHuangYueyingMouMobileRuntime) : IMouHuangYueyingMobileHost, IMouHuangYueyingMobileQuery
{
    public bool CanPlace(int owner, int target) => getCharacterHuangYueyingMouMobileRuntime().MouHyEquipmentChoices(owner, target).Any();
    public SkillProgramStepOutcome Resolve(ProgramSkillFrame f, SkillProgramEffectOp op) => op switch
    {
        SkillProgramEffectOp.ResolveMouHuangYueyingJizhi => getCharacterHuangYueyingMouMobileRuntime().BeginMouHyJizhi(f),
        SkillProgramEffectOp.PlaceMouHuangYueyingQicaiEquipment => getCharacterHuangYueyingMouMobileRuntime().BeginMouHyQicai(f),
        _ => getCharacterHuangYueyingMouMobileRuntime().BeginMouHyQiTransfer(f)};
}
