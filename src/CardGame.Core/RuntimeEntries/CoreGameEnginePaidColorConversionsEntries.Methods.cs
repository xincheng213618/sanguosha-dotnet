#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEnginePaidColorConversionsEntries
{
    internal void AddPaidColorEquipmentDuelActions(List<LegalAction> actions, CharacterState owner, int? selectedPhysicalCardId = default) => _resolve().AddPaidColorEquipmentDuelActions(actions, owner, selectedPhysicalCardId);
    internal SkillProgramStepOutcome AwaitPaidColorDamageClaimMovements(long id) => _resolve().AwaitPaidColorDamageClaimMovements(id);
    internal SkillProgramStepOutcome BeginPaidColorConversion(ProgramSkillFrame frame, string bind) => _resolve().BeginPaidColorConversion(frame, bind);
    internal IReadOnlyList<CardConversionSource> GetPaidColorTurnDuelConversions(CharacterState owner, Card card, CardKind kind, bool forResponse, CardZoneKind zone) => _resolve().GetPaidColorTurnDuelConversions(owner, card, kind, forResponse, zone);
    internal bool IsActualPaidColorMovement(int owner, ProgramPaidColorConversionOrigin p) => _resolve().IsActualPaidColorMovement(owner, p);
    internal bool IsPaidColorEquipmentDuelAction(CharacterState owner, LegalAction action, Card card) => _resolve().IsPaidColorEquipmentDuelAction(owner, action, card);
    internal bool MatchesPaidColorDrawOwner(ProgramSkillFrame root) => _resolve().MatchesPaidColorDrawOwner(root);
    internal bool ResumePaidColorDamageClaims(long id) => _resolve().ResumePaidColorDamageClaims(id);
    internal bool TracksPaidColorUseAppearance(int actorSeat, CardKind kind, IReadOnlyList<CardConversionSource> chain) => _resolve().TracksPaidColorUseAppearance(actorSeat, kind, chain);
}
