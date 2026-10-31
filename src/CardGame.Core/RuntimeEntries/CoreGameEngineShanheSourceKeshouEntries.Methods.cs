#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineShanheSourceKeshouEntries
{
    internal void AnswerSourceKeshou(PromptChoice selected) => _resolve().AnswerSourceKeshou(selected);
    internal bool CanOfferSourceKeshou(ProgramTriggerLegalityContext c) => _resolve().CanOfferSourceKeshou(c);
    internal bool HasSourceKeshouPair(int owner) => _resolve().HasSourceKeshouPair(owner);
    internal bool KeepPaidSourceKeshou(ProgramSkillFrame f) => _resolve().KeepPaidSourceKeshou(f);
    internal SkillProgramStepOutcome ResolveSourceKeshou(ProgramSkillFrame supplied, string step) => _resolve().ResolveSourceKeshou(supplied, step);
    internal PromptChoice SelectAiSourceKeshou(PendingDecision p, ProgramSkillFrame f) => _resolve().SelectAiSourceKeshou(p, f);
    internal IReadOnlyList<PromptChoice> SourceKeshouChoices(ProgramSkillFrame f) => _resolve().SourceKeshouChoices(f);
    internal IEnumerable<SourceKeshouMaterial> SourceKeshouEligibleMaterials(int owner) => _resolve().SourceKeshouEligibleMaterials(owner);
    internal IReadOnlyList<SourceKeshouMaterial[]> SourceKeshouPairs(int owner) => _resolve().SourceKeshouPairs(owner);
    internal bool SourceKeshouWasPaid(ProgramSkillFrame f) => _resolve().SourceKeshouWasPaid(f);
    internal bool ValidSourceKeshou(ProgramSkillFrame f) => _resolve().ValidSourceKeshou(f);
}
