#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CharacterZhugeJinMobileEntries
{
    internal bool CanOfferMobileZhugeJinMingzhe(global::CardGame.Core.ProgramTriggerLegalityContext gate) => _resolve().CanOfferMobileZhugeJinMingzhe(gate);
    internal bool KeepPaidMobileZhugeJinHongyuan(global::CardGame.Core.ProgramSkillFrame f) => _resolve().KeepPaidMobileZhugeJinHongyuan(f);
    internal int[] OrderMobileZhugeJinHongyuanTargets(global::CardGame.Core.ProgramSkillFrame frame, int[] targets) => _resolve().OrderMobileZhugeJinHongyuanTargets(frame, targets);
    internal bool HasMobileMingzheRegistration { get => _resolve().HasMobileMingzheRegistration; }
}
