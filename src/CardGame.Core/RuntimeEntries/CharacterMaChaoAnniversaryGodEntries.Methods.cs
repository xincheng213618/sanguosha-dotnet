#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CharacterMaChaoAnniversaryGodEntries
{
    internal void AddShouliForeignSlashActions(List<LegalAction> actions, CharacterState actor, int? selectedPhysicalCardId) => _resolve().AddShouliForeignSlashActions(actions, actor, selectedPhysicalCardId);
    internal SkillProgramStepOutcome BeginShenMaChao(ProgramSkillFrame supplied, SkillProgramEffectOp op) => _resolve().BeginShenMaChao(supplied, op);
    internal bool CanHengwu(int owner, CardActionContext? a, SkillProgramTriggerWindow? window) => _resolve().CanHengwu(owner, a, window);
    internal bool CanTriggerHengwu(ProgramTriggerLegalityContext gate) => _resolve().CanTriggerHengwu(gate);
    internal void ContinueShenMaChao(long id) => _resolve().ContinueShenMaChao(id);
    internal void ContinueShouliStartingMountReturn() => _resolve().ContinueShouliStartingMountReturn();
    internal bool DrainShenMc(long id) => _resolve().DrainShenMc(id);
    internal CardLocation FindOwnedCardLocation(CharacterState actor, Card card, CardConversionSource? acceptedConversion) => _resolve().FindOwnedCardLocation(actor, card, acceptedConversion);
    internal Card? FindShouliFieldCard(CharacterState actor, int? id) => _resolve().FindShouliFieldCard(actor, id);
    internal bool HasShouliDamageDebt(int target) => _resolve().HasShouliDamageDebt(target);
    internal CardActionContext? HengwuAction(ProgramSkillWindowContext? context) => _resolve().HengwuAction(context);
    internal bool IsAcceptedShouliMaterial(CharacterState actor, Card card, CardConversionSource? source, bool response) => _resolve().IsAcceptedShouliMaterial(actor, card, source, response);
    internal bool IsShouliFieldMaterial(CharacterState actor, Card card, scoped out CardLocation location) => _resolve().IsShouliFieldMaterial(actor, card, out location);
    internal bool IsShouliStartingEquipmentRide(ResolutionFrame child, ResolutionFrame parent) => _resolve().IsShouliStartingEquipmentRide(child, parent);
    internal void IssueShouliBorrowedMountDebt(CardActionContext action) => _resolve().IssueShouliBorrowedMountDebt(action);
    internal bool ResumeShenMc(long id) => _resolve().ResumeShenMc(id);
    internal void ReturnHengwuDraw(ProgramSkillFrame supplied, NativeDrawOutcome o) => _resolve().ReturnHengwuDraw(supplied, o);
    internal bool ReturnShenMcMovement(ProgramSkillFrame f) => _resolve().ReturnShenMcMovement(f);
    internal IEnumerable<Card> ShouliFieldCards(CharacterState actor) => _resolve().ShouliFieldCards(actor);
    internal IReadOnlyList<CardConversionSource> ShouliFieldConversions(CharacterState actor, Card card, CardKind output, bool response, bool dyingUse) => _resolve().ShouliFieldConversions(actor, card, output, response, dyingUse);
    internal IReadOnlyList<Card> ShouliFieldResponseCards(CharacterState actor, CardKind requiredKind) => _resolve().ShouliFieldResponseCards(actor, requiredKind);
    internal IEnumerable<LegalAction> ShouliForeignSlashActions(CharacterState actor) => _resolve().ShouliForeignSlashActions(actor);
    internal bool ShouliIgnoresSlashLimit(CharacterState owner, CardConversionSource? source) => _resolve().ShouliIgnoresSlashLimit(owner, source);
    internal int ShouliReceivedDamageBonus(GameEngine.IDamageAttempt attack) => _resolve().ShouliReceivedDamageBonus(attack);
    internal IEnumerable<(CardConversionSource Source, SkillProgramViewAs Rule)> ShouliSources(CharacterState actor, Card card, bool response) => _resolve().ShouliSources(actor, card, response);
    internal DamageNature? ShouliThunderNature(GameEngine.IDamageAttempt attack) => _resolve().ShouliThunderNature(attack);
    internal bool TryShouliFieldLocation(CharacterState actor, Card card, scoped out CardLocation location) => _resolve().TryShouliFieldLocation(actor, card, out location);
    internal bool HasHengwuCapability { get => _resolve().HasHengwuCapability; }
}
