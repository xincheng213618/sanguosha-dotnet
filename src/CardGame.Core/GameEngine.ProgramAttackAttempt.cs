namespace CardGame.Core;

public sealed partial class GameEngine
{
internal interface IDamageAttempt
    {
        long ResolutionId { get; }
        int SourceSeat { get; }
        int CardUserSeat { get; }
        int TargetSeat { get; }
        Card? Card { get; }
        IReadOnlyList<Card> PhysicalCards { get; }
        CardKind? EffectiveCardKind { get; }
        CardConversionSource? ConversionSource { get; }
        int DamageAmount { get; }
        bool DamageAmountFinalized { get; }
        bool IgnoresArmor { get; }
        bool IsDelayedJudgmentDamage { get; }
        bool IsProgramJudgmentDamage { get; }
        bool IsSourceLess { get; }
        DamageNature? DamageNatureOverride { get; }
        bool IsChainPropagation { get; }
        bool DamageWasApplied { get; }
        bool BeforeDamageProgramsResolved { get; }
        bool DamageRedirected { get; }
        bool PendingRedBladeDamageBonus { get; set; }
        int SourceToTargetDistanceAtDamage { get; }
        void SetIgnoresArmor(bool value);
        void FinalizeDamageAmount(int bonus, int? maximum = null);
        void ReduceFinalizedDamageAmount(int amount);
        void IncreaseFinalizedDamageAmount(int amount);
        void MarkBeforeDamageProgramsResolved();
        void MarkDamageApplied();
        void CaptureDamageDistance(int distance);
        void MarkDyingResolvedForDamage(long damageFrameId);
        bool HasResolvedDyingForDamage(long damageFrameId);
        void RedirectFinalizedDamageTarget(int targetSeat, global::CardGame.Core.GameEngine.ProgramDamageTransferFollowup followup);
        bool TryConsumeProgramDamageTransferFollowup(out global::CardGame.Core.GameEngine.ProgramDamageTransferFollowup followup);
        void SetChainedTargets(IReadOnlyList<int> targetSeats);
        bool TryAdvanceChainedTarget(Func<int, bool> isAlive, bool resetDamageForTargetModifiers,
            out int fromSeat);
    }

}
