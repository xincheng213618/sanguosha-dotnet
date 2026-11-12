#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CharacterLiuQiOlEntries
{
    internal SkillProgramStepOutcome BeginLiuQi(ProgramSkillFrame supplied, SkillProgramEffect e) => _resolve().BeginLiuQi(supplied, e);
    internal bool CanContinueLiuQi(ProgramSkillFrame f) => _resolve().CanContinueLiuQi(f);
    internal bool CanOfferLiuQi(ProgramTriggerCandidate candidate, SkillProgramTrigger trigger, ProgramSkillWindowContext context) => _resolve().CanOfferLiuQi(candidate, trigger, context);
    internal void FinishLiuQi(ProgramSkillFrame f) => _resolve().FinishLiuQi(f);
    internal PromptChoice LiuQiAiChoice(ProgramSkillFrame f) => _resolve().LiuQiAiChoice(f);
    internal IReadOnlyList<PromptChoice> LiuQiChoices(ProgramSkillFrame f) => _resolve().LiuQiChoices(f);
    internal bool LiuQiFirstChild(ProgramSkillFrame root, ResolutionFrame child) => _resolve().LiuQiFirstChild(root, child);
    internal ProgramSkillFrame? LiuQiObserverRoot() => _resolve().LiuQiObserverRoot();
    internal void PublishWenjiChoice(ProgramSkillFrame f) => _resolve().PublishWenjiChoice(f);
    internal void RequireLiuQi(ProgramSkillFrame f) => _resolve().RequireLiuQi(f);
    internal void ResolveWenjiChoice(PromptChoice choice) => _resolve().ResolveWenjiChoice(choice);
    internal bool ResumeLiuQi(long id) => _resolve().ResumeLiuQi(id);
    internal bool ReturnLiuQiMovement(ProgramSkillFrame f) => _resolve().ReturnLiuQiMovement(f);
    internal bool TryAdvanceLiuQiSubtree() => _resolve().TryAdvanceLiuQiSubtree();
    internal IReadOnlyList<Card> WenjiPayableCards(int seat) => _resolve().WenjiPayableCards(seat);
}
