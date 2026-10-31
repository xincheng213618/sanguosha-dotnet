namespace CardGame.Core;

/// <summary>Only public scalar cost facts; no unplayed hand-card identities enter these events.</summary>
public sealed record HpScaledProgramSelectionEvent(long FrameId, CardConversionSource Source, string GameplayHash,
    int OwnerHp, int MaterialCount) : IGameEvent;
public sealed record HpScaledCardUsePaidEvent(long ActionId, CardConversionSource Source, string GameplayHash,
    int OwnerHp, int MaterialCount) : IGameEvent;

internal static class HpScaledLonghunComposition
{
    internal static void ValidateViewAs(string path, SkillProgramViewAs r)
    {
        if (r.InputCountExpression is null) return;
        var suit = r.OutputKind switch
        {
            CardKind.Peach => Suit.Heart, CardKind.FireSlash => Suit.Diamond,
            CardKind.Dodge => Suit.Club, CardKind.Nullification => Suit.Spade, _ => Suit.None
        };
        if (r.InputCountExpression != SkillProgramCardCountExpression.OwnerHpAtLeastOne || r.InputCount != 1 || r.VariableInputCount ||
            suit == Suit.None || !r.InputSuits.SequenceEqual([suit]) || r.InputKinds.Count != 0 || r.InputCategories.Count != 0 ||
            !r.SourceZones.SequenceEqual([CardZoneKind.Hand, CardZoneKind.Equipment]) || !r.ExtendedUse ||
            r.ForPlay != (r.OutputKind is CardKind.Peach or CardKind.FireSlash) || !r.ForResponse ||
            !r.AllowSameKind || r.Condition.Kind != SkillProgramConditionKind.Always ||
            r.AllowChainedInput || r.SameSuit || r.InheritPreviousPlaySuit || r.AllHandCardsOfSelectedSuit || r.DistanceUnlimited ||
            r.UsesPerPhase is not null || r.UsesPerActualTurn is not null || r.UsageGroup is not null || r.ActivationUsageGroup is not null ||
            r.DamageBonus != 0 || r.RecoveryBonus != 0 || r.CostDestination is not null || r.UseOnly || r.DeclaredEntity ||
            r.SingleCardTrickUse || r.ExcludeOwnerEffects || r.NoDying || r.UnusedOutputThisTurn || r.UnusedOutputNameThisGame ||
            r.NameLedgerId is not null || r.ConversionStateId is not null || r.SourcePileSkillId is not null ||
            r.DeclarationValidation is not null || r.TieredRoundConversion is not null || r.RoundOnceCardlessBasic is not null ||
            r.LeiluanBasicUse is not null || r.XieweiBaitBasicUse is not null || r.ChainedStateCost is not null ||
            r.RoundDistinctBasicUse is not null || r.DrawFundedDistinctBasic is not null || r.DisableSkillUntilTurnEndIfNoDamage is not null ||
            r.LastInSourceZone is not null || r.RoundGainedRedHandOnly is not null || r.KenshangUse is not null ||
            r.FieldMountConversion is not null || r.EqualNameLengthUse || r.RankTwoDamageUse || r.NonBasicUnrespondableSlash || r.UseEffectiveInputSuit is not null)
            throw new InvalidOperationException(path + ": HP-scaled Longhun requires its native same-suit hand/equipment cost and plain original four conversions.");
    }

    [ProgramCatalogValidation(40650)] internal static void Validate(string path, SkillProgram p)
    {
        foreach (var a in p.Activations.Where(a => a.CardCountExpression == SkillProgramCardCountExpression.OwnerHpAtLeastOne))
        {
            if (a.Effects is not [{ Op: SkillProgramEffectOp.UseSelectedCardsAs, SourceBind: { } binding, OutputKind: { } output }] ||
                p.ViewAs.SingleOrDefault(r => r.Id == binding) is not { InputCountExpression: SkillProgramCardCountExpression.OwnerHpAtLeastOne } r ||
                output != r.OutputKind || a.MinCards != 1 || a.MaxCards != int.MaxValue ||
                !a.SourceZones.SequenceEqual(r.SourceZones) || a.UsesPerPhase is not null || a.UsesPerTurn is not null || a.UsesPerGame is not null ||
                a.MinTargets != (output == CardKind.FireSlash ? 1 : 0) || a.MaxTargets != a.MinTargets ||
                a.TargetKind != SkillProgramTargetKind.OtherLiving || a.SelectedCardsSameSuit || a.SelectedCardsDistinctSuits)
                throw new InvalidOperationException(path + ": a HP-scaled activation must select exactly its current native conversion cost without a second payment or quota.");
        }
    }
}
