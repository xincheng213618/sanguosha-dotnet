#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CharacterMaChaoAnniversaryGodEntries
{
    internal void AddShouliForeignSlashActions(global::System.Collections.Generic.List<global::CardGame.Core.LegalAction> actions, global::CardGame.Core.CharacterState actor, int? selectedPhysicalCardId) => _resolve().AddShouliForeignSlashActions(actions, actor, selectedPhysicalCardId);
    internal global::CardGame.Core.SkillProgramStepOutcome BeginShenMaChao(global::CardGame.Core.ProgramSkillFrame supplied, global::CardGame.Core.SkillProgramEffectOp op) => _resolve().BeginShenMaChao(supplied, op);
    internal bool CanHengwu(int owner, global::CardGame.Core.CardActionContext? a, global::CardGame.Core.SkillProgramTriggerWindow? window) => _resolve().CanHengwu(owner, a, window);
    internal bool CanTriggerHengwu(global::CardGame.Core.ProgramTriggerLegalityContext gate) => _resolve().CanTriggerHengwu(gate);
    internal void ContinueShenMaChao(long id) => _resolve().ContinueShenMaChao(id);
    internal void ContinueShouliStartingMountReturn() => _resolve().ContinueShouliStartingMountReturn();
    internal bool DrainShenMc(long id) => _resolve().DrainShenMc(id);
    internal global::CardGame.Core.CardLocation FindOwnedCardLocation(global::CardGame.Core.CharacterState actor, global::CardGame.Core.Card card, global::CardGame.Core.CardConversionSource? acceptedConversion) => _resolve().FindOwnedCardLocation(actor, card, acceptedConversion);
    internal global::CardGame.Core.Card? FindShouliFieldCard(global::CardGame.Core.CharacterState actor, int? id) => _resolve().FindShouliFieldCard(actor, id);
    internal bool HasShouliDamageDebt(int target) => _resolve().HasShouliDamageDebt(target);
    internal global::CardGame.Core.CardActionContext? HengwuAction(global::CardGame.Core.ProgramSkillWindowContext? context) => _resolve().HengwuAction(context);
    internal bool IsAcceptedShouliMaterial(global::CardGame.Core.CharacterState actor, global::CardGame.Core.Card card, global::CardGame.Core.CardConversionSource? source, bool response) => _resolve().IsAcceptedShouliMaterial(actor, card, source, response);
    internal bool IsShouliFieldMaterial(global::CardGame.Core.CharacterState actor, global::CardGame.Core.Card card, scoped out global::CardGame.Core.CardLocation location) => _resolve().IsShouliFieldMaterial(actor, card, out location);
    internal bool IsShouliStartingEquipmentRide(global::CardGame.Core.ResolutionFrame child, global::CardGame.Core.ResolutionFrame parent) => _resolve().IsShouliStartingEquipmentRide(child, parent);
    internal void IssueShouliBorrowedMountDebt(global::CardGame.Core.CardActionContext action) => _resolve().IssueShouliBorrowedMountDebt(action);
    internal bool ResumeShenMc(long id) => _resolve().ResumeShenMc(id);
    internal void ReturnHengwuDraw(global::CardGame.Core.ProgramSkillFrame supplied, global::CardGame.Core.NativeDrawOutcome o) => _resolve().ReturnHengwuDraw(supplied, o);
    internal bool ReturnShenMcMovement(global::CardGame.Core.ProgramSkillFrame f) => _resolve().ReturnShenMcMovement(f);
    internal global::System.Collections.Generic.IEnumerable<global::CardGame.Core.Card> ShouliFieldCards(global::CardGame.Core.CharacterState actor) => _resolve().ShouliFieldCards(actor);
    internal global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.CardConversionSource> ShouliFieldConversions(global::CardGame.Core.CharacterState actor, global::CardGame.Core.Card card, global::CardGame.Core.CardKind output, bool response, bool dyingUse) => _resolve().ShouliFieldConversions(actor, card, output, response, dyingUse);
    internal global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.Card> ShouliFieldResponseCards(global::CardGame.Core.CharacterState actor, global::CardGame.Core.CardKind requiredKind) => _resolve().ShouliFieldResponseCards(actor, requiredKind);
    internal global::System.Collections.Generic.IEnumerable<global::CardGame.Core.LegalAction> ShouliForeignSlashActions(global::CardGame.Core.CharacterState actor) => _resolve().ShouliForeignSlashActions(actor);
    internal bool ShouliIgnoresSlashLimit(global::CardGame.Core.CharacterState owner, global::CardGame.Core.CardConversionSource? source) => _resolve().ShouliIgnoresSlashLimit(owner, source);
    internal int ShouliReceivedDamageBonus(global::CardGame.Core.GameEngine.IDamageAttempt attack) => _resolve().ShouliReceivedDamageBonus(attack);
    internal global::System.Collections.Generic.IEnumerable<(global::CardGame.Core.CardConversionSource Source, global::CardGame.Core.SkillProgramViewAs Rule)> ShouliSources(global::CardGame.Core.CharacterState actor, global::CardGame.Core.Card card, bool response) => _resolve().ShouliSources(actor, card, response);
    internal global::CardGame.Core.DamageNature? ShouliThunderNature(global::CardGame.Core.GameEngine.IDamageAttempt attack) => _resolve().ShouliThunderNature(attack);
    internal bool TryShouliFieldLocation(global::CardGame.Core.CharacterState actor, global::CardGame.Core.Card card, scoped out global::CardGame.Core.CardLocation location) => _resolve().TryShouliFieldLocation(actor, card, out location);
    internal bool HasHengwuCapability { get => _resolve().HasHengwuCapability; }
}
