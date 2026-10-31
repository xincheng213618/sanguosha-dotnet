#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineAlcoholIdentityPoliciesEntries
{
    internal bool CanRunProgramDyingAlcoholPolicy(SkillProgramTrigger trigger, ProgramSkillWindowContext context, int responder) => _resolve().CanRunProgramDyingAlcoholPolicy(trigger, context, responder);
    internal bool CanUseProgramDyingAlcohol(ProgramSkillFrame frame, SkillProgramEffectOp op) => _resolve().CanUseProgramDyingAlcohol(frame, op);
    internal int? CaptureAlcoholActionRank(int providerSeat, CardKind kind, IReadOnlyList<int> ids) => _resolve().CaptureAlcoholActionRank(providerSeat, kind, ids);
    internal int? CaptureAlcoholIdentityRank(CharacterState owner, Card card) => _resolve().CaptureAlcoholIdentityRank(owner, card);
    internal int? CaptureAlcoholPindianRank(PindianFrame frame, CharacterState owner, Card card, bool source, bool top) => _resolve().CaptureAlcoholPindianRank(frame, owner, card, source, top);
    internal int? CaptureAlcoholResponseRank(CharacterState provider, CardKind kind, IReadOnlyList<CardActionCost> costs) => _resolve().CaptureAlcoholResponseRank(provider, kind, costs);
    internal ProgramDyingAlcoholPermission? CaptureProgramDyingAlcoholPermission(ProgramTriggerCandidate candidate, ProgramSkillWindowContext context) => _resolve().CaptureProgramDyingAlcoholPermission(candidate, context);
    internal int EffectiveOwnedCardRank(CharacterState owner, Card card) => _resolve().EffectiveOwnedCardRank(owner, card);
    internal bool HasAlcoholKingIdentityPolicy(CharacterState owner) => _resolve().HasAlcoholKingIdentityPolicy(owner);
    internal bool HasFrozenProgramDyingAlcoholPermission(ProgramSkillFrame frame, SkillProgramEffectOp op) => _resolve().HasFrozenProgramDyingAlcoholPermission(frame, op);
    internal bool IsAlcoholIdentityOwnedLocation(CharacterState owner, Card card) => _resolve().IsAlcoholIdentityOwnedLocation(owner, card);
    internal bool IsEffectiveHandKindExempt(CharacterState owner, Card card, IReadOnlySet<CardKind> kinds) => _resolve().IsEffectiveHandKindExempt(owner, card, kinds);
    internal bool IsForeignTurnAlcoholUseForbidden(int actorSeat, CardKind kind, CardActionType type) => _resolve().IsForeignTurnAlcoholUseForbidden(actorSeat, kind, type);
}
