#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineForcedPhaseAlcoholEntries
{
    internal (int Deaths, int Kills) ActualTurnDeathCounts(int ownerSeat) => _resolve().ActualTurnDeathCounts(ownerSeat);
    internal int ActualTurnDeathDrawCount(global::CardGame.Core.ProgramSkillFrame f, int seat) => _resolve().ActualTurnDeathDrawCount(f, seat);
    internal global::CardGame.Core.SkillProgramStepOutcome BeginPhaseOwnerAlcohol(global::CardGame.Core.ProgramSkillFrame frame, int damageToOther) => _resolve().BeginPhaseOwnerAlcohol(frame, damageToOther);
    internal bool CanContinuePhaseAlcohol(global::CardGame.Core.ProgramSkillFrame frame) => _resolve().CanContinuePhaseAlcohol(frame);
    internal bool CanOfferPhaseAlcohol(global::CardGame.Core.SkillProgramTrigger trigger, global::CardGame.Core.ProgramSkillWindowContext context) => _resolve().CanOfferPhaseAlcohol(trigger, context);
    internal bool CanUsePhaseOwnerAlcohol(global::CardGame.Core.ProgramSkillWindowContext? context) => _resolve().CanUsePhaseOwnerAlcohol(context);
    internal void CommitPhaseAlcoholPayment(long id) => _resolve().CommitPhaseAlcoholPayment(id);
    internal bool HasExactPhaseAlcoholReceipt(global::CardGame.Core.ProgramSkillFrame f) => _resolve().HasExactPhaseAlcoholReceipt(f);
    internal bool HasPhaseAlcoholChain(global::CardGame.Core.ProgramSkillFrame frame) => _resolve().HasPhaseAlcoholChain(frame);
    internal bool PreparePhaseAlcoholPayment(global::CardGame.Core.ProgramSkillFrame f, string bind, global::CardGame.Core.CardMoveReason reason) => _resolve().PreparePhaseAlcoholPayment(f, bind, reason);
    internal bool ResumePhaseAlcohol(long id) => _resolve().ResumePhaseAlcohol(id);
}
