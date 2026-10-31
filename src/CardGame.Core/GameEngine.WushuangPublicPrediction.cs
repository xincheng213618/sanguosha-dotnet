namespace CardGame.Core;

/// <summary>One visible damage segment and its immediate retaliation; never a promise that hidden responses fail.</summary>
public sealed record PublicWushuangCombatPrediction(PublicDamagePrediction TargetPayment,
    PublicDamageSequencePrediction? RetaliationIfTargetSurvives, bool DrunkenSlashUnrespondable, bool FixedDamage,
    int IncomingReductionSegmentsRemaining = 0, int RemainingOrdinarySlashUses = 0);

public sealed partial record PublicDamagePrediction
{
    [System.Text.Json.Serialization.JsonIgnore(Condition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull)]
    public PublicWushuangCombatPrediction? Wushuang { get; init; }
}
