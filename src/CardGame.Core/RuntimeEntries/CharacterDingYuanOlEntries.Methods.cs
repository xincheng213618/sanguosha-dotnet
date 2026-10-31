#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CharacterDingYuanOlEntries
{
    internal int AdoptedChildSlashDamage(GameEngine.IDamageAttempt attack) => _resolve().AdoptedChildSlashDamage(attack);
    internal void AssignAdoptedChild(ProgramSkillFrame supplied) => _resolve().AssignAdoptedChild(supplied);
    internal bool CanContinueIssuedFirstRoundDamage(ProgramSkillFrame frame) => _resolve().CanContinueIssuedFirstRoundDamage(frame);
    internal bool CanOfferDingYuan(ProgramTriggerCandidate candidate, SkillProgramTrigger trigger, ProgramSkillWindowContext context) => _resolve().CanOfferDingYuan(candidate, trigger, context);
    internal void CaptureAdoptedChildSlashDamageIssue(GameEngine.IDamageAttempt attack) => _resolve().CaptureAdoptedChildSlashDamageIssue(attack);
    internal RoundFirstSourcedDamageEvent? FirstRoundDamageFor(ProgramSkillWindowContext context) => _resolve().FirstRoundDamageFor(context);
    internal bool IsAdoptedChildSlashDamage(GameEngine.IDamageAttempt attack, scoped out AdoptedChildSlashUseReceipt origin) => _resolve().IsAdoptedChildSlashDamage(attack, out origin);
    internal void ObserveDingYuanNativeFacts(IGameEvent payload) => _resolve().ObserveDingYuanNativeFacts(payload);
    internal void PrepareAdoptedChild(ProgramSkillFrame supplied, bool transfer) => _resolve().PrepareAdoptedChild(supplied, transfer);
    internal void QualifyFirstRoundDamage(ProgramSkillFrame supplied) => _resolve().QualifyFirstRoundDamage(supplied);
    internal void SynchronizeAdoptedChildSkillGrants() => _resolve().SynchronizeAdoptedChildSkillGrants();
    internal int? AdoptedChildHolder { get => _resolve().AdoptedChildHolder; }
    internal bool TracksDingYuan { get => _resolve().TracksDingYuan; }
    internal bool TracksRoundFirstDamage { get => _resolve().TracksRoundFirstDamage; }
}
