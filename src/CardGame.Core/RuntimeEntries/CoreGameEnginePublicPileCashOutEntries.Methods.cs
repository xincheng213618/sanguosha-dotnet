#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEnginePublicPileCashOutEntries
{
    internal global::CardGame.Core.SkillProgramStepOutcome BeginPublicPileCashOut(global::CardGame.Core.SkillProgramEffect effect, global::CardGame.Core.ProgramSkillFrame input) => _resolve().BeginPublicPileCashOut(effect, input);
    internal bool CanRunPublicPileCashOut(global::CardGame.Core.ProgramTriggerCandidate candidate, global::CardGame.Core.SkillProgramTrigger trigger, global::CardGame.Core.ProgramSkillWindowContext context) => _resolve().CanRunPublicPileCashOut(candidate, trigger, context);
    internal bool DrainPublicPileCashOut(global::CardGame.Core.ProgramSkillFrame frame) => _resolve().DrainPublicPileCashOut(frame);
    internal bool ExactPublicPileCashOutParent(global::CardGame.Core.ProgramSkillFrame frame, global::CardGame.Core.SkillProgramEffectOp op) => _resolve().ExactPublicPileCashOutParent(frame, op);
    internal bool IsPublicPileCashOutProgramDying() => _resolve().IsPublicPileCashOutProgramDying();
    internal bool MatchesPublicPileStorageSelection(global::CardGame.Core.ProgramSkillFrame frame, string bind, global::CardGame.Core.Card card, global::CardGame.Core.CardLocation location) => _resolve().MatchesPublicPileStorageSelection(frame, bind, card, location);
    internal bool PublicPileCashOutFirstChild(global::CardGame.Core.ProgramSkillFrame frame, global::CardGame.Core.ResolutionFrame child) => _resolve().PublicPileCashOutFirstChild(frame, child);
    internal global::CardGame.Core.ProgramSkillFrame? PublicPileCashOutObserverRoot() => _resolve().PublicPileCashOutObserverRoot();
    internal bool PublicPileCashOutPaymentDestination(global::CardGame.Core.ProgramSkillFrame frame, global::CardGame.Core.CardMovementRecord movement, global::CardGame.Core.CardLocation destination) => _resolve().PublicPileCashOutPaymentDestination(frame, movement, destination);
    internal bool PublicPileStorageCard(int owner, string skill, string instance, global::CardGame.Core.Card card, global::CardGame.Core.CardLocation location) => _resolve().PublicPileStorageCard(owner, skill, instance, card, location);
    internal bool ResumePublicPileCashOut(long id) => _resolve().ResumePublicPileCashOut(id);
    internal bool ReturnPublicPileCashOutMovement(global::CardGame.Core.ProgramSkillFrame frame) => _resolve().ReturnPublicPileCashOutMovement(frame);
    internal bool ValidPublicPileCashOut(global::CardGame.Core.ProgramSkillFrame frame) => _resolve().ValidPublicPileCashOut(frame);
    internal long PublicPileCashOutSequence { get => _resolve().PublicPileCashOutSequence; }
}
