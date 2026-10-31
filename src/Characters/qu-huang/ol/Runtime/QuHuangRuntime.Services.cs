namespace CardGame.Core;

internal sealed partial class QuHuangRuntime
{
    // These views read the original stores at each use. Match state, pending
    // payments and typed returns stay on their existing owners and frames.
    private IReadOnlyList<CharacterState> _players => participants.Players;
    private IReadOnlyList<ResolutionFrame> _resolutionStack => frames.OfType<ResolutionFrame>();
    private IReadOnlyList<CardMovementRecord> _cardMovements => movements.Read();
    private ICardZoneQueries _cardZones => zones;
    private int _turnNumber => facts.TurnNumber;
    private int _roundNumber => facts.RoundNumber;
    private Winner _winner => facts.Winner;
    private int _movementSequence => facts.MovementSequence;
    private long _resolutionSequence => facts.ResolutionSequence;
    private long _cardActionSequence => facts.CardActionSequence;
    private Card GetAdvancedCard(int id) => facts.Card(id);
    private IReadOnlyList<Card> GetHand(CharacterState player) => zones.CardsAt(CardLocation.Hand(player.Seat));
    private IReadOnlyList<T> ProgramEventHistory<T>() where T : IGameEvent => history.Read<T>();
    private SkillProgramTrigger GetProgramTrigger(ProgramSkillFrame frame) => facts.Trigger(frame);
    private SkillProgramTrigger GetProgramTrigger(ProgramTriggerCandidate candidate) => bindings.Trigger(candidate);
    private IReadOnlyList<ProgramTriggerCandidate> CollectProgramTriggerCandidates(CharacterState player, SkillProgramTriggerWindow window) =>
        triggers.CollectProgramTriggerCandidates(player.Seat, window);
    private int FrameIndex(Predicate<ResolutionFrame> predicate)
    {
        var stack = _resolutionStack;
        for (var index = 0; index < stack.Count; index++)
            if (predicate(stack[index])) return index;
        return -1;
    }
    private static string GetProgramBindingId(ProgramSkillFrame frame) =>
        RuntimeSkills.RuntimeRules_Core_ProgramLifecycle.GetProgramBindingId(frame);
    private static bool MountObserverCandidateMatches(ProgramSkillFrame frame, ProgramTriggerCandidate candidate) =>
        RuntimeSkills.RuntimeRules_Core_DamageTargetMountObserver.MountObserverCandidateMatches(frame, candidate);
    private static bool AssistedChoicesEqual(IReadOnlyList<PromptChoice> actual, IReadOnlyList<PromptChoice> expected) =>
        RuntimeSkills.RuntimeRules_Core_AssistedPhysicalCards.AssistedChoicesEqual(actual, expected);
    private static bool LostLastHandCard(CardMovementBatchContext batch, int seat) =>
        RuntimeSkills.RuntimeRules_pan_shu_ol_PanShuPrograms.LostLastHandCard(batch, seat);
    private static bool? SuitColor(Suit suit) => RuntimeSkills.RuntimeRules_Core_PublicPileColors.SuitColor(suit);
    private static bool IsSlashCard(CardKind kind) => RuntimeSkills.RuntimeRules_Core_CommonRules.IsSlashCard(kind);
}
