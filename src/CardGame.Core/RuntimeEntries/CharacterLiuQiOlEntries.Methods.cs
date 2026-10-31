#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CharacterLiuQiOlEntries
{
    internal global::CardGame.Core.SkillProgramStepOutcome BeginLiuQi(global::CardGame.Core.ProgramSkillFrame supplied, global::CardGame.Core.SkillProgramEffect e) => _resolve().BeginLiuQi(supplied, e);
    internal bool CanContinueLiuQi(global::CardGame.Core.ProgramSkillFrame f) => _resolve().CanContinueLiuQi(f);
    internal bool CanOfferLiuQi(global::CardGame.Core.ProgramTriggerCandidate candidate, global::CardGame.Core.SkillProgramTrigger trigger, global::CardGame.Core.ProgramSkillWindowContext context) => _resolve().CanOfferLiuQi(candidate, trigger, context);
    internal void FinishLiuQi(global::CardGame.Core.ProgramSkillFrame f) => _resolve().FinishLiuQi(f);
    internal bool HasUntargetedUnskippedLiuQiPlay(int owner) => _resolve().HasUntargetedUnskippedLiuQiPlay(owner);
    internal bool IsWenjiNullificationResponseForbidden(global::CardGame.Core.NullificationWindowFrame window, int responderSeat) => _resolve().IsWenjiNullificationResponseForbidden(window, responderSeat);
    internal bool IsWenjiResponseForbidden(global::CardGame.Core.CardActionContext originalUse, int responderSeat) => _resolve().IsWenjiResponseForbidden(originalUse, responderSeat);
    internal bool IsWenjiResponseForbidden2(long useId, int responderSeat) => _resolve().IsWenjiResponseForbidden(useId, responderSeat);
    internal global::CardGame.Core.PromptChoice LiuQiAiChoice(global::CardGame.Core.ProgramSkillFrame f) => _resolve().LiuQiAiChoice(f);
    internal global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.PromptChoice> LiuQiChoices(global::CardGame.Core.ProgramSkillFrame f) => _resolve().LiuQiChoices(f);
    internal bool LiuQiFirstChild(global::CardGame.Core.ProgramSkillFrame root, global::CardGame.Core.ResolutionFrame child) => _resolve().LiuQiFirstChild(root, child);
    internal global::CardGame.Core.ProgramSkillFrame? LiuQiObserverRoot() => _resolve().LiuQiObserverRoot();
    internal void ObserveLiuQiEvent(global::CardGame.Core.IGameEvent payload) => _resolve().ObserveLiuQiEvent(payload);
    internal void ObserveLiuQiPlayBoundary(int ownerSeat, int phaseInstance, bool skipped) => _resolve().ObserveLiuQiPlayBoundary(ownerSeat, phaseInstance, skipped);
    internal void PublishWenjiChoice(global::CardGame.Core.ProgramSkillFrame f) => _resolve().PublishWenjiChoice(f);
    internal void RequireLiuQi(global::CardGame.Core.ProgramSkillFrame f) => _resolve().RequireLiuQi(f);
    internal void ResolveWenjiChoice(global::CardGame.Core.PromptChoice choice) => _resolve().ResolveWenjiChoice(choice);
    internal bool ResumeLiuQi(long id) => _resolve().ResumeLiuQi(id);
    internal bool ReturnLiuQiMovement(global::CardGame.Core.ProgramSkillFrame f) => _resolve().ReturnLiuQiMovement(f);
    internal bool TryAdvanceLiuQiSubtree() => _resolve().TryAdvanceLiuQiSubtree();
    internal global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.Card> WenjiPayableCards(int seat) => _resolve().WenjiPayableCards(seat);
    internal bool HasTunjiangCapability { get => _resolve().HasTunjiangCapability; }
}
