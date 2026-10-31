#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineOwnTrickDrawTargetsEntries
{
    internal SkillProgramStepOutcome BeginOwnTrickDraw(ProgramSkillFrame input) => _resolve().BeginOwnTrickDraw(input);
    internal bool CanOfferOwnTrickOrHandCategory(ProgramTriggerCandidate c, SkillProgramTrigger trigger, ProgramSkillWindowContext context) => _resolve().CanOfferOwnTrickOrHandCategory(c, trigger, context);
    internal bool HasOwnMultiTargetTrickSelfTarget(CardUseFrame use, int seat) => _resolve().HasOwnMultiTargetTrickSelfTarget(use, seat);
    internal bool IsOwnTrickDrawMovement(ProgramSkillFrame f, SkillProgramEffect? e, ProgramMovementContinuation m) => _resolve().IsOwnTrickDrawMovement(f, e, m);
    internal bool IsOwnTrickDrawProgramDying() => _resolve().IsOwnTrickDrawProgramDying();
    internal bool OwnMultiTargetTrickCandidate(ProgramTriggerCandidate c) => _resolve().OwnMultiTargetTrickCandidate(c);
    internal bool OwnTrickDrawFirstChild(ProgramSkillFrame f, ResolutionFrame child) => _resolve().OwnTrickDrawFirstChild(f, child);
    internal bool OwnTrickDrawMatches(ProgramSkillFrame root, ActualUseTargetWindowFrame parent) => _resolve().OwnTrickDrawMatches(root, parent);
    internal bool ResumeOwnTrickDraw(long id) => _resolve().ResumeOwnTrickDraw(id);
    internal bool ReturnOwnTrickDrawMovement(ProgramSkillFrame frame) => _resolve().ReturnOwnTrickDrawMovement(frame);
    internal bool HasOwnMultiTargetTrickDraw { get => _resolve().HasOwnMultiTargetTrickDraw; }
}
