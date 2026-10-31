#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CharacterCaoZhenMobileEntries
{
    internal SkillProgramStepOutcome ConsumeMobileCaoZhenSidi(ProgramSkillFrame supplied, string bind) => _resolve().ConsumeMobileCaoZhenSidi(supplied, bind);
    internal bool MobileCaoZhenDodgeQualification(ProgramTriggerLegalityContext gate) => _resolve().MobileCaoZhenDodgeQualification(gate);
    internal IEnumerable<RuleQueryContribution> MobileCaoZhenSidiContributions(CharacterState player, SkillRuleQuery query) => _resolve().MobileCaoZhenSidiContributions(player, query);
}
