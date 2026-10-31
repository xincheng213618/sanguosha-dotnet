#nullable enable

namespace CardGame.Core.NativeOwners;

internal sealed class ActionQuery(int players,(int Seat, Card Card)? printedTargetTemplate = null) : IActualDiscardPhaseQueryCacheState
    {
        internal readonly (int Seat, Card Card)? PrintedTargetTemplate = printedTargetTemplate;
        internal readonly SkillBindingShard?[] Bindings = new SkillBindingShard?[players];
        internal readonly Dictionary<(int Seat, Card Card), IReadOnlyList<global::CardGame.Core.GameEngine.ProgramCardIdentityMatch>> Identities = [];
        internal readonly Dictionary<(int Seat, Card Card, CardKind Kind, bool Response, bool Dying),
            IReadOnlyList<CardConversionSource>> Conversions = [];
        internal readonly Dictionary<(int Source, int Target, int? Excluded), RuleQueryEvaluation> Distances = [];
        internal readonly Dictionary<(int Seat, int? Excluded), RuleQueryEvaluation> AttackRanges = [];
        internal readonly Dictionary<(int Seat, int? Excluded, EquipmentSlot? Slot), RuleQueryEvaluation> SlashUseLimits = [];
        internal readonly Dictionary<(int Seat, bool IgnoreSuitBan), IReadOnlyList<Card>> SlashCards = [];
        internal bool HasActualDiscardRecoveryPhase;
        internal ActualDiscardRecoveryPhaseKey? ActualDiscardRecoveryPhase;
        bool IActualDiscardPhaseQueryCacheState.HasActualDiscardRecoveryPhase
        { get => HasActualDiscardRecoveryPhase; set => HasActualDiscardRecoveryPhase = value; }
        ActualDiscardRecoveryPhaseKey? IActualDiscardPhaseQueryCacheState.ActualDiscardRecoveryPhase
        { get => ActualDiscardRecoveryPhase; set => ActualDiscardRecoveryPhase = value; }
    }
