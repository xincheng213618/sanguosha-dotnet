#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CharacterLeJiuAnniversaryEntries
{
    internal bool AdmitLeJiuIssuedCompletion(global::CardGame.Core.ProgramTriggerCandidate c, global::CardGame.Core.ProgramSkillWindowContext w) => _resolve().AdmitLeJiuIssuedCompletion(c, w);
    internal void ArmLeJiuCuijin(global::CardGame.Core.ProgramSkillFrame supplied) => _resolve().ArmLeJiuCuijin(supplied);
    internal void AssertLeJiuNativeUse(global::CardGame.Core.CardUseFrame use) => _resolve().AssertLeJiuNativeUse(use);
    internal void BindLeJiuCuijin(global::CardGame.Core.ProgramSkillFrame supplied) => _resolve().BindLeJiuCuijin(supplied);
    internal bool CanOfferLeJiu(global::CardGame.Core.ProgramTriggerCandidate c, global::CardGame.Core.SkillProgramTrigger t, global::CardGame.Core.ProgramSkillWindowContext w) => _resolve().CanOfferLeJiu(c, t, w);
    internal void CollectLeJiuCompletedUses(global::CardGame.Core.CardActionContext action, global::CardGame.Core.SkillProgramTriggerWindow window, global::System.Collections.Generic.List<global::CardGame.Core.ProgramCardTriggerCandidate> result) => _resolve().CollectLeJiuCompletedUses(action, window, result);
    internal bool ContinuePaidLeJiu(global::CardGame.Core.ProgramSkillFrame f) => _resolve().ContinuePaidLeJiu(f);
    internal bool HasAnniversaryLeJiuUseObserver(global::CardGame.Core.CharacterState actor) => _resolve().HasAnniversaryLeJiuUseObserver(actor);
    internal bool IsIssuedLeJiuCandidate(global::CardGame.Core.ProgramTriggerCandidate c, global::CardGame.Core.ProgramSkillWindowContext w, bool includeBound) => _resolve().IsIssuedLeJiuCandidate(c, w, includeBound);
    internal global::CardGame.Core.CardUseFrame? LeJiuContextUse(global::CardGame.Core.ProgramSkillWindowContext w) => _resolve().LeJiuContextUse(w);
    internal int LeJiuCuijinDamage(global::CardGame.Core.GameEngine.IDamageAttempt attack) => _resolve().LeJiuCuijinDamage(attack);
    internal global::CardGame.Core.LeJiuCuijinGrant? LeJiuGrantFor(global::CardGame.Core.CardUseFrame use, global::CardGame.Core.ProgramTriggerCandidate c, bool includeBound = false) => _resolve().LeJiuGrantFor(use, c, includeBound);
    internal bool LeJiuHasCost(int owner) => _resolve().LeJiuHasCost(owner);
    internal (global::CardGame.Core.NativeLogicalDiscardInvocationRecordedEvent Invocation, global::CardGame.Core.NativeLogicalDiscardMaterialCompletedEvent Material)? LeJiuPaidCost(global::CardGame.Core.ProgramSkillFrame f) => _resolve().LeJiuPaidCost(f);
    internal bool LeJiuParent(global::CardGame.Core.ProgramSkillFrame f) => _resolve().LeJiuParent(f);
    internal bool ValidLeJiuFrame(global::CardGame.Core.ProgramSkillFrame f) => _resolve().ValidLeJiuFrame(f);
    internal bool ValidLeJiuGrant(global::CardGame.Core.CardUseFrame use, global::CardGame.Core.LeJiuCuijinGrant g) => _resolve().ValidLeJiuGrant(use, g);
}
