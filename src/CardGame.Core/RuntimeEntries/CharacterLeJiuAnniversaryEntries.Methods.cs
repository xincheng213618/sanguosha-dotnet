#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CharacterLeJiuAnniversaryEntries
{
    internal bool AdmitLeJiuIssuedCompletion(ProgramTriggerCandidate c, ProgramSkillWindowContext w) => _resolve().AdmitLeJiuIssuedCompletion(c, w);
    internal void ArmLeJiuCuijin(ProgramSkillFrame supplied) => _resolve().ArmLeJiuCuijin(supplied);
    internal void AssertLeJiuNativeUse(CardUseFrame use) => _resolve().AssertLeJiuNativeUse(use);
    internal void BindLeJiuCuijin(ProgramSkillFrame supplied) => _resolve().BindLeJiuCuijin(supplied);
    internal bool CanOfferLeJiu(ProgramTriggerCandidate c, SkillProgramTrigger t, ProgramSkillWindowContext w) => _resolve().CanOfferLeJiu(c, t, w);
    internal void CollectLeJiuCompletedUses(CardActionContext action, SkillProgramTriggerWindow window, List<ProgramCardTriggerCandidate> result) => _resolve().CollectLeJiuCompletedUses(action, window, result);
    internal bool ContinuePaidLeJiu(ProgramSkillFrame f) => _resolve().ContinuePaidLeJiu(f);
    internal bool HasAnniversaryLeJiuUseObserver(CharacterState actor) => _resolve().HasAnniversaryLeJiuUseObserver(actor);
    internal bool IsIssuedLeJiuCandidate(ProgramTriggerCandidate c, ProgramSkillWindowContext w, bool includeBound) => _resolve().IsIssuedLeJiuCandidate(c, w, includeBound);
    internal CardUseFrame? LeJiuContextUse(ProgramSkillWindowContext w) => _resolve().LeJiuContextUse(w);
    internal int LeJiuCuijinDamage(GameEngine.IDamageAttempt attack) => _resolve().LeJiuCuijinDamage(attack);
    internal LeJiuCuijinGrant? LeJiuGrantFor(CardUseFrame use, ProgramTriggerCandidate c, bool includeBound = false) => _resolve().LeJiuGrantFor(use, c, includeBound);
    internal bool LeJiuHasCost(int owner) => _resolve().LeJiuHasCost(owner);
    internal (NativeLogicalDiscardInvocationRecordedEvent Invocation, NativeLogicalDiscardMaterialCompletedEvent Material)? LeJiuPaidCost(ProgramSkillFrame f) => _resolve().LeJiuPaidCost(f);
    internal bool LeJiuParent(ProgramSkillFrame f) => _resolve().LeJiuParent(f);
    internal bool ValidLeJiuFrame(ProgramSkillFrame f) => _resolve().ValidLeJiuFrame(f);
    internal bool ValidLeJiuGrant(CardUseFrame use, LeJiuCuijinGrant g) => _resolve().ValidLeJiuGrant(use, g);
}
