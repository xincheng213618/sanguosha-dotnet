#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineNearestLegalSlashRequestsEntries
{
    internal bool IsNearestFactionChoiceResult(ProgramSkillFrame frame, ProgramChoiceResultBinding binding) => _resolve().IsNearestFactionChoiceResult(frame, binding);
    internal bool IsNearestLegalFactionOrigin(ProgramSkillFrame frame, int actor, int target, string? bind) => _resolve().IsNearestLegalFactionOrigin(frame, actor, target, bind);
    internal IReadOnlyList<PromptChoice> NearestLegalSlashChoices(ProgramSkillFrame frame) => _resolve().NearestLegalSlashChoices(frame);
    internal IReadOnlyList<PromptChoice> NearestPhysicalSlashChoices(ProgramSkillFrame view) => _resolve().NearestPhysicalSlashChoices(view);
    internal bool NearestSlashPaymentHasRange(CharacterState actor, int target, IReadOnlyList<Card> cards, CardConversionSource? conversion, CardKind kind) => _resolve().NearestSlashPaymentHasRange(actor, target, cards, conversion, kind);
    internal SkillProgramStepOutcome OfferUnlimitedVirtualSlash(ProgramSkillFrame frame) => _resolve().OfferUnlimitedVirtualSlash(frame);
    internal SkillProgramStepOutcome RequestLegalNearestSlashes(SkillProgramEffect effect, ProgramSkillFrame frame) => _resolve().RequestLegalNearestSlashes(effect, frame);
    internal SkillProgramStepOutcome RequestLegalSlashByNearestActor(ProgramSkillFrame frame, int actorSeat, int hpAmount, bool trickEffectReturned = false) => _resolve().RequestLegalSlashByNearestActor(frame, actorSeat, hpAmount, trickEffectReturned);
    internal void ResolveNearestLegalSlashChoice(PromptChoice selected) => _resolve().ResolveNearestLegalSlashChoice(selected);
    internal void ResolveUnlimitedVirtualSlashChoice(PromptChoice selected) => _resolve().ResolveUnlimitedVirtualSlashChoice(selected);
    internal PromptChoice SelectAiNearestLegalSlash(PendingDecision decision) => _resolve().SelectAiNearestLegalSlash(decision);
    internal PromptChoice SelectAiUnlimitedSlash(PendingDecision decision) => _resolve().SelectAiUnlimitedSlash(decision);
    internal IReadOnlyList<PromptChoice> UnlimitedVirtualSlashChoices(ProgramSkillFrame frame) => _resolve().UnlimitedVirtualSlashChoices(frame);
    internal int[] UnlimitedVirtualSlashTargets(ProgramSkillFrame frame) => _resolve().UnlimitedVirtualSlashTargets(frame);
    internal bool ValidateNearestLegalFactionParent(NativeOwners.FactionCardRequestHandle pending, ProgramSkillFrame frame) => _resolve().ValidateNearestLegalFactionParent(pending, frame);
}
