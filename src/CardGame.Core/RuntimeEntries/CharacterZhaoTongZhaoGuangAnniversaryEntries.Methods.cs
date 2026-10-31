#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CharacterZhaoTongZhaoGuangAnniversaryEntries
{
    internal bool AdvanceAnniversaryQingrenChildren() => _resolve().AdvanceAnniversaryQingrenChildren();
    internal bool AnniversaryQingrenDyingObserver() => _resolve().AnniversaryQingrenDyingObserver();
    internal bool AnniversaryQingrenFirstChild(ProgramSkillFrame f, ResolutionFrame child) => _resolve().AnniversaryQingrenFirstChild(f, child);
    internal ProgramSkillFrame? AnniversaryQingrenObserverRoot() => _resolve().AnniversaryQingrenObserverRoot();
    internal bool AnniversaryQingrenObserverSuffix(int root) => _resolve().AnniversaryQingrenObserverSuffix(root);
    internal bool AnniversaryYizanAlcoholAllowed(CharacterState owner, ProgramMultiCardViewAsSelection s) => _resolve().AnniversaryYizanAlcoholAllowed(owner, s);
    internal int AnniversaryYizanCount(int owner, int? actualTurn = default, long cutoff = 9223372036854775807L) => _resolve().AnniversaryYizanCount(owner, actualTurn, cutoff);
    internal bool AnniversaryYizanMaterials(CharacterState owner, CardConversionSource source, IReadOnlyList<int> ids) => _resolve().AnniversaryYizanMaterials(owner, source, ids);
    internal bool AnniversaryYizanModified(int owner) => _resolve().AnniversaryYizanModified(owner);
    internal IEnumerable<LegalAction> AnniversaryYizanPairActions(CharacterState owner) => _resolve().AnniversaryYizanPairActions(owner);
    internal IReadOnlyList<int> AnniversaryYizanPlayTargets(CharacterState owner, ProgramMultiCardViewAsSelection selection) => _resolve().AnniversaryYizanPlayTargets(owner, selection);
    internal bool AnniversaryZtgOriginal(ProgramSkillFrame f) => _resolve().AnniversaryZtgOriginal(f);
    internal CardConversionSource AnniversaryZtgSource(ProgramSkillFrame f) => _resolve().AnniversaryZtgSource(f);
    internal bool BanUnavailableAnniversaryYizan(ProgramViewAsLegalityQuery q) => _resolve().BanUnavailableAnniversaryYizan(q);
    internal bool BanUnavailableAnniversaryYizanSingle(ProgramConfiguredViewAsQuery q) => _resolve().BanUnavailableAnniversaryYizanSingle(q);
    internal SkillProgramStepOutcome BeginAnniversaryZhaoTongZhaoGuang(ProgramSkillFrame supplied, SkillProgramEffectOp op) => _resolve().BeginAnniversaryZhaoTongZhaoGuang(supplied, op);
    internal bool ContinueAnniversaryQingren(long id) => _resolve().ContinueAnniversaryQingren(id);
    internal void CountActualAnniversaryYizan(IGameEvent payload) => _resolve().CountActualAnniversaryYizan(payload);
    internal bool ExecuteAnniversaryYizanPlay(CharacterState owner, LegalAction action, IReadOnlyList<int> ids, IReadOnlyList<int> targets) => _resolve().ExecuteAnniversaryYizanPlay(owner, action, ids, targets);
    internal bool IsAnniversaryYizanSelection(ProgramMultiCardViewAsSelection s) => _resolve().IsAnniversaryYizanSelection(s);
    internal bool KeepPaidAnniversaryQingren(ProgramSkillFrame f) => _resolve().KeepPaidAnniversaryQingren(f);
    internal bool QualifyAnniversaryLongyuan(ProgramTriggerLegalityContext q) => _resolve().QualifyAnniversaryLongyuan(q);
    internal void ReturnAnniversaryQingrenDraw(ProgramSkillFrame supplied, NativeDrawOutcome draw) => _resolve().ReturnAnniversaryQingrenDraw(supplied, draw);
    internal bool ReturnAnniversaryQingrenMovement(ProgramSkillFrame f) => _resolve().ReturnAnniversaryQingrenMovement(f);
    internal bool SubmitAnniversaryYizanPlay(UseProgramSkillCommand command, scoped out CommandResult result) => _resolve().SubmitAnniversaryYizanPlay(command, out result);
    internal bool TryFinishAnniversaryYizanAlcoholCost(CardUseFrame use, Card card) => _resolve().TryFinishAnniversaryYizanAlcoholCost(use, card);
    internal bool TryResolveAnniversaryYizanDyingAlcohol(CharacterState owner, ProgramMultiCardViewAsSelection selection) => _resolve().TryResolveAnniversaryYizanDyingAlcohol(owner, selection);
    internal bool ValidAnniversaryQingren(ProgramSkillFrame f) => _resolve().ValidAnniversaryQingren(f);
    internal CommandError? ValidateAnniversaryYizanPlay(CharacterState owner, LegalAction action, IReadOnlyList<int> ids, IReadOnlyList<int> targets) => _resolve().ValidateAnniversaryYizanPlay(owner, action, ids, targets);
    internal void ValidateAnniversaryYizanUse(ProgramSubmittedUseQuery q) => _resolve().ValidateAnniversaryYizanUse(q);
}
