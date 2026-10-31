#nullable enable
namespace CardGame.Core.NativeOwners;
internal sealed class MobileDongBaiHost(global::System.Func<global::CardGame.Core.RuntimeEntries.CharacterDongBaiMobileEntries> getCharacterDongBaiMobileRuntime) : IMobileDongBaiHost
{
    public SkillProgramStepOutcome Execute(ProgramSkillFrame f, SkillProgramEffect effect) => effect.Op switch
    {
        SkillProgramEffectOp.PrepareMobileDongBaiLianzhu => getCharacterDongBaiMobileRuntime().PrepareMobileLianzhu(f, effect.SourceBind),
        SkillProgramEffectOp.ResolveMobileDongBaiLianzhuRelay => getCharacterDongBaiMobileRuntime().BeginMobileLianzhuRelay(f),
        _ => getCharacterDongBaiMobileRuntime().RecoverMobileLianzhu(f)};
}
