#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEnginePublicPileCashOutEntries
{
    internal SkillProgramStepOutcome BeginPublicPileCashOut(SkillProgramEffect effect, ProgramSkillFrame input) => _resolve().BeginPublicPileCashOut(effect, input);
    internal bool CanRunPublicPileCashOut(ProgramTriggerCandidate candidate, SkillProgramTrigger trigger, ProgramSkillWindowContext context) => _resolve().CanRunPublicPileCashOut(candidate, trigger, context);
    internal bool DrainPublicPileCashOut(ProgramSkillFrame frame) => _resolve().DrainPublicPileCashOut(frame);
    internal bool ExactPublicPileCashOutParent(ProgramSkillFrame frame, SkillProgramEffectOp op) => _resolve().ExactPublicPileCashOutParent(frame, op);
    internal bool IsPublicPileCashOutProgramDying() => _resolve().IsPublicPileCashOutProgramDying();
    internal bool MatchesPublicPileStorageSelection(ProgramSkillFrame frame, string bind, Card card, CardLocation location) => _resolve().MatchesPublicPileStorageSelection(frame, bind, card, location);
    internal bool PublicPileCashOutFirstChild(ProgramSkillFrame frame, ResolutionFrame child) => _resolve().PublicPileCashOutFirstChild(frame, child);
    internal ProgramSkillFrame? PublicPileCashOutObserverRoot() => _resolve().PublicPileCashOutObserverRoot();
    internal bool PublicPileCashOutPaymentDestination(ProgramSkillFrame frame, CardMovementRecord movement, CardLocation destination) => _resolve().PublicPileCashOutPaymentDestination(frame, movement, destination);
    internal bool PublicPileStorageCard(int owner, string skill, string instance, Card card, CardLocation location) => _resolve().PublicPileStorageCard(owner, skill, instance, card, location);
    internal bool ResumePublicPileCashOut(long id) => _resolve().ResumePublicPileCashOut(id);
    internal bool ReturnPublicPileCashOutMovement(ProgramSkillFrame frame) => _resolve().ReturnPublicPileCashOutMovement(frame);
    internal bool ValidPublicPileCashOut(ProgramSkillFrame frame) => _resolve().ValidPublicPileCashOut(frame);
    internal long PublicPileCashOutSequence { get => _resolve().PublicPileCashOutSequence; }
}
