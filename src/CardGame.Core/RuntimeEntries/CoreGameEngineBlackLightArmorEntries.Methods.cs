#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineBlackLightArmorEntries
{
    internal bool CanOfferBlackLightArmor(ProgramTriggerCandidate candidate, SkillProgramTrigger trigger, ProgramSkillWindowContext context) => _resolve().CanOfferBlackLightArmor(candidate, trigger, context);
}
