#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CharacterZhugeJinMobileEntries
{
    internal bool CanOfferMobileZhugeJinMingzhe(ProgramTriggerLegalityContext gate) => _resolve().CanOfferMobileZhugeJinMingzhe(gate);
    internal bool KeepPaidMobileZhugeJinHongyuan(ProgramSkillFrame f) => _resolve().KeepPaidMobileZhugeJinHongyuan(f);
    internal int[] OrderMobileZhugeJinHongyuanTargets(ProgramSkillFrame frame, int[] targets) => _resolve().OrderMobileZhugeJinHongyuanTargets(frame, targets);
    internal bool HasMobileMingzheRegistration { get => _resolve().HasMobileMingzheRegistration; }
}
