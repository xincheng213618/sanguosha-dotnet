#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineUniqueHpPeerProgramsEntries
{
    internal global::CardGame.Core.SkillProgramStepOutcome BeginUniqueHpPeer(global::CardGame.Core.ProgramSkillFrame supplied) => _resolve().BeginUniqueHpPeer(supplied);
    internal bool CanRunUniqueHpPeer(global::CardGame.Core.ProgramTriggerCandidate c, global::CardGame.Core.SkillProgramTrigger trigger, global::CardGame.Core.ProgramSkillWindowContext context) => _resolve().CanRunUniqueHpPeer(c, trigger, context);
    internal bool ExactUniqueHpPeerParent(global::CardGame.Core.ProgramSkillFrame f) => _resolve().ExactUniqueHpPeerParent(f);
    internal void PayUniqueHpPeerCost(global::CardGame.Core.ProgramSkillFrame f) => _resolve().PayUniqueHpPeerCost(f);
    internal void PublishUniqueHpPeer(global::CardGame.Core.ProgramSkillFrame f) => _resolve().PublishUniqueHpPeer(f);
    internal global::CardGame.Core.CardConversionSource RecipientContestSource(global::CardGame.Core.ProgramSkillFrame f) => _resolve().RecipientContestSource(f);
    internal bool RecipientContestUnissuedSource(global::CardGame.Core.ProgramSkillFrame f) => _resolve().RecipientContestUnissuedSource(f);
    internal void ResolveUniqueHpPeerChoice(global::CardGame.Core.PromptChoice choice) => _resolve().ResolveUniqueHpPeerChoice(choice);
    internal bool ResumeUniqueHpPeer(long id) => _resolve().ResumeUniqueHpPeer(id);
    internal global::CardGame.Core.PromptChoice SelectAiRecipientContest(global::CardGame.Core.PendingDecision decision, global::CardGame.Core.ProgramSkillFrame f) => _resolve().SelectAiRecipientContest(decision, f);
    internal void StartUniqueHpPeerCost(global::CardGame.Core.ProgramSkillFrame f) => _resolve().StartUniqueHpPeerCost(f);
    internal global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.PromptChoice> UniqueHpPeerChoices(global::CardGame.Core.ProgramSkillFrame f) => _resolve().UniqueHpPeerChoices(f);
    internal global::System.Collections.Generic.IEnumerable<(global::CardGame.Core.Card Card, global::CardGame.Core.CardLocation From)> UniqueHpPeerLegalCards(global::CardGame.Core.ProgramSkillFrame f, int actor) => _resolve().UniqueHpPeerLegalCards(f, actor);
    internal bool UniqueHpPeerPayable(global::CardGame.Core.ProgramSkillFrame f, int seat) => _resolve().UniqueHpPeerPayable(f, seat);
    internal int UniqueHpPeerRequired(int seat) => _resolve().UniqueHpPeerRequired(seat);
    internal long RecipientContestSequence { get => _resolve().RecipientContestSequence; }
}
