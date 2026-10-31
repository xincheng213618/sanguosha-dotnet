#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineNearestLegalSlashRequestsEntries
{
    internal void AssertNearestLegalSlashState(global::CardGame.Core.ProgramSkillFrame frame, global::CardGame.Core.SkillProgramEffect paused) => _resolve().AssertNearestLegalSlashState(frame, paused);
    internal bool IsNearestFactionChoiceResult(global::CardGame.Core.ProgramSkillFrame frame, global::CardGame.Core.ProgramChoiceResultBinding binding) => _resolve().IsNearestFactionChoiceResult(frame, binding);
    internal bool IsNearestLegalFactionOrigin(global::CardGame.Core.ProgramSkillFrame frame, int actor, int target, string? bind) => _resolve().IsNearestLegalFactionOrigin(frame, actor, target, bind);
    internal bool IsValidNearestSlashProgramSelection(global::CardGame.Core.ProgramSkillFrame frame, global::CardGame.Core.SkillProgramEffect effect, global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.SkillProgramEffect> instructions) => _resolve().IsValidNearestSlashProgramSelection(frame, effect, instructions);
    internal global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.PromptChoice> NearestLegalSlashChoices(global::CardGame.Core.ProgramSkillFrame frame) => _resolve().NearestLegalSlashChoices(frame);
    internal global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.PromptChoice> NearestPhysicalSlashChoices(global::CardGame.Core.ProgramSkillFrame view) => _resolve().NearestPhysicalSlashChoices(view);
    internal bool NearestSlashPaymentHasRange(global::CardGame.Core.CharacterState actor, int target, global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.Card> cards, global::CardGame.Core.CardConversionSource? conversion, global::CardGame.Core.CardKind kind) => _resolve().NearestSlashPaymentHasRange(actor, target, cards, conversion, kind);
    internal global::CardGame.Core.SkillProgramStepOutcome OfferUnlimitedVirtualSlash(global::CardGame.Core.ProgramSkillFrame frame) => _resolve().OfferUnlimitedVirtualSlash(frame);
    internal global::CardGame.Core.SkillProgramStepOutcome RequestLegalNearestSlashes(global::CardGame.Core.SkillProgramEffect effect, global::CardGame.Core.ProgramSkillFrame frame) => _resolve().RequestLegalNearestSlashes(effect, frame);
    internal global::CardGame.Core.SkillProgramStepOutcome RequestLegalSlashByNearestActor(global::CardGame.Core.ProgramSkillFrame frame, int actorSeat, int hpAmount, bool trickEffectReturned = false) => _resolve().RequestLegalSlashByNearestActor(frame, actorSeat, hpAmount, trickEffectReturned);
    internal void ResolveNearestLegalSlashChoice(global::CardGame.Core.PromptChoice selected) => _resolve().ResolveNearestLegalSlashChoice(selected);
    internal void ResolveUnlimitedVirtualSlashChoice(global::CardGame.Core.PromptChoice selected) => _resolve().ResolveUnlimitedVirtualSlashChoice(selected);
    internal global::CardGame.Core.PromptChoice SelectAiNearestLegalSlash(global::CardGame.Core.PendingDecision decision) => _resolve().SelectAiNearestLegalSlash(decision);
    internal global::CardGame.Core.PromptChoice SelectAiUnlimitedSlash(global::CardGame.Core.PendingDecision decision) => _resolve().SelectAiUnlimitedSlash(decision);
    internal global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.PromptChoice> UnlimitedVirtualSlashChoices(global::CardGame.Core.ProgramSkillFrame frame) => _resolve().UnlimitedVirtualSlashChoices(frame);
    internal int[] UnlimitedVirtualSlashTargets(global::CardGame.Core.ProgramSkillFrame frame) => _resolve().UnlimitedVirtualSlashTargets(frame);
    internal bool ValidateNearestLegalFactionParent(global::CardGame.Core.NativeOwners.FactionCardRequestHandle pending, global::CardGame.Core.ProgramSkillFrame frame) => _resolve().ValidateNearestLegalFactionParent(pending, frame);
}
