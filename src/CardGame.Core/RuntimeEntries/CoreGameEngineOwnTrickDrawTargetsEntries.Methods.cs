#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineOwnTrickDrawTargetsEntries
{
    internal global::CardGame.Core.SkillProgramStepOutcome BeginOwnTrickDraw(global::CardGame.Core.ProgramSkillFrame input) => _resolve().BeginOwnTrickDraw(input);
    internal bool CanOfferOwnTrickOrHandCategory(global::CardGame.Core.ProgramTriggerCandidate c, global::CardGame.Core.SkillProgramTrigger trigger, global::CardGame.Core.ProgramSkillWindowContext context) => _resolve().CanOfferOwnTrickOrHandCategory(c, trigger, context);
    internal bool HasOwnMultiTargetTrickSelfTarget(global::CardGame.Core.CardUseFrame use, int seat) => _resolve().HasOwnMultiTargetTrickSelfTarget(use, seat);
    internal bool IsOwnTrickDrawMovement(global::CardGame.Core.ProgramSkillFrame f, global::CardGame.Core.SkillProgramEffect? e, global::CardGame.Core.ProgramMovementContinuation m) => _resolve().IsOwnTrickDrawMovement(f, e, m);
    internal bool IsOwnTrickDrawProgramDying() => _resolve().IsOwnTrickDrawProgramDying();
    internal bool OwnMultiTargetTrickCandidate(global::CardGame.Core.ProgramTriggerCandidate c) => _resolve().OwnMultiTargetTrickCandidate(c);
    internal bool OwnTrickDrawFirstChild(global::CardGame.Core.ProgramSkillFrame f, global::CardGame.Core.ResolutionFrame child) => _resolve().OwnTrickDrawFirstChild(f, child);
    internal bool OwnTrickDrawMatches(global::CardGame.Core.ProgramSkillFrame root, global::CardGame.Core.ActualUseTargetWindowFrame parent) => _resolve().OwnTrickDrawMatches(root, parent);
    internal bool ResumeOwnTrickDraw(long id) => _resolve().ResumeOwnTrickDraw(id);
    internal bool ReturnOwnTrickDrawMovement(global::CardGame.Core.ProgramSkillFrame frame) => _resolve().ReturnOwnTrickDrawMovement(frame);
    internal bool HasOwnMultiTargetTrickDraw { get => _resolve().HasOwnMultiTargetTrickDraw; }
}
