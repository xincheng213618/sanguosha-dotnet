#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CharacterZhaoTongZhaoGuangAnniversaryEntries
{
    internal bool AdvanceAnniversaryQingrenChildren() => _resolve().AdvanceAnniversaryQingrenChildren();
    internal bool AnniversaryQingrenBeforeDamageObserver(long id) => _resolve().AnniversaryQingrenBeforeDamageObserver(id);
    internal bool AnniversaryQingrenDamageObserver(long id) => _resolve().AnniversaryQingrenDamageObserver(id);
    internal bool AnniversaryQingrenDyingObserver() => _resolve().AnniversaryQingrenDyingObserver();
    internal bool AnniversaryQingrenFirstChild(global::CardGame.Core.ProgramSkillFrame f, global::CardGame.Core.ResolutionFrame child) => _resolve().AnniversaryQingrenFirstChild(f, child);
    internal bool AnniversaryQingrenMovementProof(global::CardGame.Core.ProgramSkillFrame f, global::CardGame.Core.SkillProgramEffect? e, global::CardGame.Core.ProgramMovementContinuation p) => _resolve().AnniversaryQingrenMovementProof(f, e, p);
    internal global::CardGame.Core.ProgramSkillFrame? AnniversaryQingrenObserverRoot() => _resolve().AnniversaryQingrenObserverRoot();
    internal bool AnniversaryQingrenObserverSuffix(int root) => _resolve().AnniversaryQingrenObserverSuffix(root);
    internal bool AnniversaryYizanAlcoholAllowed(global::CardGame.Core.CharacterState owner, global::CardGame.Core.ProgramMultiCardViewAsSelection s) => _resolve().AnniversaryYizanAlcoholAllowed(owner, s);
    internal int AnniversaryYizanCount(int owner, int? actualTurn = default, long cutoff = 9223372036854775807L) => _resolve().AnniversaryYizanCount(owner, actualTurn, cutoff);
    internal bool AnniversaryYizanMaterials(global::CardGame.Core.CharacterState owner, global::CardGame.Core.CardConversionSource source, global::System.Collections.Generic.IReadOnlyList<int> ids) => _resolve().AnniversaryYizanMaterials(owner, source, ids);
    internal bool AnniversaryYizanModified(int owner) => _resolve().AnniversaryYizanModified(owner);
    internal global::System.Collections.Generic.IEnumerable<global::CardGame.Core.LegalAction> AnniversaryYizanPairActions(global::CardGame.Core.CharacterState owner) => _resolve().AnniversaryYizanPairActions(owner);
    internal global::System.Collections.Generic.IReadOnlyList<int> AnniversaryYizanPlayTargets(global::CardGame.Core.CharacterState owner, global::CardGame.Core.ProgramMultiCardViewAsSelection selection) => _resolve().AnniversaryYizanPlayTargets(owner, selection);
    internal bool AnniversaryZtgOriginal(global::CardGame.Core.ProgramSkillFrame f) => _resolve().AnniversaryZtgOriginal(f);
    internal global::CardGame.Core.CardConversionSource AnniversaryZtgSource(global::CardGame.Core.ProgramSkillFrame f) => _resolve().AnniversaryZtgSource(f);
    internal void AssertAnniversaryQingren(global::CardGame.Core.ProgramSkillFrame f) => _resolve().AssertAnniversaryQingren(f);
    internal bool BanUnavailableAnniversaryYizan(global::CardGame.Core.ProgramViewAsLegalityQuery q) => _resolve().BanUnavailableAnniversaryYizan(q);
    internal bool BanUnavailableAnniversaryYizanSingle(global::CardGame.Core.ProgramConfiguredViewAsQuery q) => _resolve().BanUnavailableAnniversaryYizanSingle(q);
    internal global::CardGame.Core.SkillProgramStepOutcome BeginAnniversaryZhaoTongZhaoGuang(global::CardGame.Core.ProgramSkillFrame supplied, global::CardGame.Core.SkillProgramEffectOp op) => _resolve().BeginAnniversaryZhaoTongZhaoGuang(supplied, op);
    internal bool ContinueAnniversaryQingren(long id) => _resolve().ContinueAnniversaryQingren(id);
    internal void CountActualAnniversaryYizan(global::CardGame.Core.IGameEvent payload) => _resolve().CountActualAnniversaryYizan(payload);
    internal bool ExecuteAnniversaryYizanPlay(global::CardGame.Core.CharacterState owner, global::CardGame.Core.LegalAction action, global::System.Collections.Generic.IReadOnlyList<int> ids, global::System.Collections.Generic.IReadOnlyList<int> targets) => _resolve().ExecuteAnniversaryYizanPlay(owner, action, ids, targets);
    internal bool IsAnniversaryYizanSelection(global::CardGame.Core.ProgramMultiCardViewAsSelection s) => _resolve().IsAnniversaryYizanSelection(s);
    internal bool KeepPaidAnniversaryQingren(global::CardGame.Core.ProgramSkillFrame f) => _resolve().KeepPaidAnniversaryQingren(f);
    internal bool QualifyAnniversaryLongyuan(global::CardGame.Core.ProgramTriggerLegalityContext q) => _resolve().QualifyAnniversaryLongyuan(q);
    internal void ReturnAnniversaryQingrenDraw(global::CardGame.Core.ProgramSkillFrame supplied, global::CardGame.Core.NativeDrawOutcome draw) => _resolve().ReturnAnniversaryQingrenDraw(supplied, draw);
    internal bool ReturnAnniversaryQingrenMovement(global::CardGame.Core.ProgramSkillFrame f) => _resolve().ReturnAnniversaryQingrenMovement(f);
    internal bool SubmitAnniversaryYizanPlay(global::CardGame.Core.UseProgramSkillCommand command, scoped out global::CardGame.Core.CommandResult result) => _resolve().SubmitAnniversaryYizanPlay(command, out result);
    internal bool TryFinishAnniversaryYizanAlcoholCost(global::CardGame.Core.CardUseFrame use, global::CardGame.Core.Card card) => _resolve().TryFinishAnniversaryYizanAlcoholCost(use, card);
    internal bool TryResolveAnniversaryYizanDyingAlcohol(global::CardGame.Core.CharacterState owner, global::CardGame.Core.ProgramMultiCardViewAsSelection selection) => _resolve().TryResolveAnniversaryYizanDyingAlcohol(owner, selection);
    internal bool ValidAnniversaryQingren(global::CardGame.Core.ProgramSkillFrame f) => _resolve().ValidAnniversaryQingren(f);
    internal global::CardGame.Core.CommandError? ValidateAnniversaryYizanPlay(global::CardGame.Core.CharacterState owner, global::CardGame.Core.LegalAction action, global::System.Collections.Generic.IReadOnlyList<int> ids, global::System.Collections.Generic.IReadOnlyList<int> targets) => _resolve().ValidateAnniversaryYizanPlay(owner, action, ids, targets);
    internal void ValidateAnniversaryYizanUse(global::CardGame.Core.ProgramSubmittedUseQuery q) => _resolve().ValidateAnniversaryYizanUse(q);
}
