#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineAlcoholIdentityPoliciesEntries
{
    internal bool CanRunProgramDyingAlcoholPolicy(global::CardGame.Core.SkillProgramTrigger trigger, global::CardGame.Core.ProgramSkillWindowContext context, int responder) => _resolve().CanRunProgramDyingAlcoholPolicy(trigger, context, responder);
    internal bool CanUseProgramDyingAlcohol(global::CardGame.Core.ProgramSkillFrame frame, global::CardGame.Core.SkillProgramEffectOp op) => _resolve().CanUseProgramDyingAlcohol(frame, op);
    internal int? CaptureAlcoholActionRank(int providerSeat, global::CardGame.Core.CardKind kind, global::System.Collections.Generic.IReadOnlyList<int> ids) => _resolve().CaptureAlcoholActionRank(providerSeat, kind, ids);
    internal int? CaptureAlcoholIdentityRank(global::CardGame.Core.CharacterState owner, global::CardGame.Core.Card card) => _resolve().CaptureAlcoholIdentityRank(owner, card);
    internal int? CaptureAlcoholPindianRank(global::CardGame.Core.PindianFrame frame, global::CardGame.Core.CharacterState owner, global::CardGame.Core.Card card, bool source, bool top) => _resolve().CaptureAlcoholPindianRank(frame, owner, card, source, top);
    internal int? CaptureAlcoholResponseRank(global::CardGame.Core.CharacterState provider, global::CardGame.Core.CardKind kind, global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.CardActionCost> costs) => _resolve().CaptureAlcoholResponseRank(provider, kind, costs);
    internal global::CardGame.Core.ProgramDyingAlcoholPermission? CaptureProgramDyingAlcoholPermission(global::CardGame.Core.ProgramTriggerCandidate candidate, global::CardGame.Core.ProgramSkillWindowContext context) => _resolve().CaptureProgramDyingAlcoholPermission(candidate, context);
    internal int EffectiveOwnedCardRank(global::CardGame.Core.CharacterState owner, global::CardGame.Core.Card card) => _resolve().EffectiveOwnedCardRank(owner, card);
    internal bool HasAlcoholKingIdentityPolicy(global::CardGame.Core.CharacterState owner) => _resolve().HasAlcoholKingIdentityPolicy(owner);
    internal bool HasFrozenProgramDyingAlcoholPermission(global::CardGame.Core.ProgramSkillFrame frame, global::CardGame.Core.SkillProgramEffectOp op) => _resolve().HasFrozenProgramDyingAlcoholPermission(frame, op);
    internal bool IsAlcoholIdentityOwnedLocation(global::CardGame.Core.CharacterState owner, global::CardGame.Core.Card card) => _resolve().IsAlcoholIdentityOwnedLocation(owner, card);
    internal bool IsEffectiveHandKindExempt(global::CardGame.Core.CharacterState owner, global::CardGame.Core.Card card, global::System.Collections.Generic.IReadOnlySet<global::CardGame.Core.CardKind> kinds) => _resolve().IsEffectiveHandKindExempt(owner, card, kinds);
    internal bool IsForeignTurnAlcoholUseForbidden(int actorSeat, global::CardGame.Core.CardKind kind, global::CardGame.Core.CardActionType type) => _resolve().IsForeignTurnAlcoholUseForbidden(actorSeat, kind, type);
}
