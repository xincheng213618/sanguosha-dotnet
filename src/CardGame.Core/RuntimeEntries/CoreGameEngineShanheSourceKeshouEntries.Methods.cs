#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineShanheSourceKeshouEntries
{
    internal void AnswerSourceKeshou(global::CardGame.Core.PromptChoice selected) => _resolve().AnswerSourceKeshou(selected);
    internal void AssertSourceKeshou(global::CardGame.Core.ProgramSkillFrame f) => _resolve().AssertSourceKeshou(f);
    internal bool CanOfferSourceKeshou(global::CardGame.Core.ProgramTriggerLegalityContext c) => _resolve().CanOfferSourceKeshou(c);
    internal bool HasSourceKeshouPair(int owner) => _resolve().HasSourceKeshouPair(owner);
    internal bool KeepPaidSourceKeshou(global::CardGame.Core.ProgramSkillFrame f) => _resolve().KeepPaidSourceKeshou(f);
    internal global::CardGame.Core.SkillProgramStepOutcome ResolveSourceKeshou(global::CardGame.Core.ProgramSkillFrame supplied, string step) => _resolve().ResolveSourceKeshou(supplied, step);
    internal global::CardGame.Core.PromptChoice SelectAiSourceKeshou(global::CardGame.Core.PendingDecision p, global::CardGame.Core.ProgramSkillFrame f) => _resolve().SelectAiSourceKeshou(p, f);
    internal global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.PromptChoice> SourceKeshouChoices(global::CardGame.Core.ProgramSkillFrame f) => _resolve().SourceKeshouChoices(f);
    internal global::System.Collections.Generic.IEnumerable<global::CardGame.Core.SourceKeshouMaterial> SourceKeshouEligibleMaterials(int owner) => _resolve().SourceKeshouEligibleMaterials(owner);
    internal global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.SourceKeshouMaterial[]> SourceKeshouPairs(int owner) => _resolve().SourceKeshouPairs(owner);
    internal bool SourceKeshouWasPaid(global::CardGame.Core.ProgramSkillFrame f) => _resolve().SourceKeshouWasPaid(f);
    internal bool ValidSourceKeshou(global::CardGame.Core.ProgramSkillFrame f) => _resolve().ValidSourceKeshou(f);
}
