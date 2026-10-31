#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineForcedPhaseAlcoholEntries
{
    internal (int Deaths, int Kills) ActualTurnDeathCounts(int ownerSeat) => _resolve().ActualTurnDeathCounts(ownerSeat);
    internal int ActualTurnDeathDrawCount(ProgramSkillFrame f, int seat) => _resolve().ActualTurnDeathDrawCount(f, seat);
    internal SkillProgramStepOutcome BeginPhaseOwnerAlcohol(ProgramSkillFrame frame, int damageToOther) => _resolve().BeginPhaseOwnerAlcohol(frame, damageToOther);
    internal bool CanContinuePhaseAlcohol(ProgramSkillFrame frame) => _resolve().CanContinuePhaseAlcohol(frame);
    internal bool CanOfferPhaseAlcohol(SkillProgramTrigger trigger, ProgramSkillWindowContext context) => _resolve().CanOfferPhaseAlcohol(trigger, context);
    internal bool CanUsePhaseOwnerAlcohol(ProgramSkillWindowContext? context) => _resolve().CanUsePhaseOwnerAlcohol(context);
    internal void CommitPhaseAlcoholPayment(long id) => _resolve().CommitPhaseAlcoholPayment(id);
    internal bool HasExactPhaseAlcoholReceipt(ProgramSkillFrame f) => _resolve().HasExactPhaseAlcoholReceipt(f);
    internal bool HasPhaseAlcoholChain(ProgramSkillFrame frame) => _resolve().HasPhaseAlcoholChain(frame);
    internal bool PreparePhaseAlcoholPayment(ProgramSkillFrame f, string bind, CardMoveReason reason) => _resolve().PreparePhaseAlcoholPayment(f, bind, reason);
    internal bool ResumePhaseAlcohol(long id) => _resolve().ResumePhaseAlcohol(id);
}
