#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CharacterDingYuanOlEntries
{
    internal int AdoptedChildSlashDamage(global::CardGame.Core.GameEngine.IDamageAttempt attack) => _resolve().AdoptedChildSlashDamage(attack);
    internal void AssertAdoptedChildRelationshipFrame(global::CardGame.Core.ProgramSkillFrame frame) => _resolve().AssertAdoptedChildRelationshipFrame(frame);
    internal void AssignAdoptedChild(global::CardGame.Core.ProgramSkillFrame supplied) => _resolve().AssignAdoptedChild(supplied);
    internal bool CanContinueIssuedFirstRoundDamage(global::CardGame.Core.ProgramSkillFrame frame) => _resolve().CanContinueIssuedFirstRoundDamage(frame);
    internal bool CanOfferDingYuan(global::CardGame.Core.ProgramTriggerCandidate candidate, global::CardGame.Core.SkillProgramTrigger trigger, global::CardGame.Core.ProgramSkillWindowContext context) => _resolve().CanOfferDingYuan(candidate, trigger, context);
    internal void CaptureAdoptedChildSlashDamageIssue(global::CardGame.Core.GameEngine.IDamageAttempt attack) => _resolve().CaptureAdoptedChildSlashDamageIssue(attack);
    internal global::CardGame.Core.RoundFirstSourcedDamageEvent? FirstRoundDamageFor(global::CardGame.Core.ProgramSkillWindowContext context) => _resolve().FirstRoundDamageFor(context);
    internal bool IsAdoptedChildSlashDamage(global::CardGame.Core.GameEngine.IDamageAttempt attack, scoped out global::CardGame.Core.AdoptedChildSlashUseReceipt origin) => _resolve().IsAdoptedChildSlashDamage(attack, out origin);
    internal void ObserveDingYuanNativeFacts(global::CardGame.Core.IGameEvent payload) => _resolve().ObserveDingYuanNativeFacts(payload);
    internal void PrepareAdoptedChild(global::CardGame.Core.ProgramSkillFrame supplied, bool transfer) => _resolve().PrepareAdoptedChild(supplied, transfer);
    internal void QualifyFirstRoundDamage(global::CardGame.Core.ProgramSkillFrame supplied) => _resolve().QualifyFirstRoundDamage(supplied);
    internal void SynchronizeAdoptedChildSkillGrants() => _resolve().SynchronizeAdoptedChildSkillGrants();
    internal int? AdoptedChildHolder { get => _resolve().AdoptedChildHolder; }
    internal bool TracksDingYuan { get => _resolve().TracksDingYuan; }
    internal bool TracksRoundFirstDamage { get => _resolve().TracksRoundFirstDamage; }
}
