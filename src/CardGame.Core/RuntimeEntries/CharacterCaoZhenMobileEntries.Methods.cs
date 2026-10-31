#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CharacterCaoZhenMobileEntries
{
    internal global::CardGame.Core.SkillProgramStepOutcome ConsumeMobileCaoZhenSidi(global::CardGame.Core.ProgramSkillFrame supplied, string bind) => _resolve().ConsumeMobileCaoZhenSidi(supplied, bind);
    internal bool MobileCaoZhenDodgeQualification(global::CardGame.Core.ProgramTriggerLegalityContext gate) => _resolve().MobileCaoZhenDodgeQualification(gate);
    internal global::System.Collections.Generic.IEnumerable<global::CardGame.Core.RuleQueryContribution> MobileCaoZhenSidiContributions(global::CardGame.Core.CharacterState player, global::CardGame.Core.SkillRuleQuery query) => _resolve().MobileCaoZhenSidiContributions(player, query);
}
