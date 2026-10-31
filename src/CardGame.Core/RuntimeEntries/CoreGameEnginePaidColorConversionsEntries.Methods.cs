#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEnginePaidColorConversionsEntries
{
    internal void AddPaidColorEquipmentDuelActions(global::System.Collections.Generic.List<global::CardGame.Core.LegalAction> actions, global::CardGame.Core.CharacterState owner, int? selectedPhysicalCardId = default) => _resolve().AddPaidColorEquipmentDuelActions(actions, owner, selectedPhysicalCardId);
    internal global::CardGame.Core.SkillProgramStepOutcome AwaitPaidColorDamageClaimMovements(long id) => _resolve().AwaitPaidColorDamageClaimMovements(id);
    internal global::CardGame.Core.SkillProgramStepOutcome BeginPaidColorConversion(global::CardGame.Core.ProgramSkillFrame frame, string bind) => _resolve().BeginPaidColorConversion(frame, bind);
    internal global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.CardConversionSource> GetPaidColorTurnDuelConversions(global::CardGame.Core.CharacterState owner, global::CardGame.Core.Card card, global::CardGame.Core.CardKind kind, bool forResponse, global::CardGame.Core.CardZoneKind zone) => _resolve().GetPaidColorTurnDuelConversions(owner, card, kind, forResponse, zone);
    internal bool IsActualPaidColorMovement(int owner, global::CardGame.Core.ProgramPaidColorConversionOrigin p) => _resolve().IsActualPaidColorMovement(owner, p);
    internal bool IsPaidColorEquipmentDuelAction(global::CardGame.Core.CharacterState owner, global::CardGame.Core.LegalAction action, global::CardGame.Core.Card card) => _resolve().IsPaidColorEquipmentDuelAction(owner, action, card);
    internal bool MatchesPaidColorDrawOwner(global::CardGame.Core.ProgramSkillFrame root) => _resolve().MatchesPaidColorDrawOwner(root);
    internal bool ResumePaidColorDamageClaims(long id) => _resolve().ResumePaidColorDamageClaims(id);
    internal bool TracksPaidColorUseAppearance(int actorSeat, global::CardGame.Core.CardKind kind, global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.CardConversionSource> chain) => _resolve().TracksPaidColorUseAppearance(actorSeat, kind, chain);
}
