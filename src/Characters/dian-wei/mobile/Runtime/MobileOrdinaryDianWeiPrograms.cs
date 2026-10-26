using System.Text.Json.Serialization;

namespace CardGame.Core;

public sealed record MobileDianWeiQiangxiReceipt(long FrameId, CardConversionSource Source, string GameplayHash,
    int TurnNumber, int PhaseInstanceId, int TargetSeat, int HpBefore, int? WeaponCardId,
    CardLocation? WeaponFrom, int DistanceWithoutCost, int RangeWithoutCost, bool SpecialRange, int MovementBefore)
{
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public int? MovementAfter { get; init; }
}

public sealed partial record ProgramSkillFrame
{
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public MobileDianWeiQiangxiReceipt? MobileOrdinaryQiangxi { get; init; }
}

internal interface IMobileOrdinaryQiangxiQueries : IProgramSelectionQuery
{
    bool AllowsWeaponTarget(int ownerSeat, int weaponId, int targetSeat);
}

[ProgramSelectionModule(ProgramSelectionStage.AfterTargets, 82800, SkillProgramEffectOp.DiscardSelected)]
internal sealed class MobileOrdinaryQiangxiPairs : IProgramSelectionCandidateFilter
{
    public bool Apply(ProgramSelectionContext context, ProgramSelectionCandidates candidates, IProgramSelectionQueries queries)
    {
        if (context.SkillId != MobileOrdinaryQiangxiComposition.Skill) return true;
        var query = queries.Get<IMobileOrdinaryQiangxiQueries>();
        var pairs = candidates.Cards.SelectMany(card => candidates.Targets
            .Where(target => query.AllowsWeaponTarget(context.OwnerSeat, card, target))
            .Select(target => new ActiveSkillCardTarget(card, target))).ToArray();
        candidates.CardTargets = Array.AsReadOnly(pairs);
        candidates.Cards = pairs.Select(pair => pair.CardId).Distinct().ToArray();
        candidates.Targets = pairs.Select(pair => pair.TargetSeat).Distinct().ToArray();
        return pairs.Length > 0;
    }
}

[ProgramSelectionValidator(ProgramSelectionValidationStage.AfterCardFilters, 82800, SkillProgramEffectOp.DiscardSelected)]
internal sealed class MobileOrdinaryQiangxiPairValidator : IProgramSelectionValidator
{
    public CommandError? Validate(ProgramSelectionValidationRequest request, IProgramSelectionQueries queries) =>
        request.Context.SkillId != MobileOrdinaryQiangxiComposition.Skill ||
        request.Cards is [var card] && request.Targets is [var target] &&
        queries.Get<IMobileOrdinaryQiangxiQueries>().AllowsWeaponTarget(request.Context.OwnerSeat, card, target)
            ? null : new(CommandErrorCode.IllegalAction, "强袭弃置武器时，目标须在不计该装备武器的攻击范围内。");
}

internal static class MobileOrdinaryQiangxiComposition
{
    internal const string Skill = "mobile:qiangxi";
    internal const string Hp = "lose-hp-and-damage", Weapon = "discard-weapon-and-damage";

    [ProgramCatalogValidation(82800)]
    internal static void Validate(string path, SkillProgram program)
    {
        if (program.Id != Skill) return;
        var valid = program.Triggers.Count == 0 && program.ViewAs.Count == 0 && program.Modifiers.Count == 0 &&
            program.BooleanStates.Count == 0 && program.CardPolicies.Count == 0 && program.Activations.Count == 2;
        foreach (var activation in program.Activations)
            valid &= activation.MinTargets == 1 && activation.MaxTargets == 1 &&
                activation.TargetKind == SkillProgramTargetKind.OtherLivingInAttackRange &&
                activation.UsesPerTurn is null && activation.UsesPerPhase == 1 && activation.UsesPerGame is null &&
                activation.UsageGroup == "mobile-qiangxi" && !activation.ContinueAfterOwnerDeath &&
                activation.Effects is [var cost, { Op: SkillProgramEffectOp.Damage, Target: SkillProgramEffectTarget.SelectedTarget,
                    Amount: 1, Condition.Kind: SkillProgramConditionKind.Always }] &&
                cost.Target == SkillProgramEffectTarget.Owner && cost.Amount == 1 && cost.Condition.Kind == SkillProgramConditionKind.Always &&
                (activation.Id == Hp && activation.MinCards == 0 && activation.MaxCards == 0 &&
                    activation.Condition is { Kind: SkillProgramConditionKind.HpAtLeast, Value: 1 } && cost.Op == SkillProgramEffectOp.LoseHp ||
                 activation.Id == Weapon && activation.MinCards == 1 && activation.MaxCards == 1 &&
                    activation.Condition.Kind == SkillProgramConditionKind.Always && cost.Op == SkillProgramEffectOp.DiscardSelected &&
                    activation.SourceZones.SequenceEqual([CardZoneKind.Hand, CardZoneKind.Equipment]) &&
                    activation.EquipmentSlots.SequenceEqual([EquipmentSlot.Weapon]));
        if (!valid) throw new InvalidOperationException(path + ": ordinary mobile Qiangxi requires its shared one-per-Play quota, exact native HP/HE weapon cost and one original-target damage tail.");
    }
}
