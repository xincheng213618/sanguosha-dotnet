#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineUniqueHpPeerProgramsEntries
{
    internal SkillProgramStepOutcome BeginUniqueHpPeer(ProgramSkillFrame supplied) => _resolve().BeginUniqueHpPeer(supplied);
    internal bool CanRunUniqueHpPeer(ProgramTriggerCandidate c, SkillProgramTrigger trigger, ProgramSkillWindowContext context) => _resolve().CanRunUniqueHpPeer(c, trigger, context);
    internal bool ExactUniqueHpPeerParent(ProgramSkillFrame f) => _resolve().ExactUniqueHpPeerParent(f);
    internal void PayUniqueHpPeerCost(ProgramSkillFrame f) => _resolve().PayUniqueHpPeerCost(f);
    internal void PublishUniqueHpPeer(ProgramSkillFrame f) => _resolve().PublishUniqueHpPeer(f);
    internal CardConversionSource RecipientContestSource(ProgramSkillFrame f) => _resolve().RecipientContestSource(f);
    internal bool RecipientContestUnissuedSource(ProgramSkillFrame f) => _resolve().RecipientContestUnissuedSource(f);
    internal void ResolveUniqueHpPeerChoice(PromptChoice choice) => _resolve().ResolveUniqueHpPeerChoice(choice);
    internal bool ResumeUniqueHpPeer(long id) => _resolve().ResumeUniqueHpPeer(id);
    internal PromptChoice SelectAiRecipientContest(PendingDecision decision, ProgramSkillFrame f) => _resolve().SelectAiRecipientContest(decision, f);
    internal void StartUniqueHpPeerCost(ProgramSkillFrame f) => _resolve().StartUniqueHpPeerCost(f);
    internal IReadOnlyList<PromptChoice> UniqueHpPeerChoices(ProgramSkillFrame f) => _resolve().UniqueHpPeerChoices(f);
    internal IEnumerable<(Card Card, CardLocation From)> UniqueHpPeerLegalCards(ProgramSkillFrame f, int actor) => _resolve().UniqueHpPeerLegalCards(f, actor);
    internal bool UniqueHpPeerPayable(ProgramSkillFrame f, int seat) => _resolve().UniqueHpPeerPayable(f, seat);
    internal int UniqueHpPeerRequired(int seat) => _resolve().UniqueHpPeerRequired(seat);
    internal long RecipientContestSequence { get => _resolve().RecipientContestSequence; }
}
