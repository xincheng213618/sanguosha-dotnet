#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CharacterLuoXianOlEntries
{
    internal bool AdvanceDailiSubtree() => _resolve().AdvanceDailiSubtree();
    internal bool CanOfferDaili(ProgramTriggerCandidate candidate, SkillProgramTrigger trigger, ProgramSkillWindowContext context) => _resolve().CanOfferDaili(candidate, trigger, context);
    internal void CompleteDailiAfterPublicReveal(ProgramSkillFrame frame) => _resolve().CompleteDailiAfterPublicReveal(frame);
    internal bool ContinueIssuedDaili(ProgramSkillFrame frame) => _resolve().ContinueIssuedDaili(frame);
    internal long? CurrentHandEntry(int owner, int id) => _resolve().CurrentHandEntry(owner, id);
    internal IEnumerable<PublicHandEntityRevealedEvent> CurrentPublicHandReveals() => _resolve().CurrentPublicHandReveals();
    internal IEnumerable<CardMovementRecord> DailiDrawnMovements(ProgramSkillFrame frame) => _resolve().DailiDrawnMovements(frame);
    internal bool DailiFirstChild(ProgramSkillFrame frame, ResolutionFrame child) => _resolve().DailiFirstChild(frame, child);
    internal ProgramSkillFrame? DailiObserverRoot() => _resolve().DailiObserverRoot();
    internal bool DrainDailiChildren(long id) => _resolve().DrainDailiChildren(id);
    internal void ObserveExplicitPublicHandReveal(IGameEvent payload) => _resolve().ObserveExplicitPublicHandReveal(payload);
    internal int PubliclyRevealedHandCount(int owner) => _resolve().PubliclyRevealedHandCount(owner);
    internal SkillProgramStepOutcome ResolveDailiTurnEnd(ProgramSkillFrame supplied) => _resolve().ResolveDailiTurnEnd(supplied);
    internal bool ResumeDaili(long id) => _resolve().ResumeDaili(id);
    internal bool ReturnDailiMovement(ProgramSkillFrame frame) => _resolve().ReturnDailiMovement(frame);
    internal bool ValidDaili(ProgramSkillFrame frame) => _resolve().ValidDaili(frame);
    internal bool TracksPublicHandReveals { get => _resolve().TracksPublicHandReveals; }
}
