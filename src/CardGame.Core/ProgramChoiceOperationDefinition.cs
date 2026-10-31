namespace CardGame.Core;

/// <summary>A named, conditional choice; effects remain ordinary subsequent instructions.</summary>
internal sealed class ChooseOptionProgramOperationDescriptor : ProgramOperationDescriptorBase
{
    public override SkillProgramEffectOp Op => SkillProgramEffectOp.ChooseOption;
    public override bool AllowsDeadTarget(in ProgramEffectContinuationContext c) =>
        c.Effect.Target == SkillProgramEffectTarget.Owner && c.State.CanContinueAfterOwnerDeath(c.Frame);
    public override ISkillProgramEffectHandler Handler { get; } = new ChooseOptionSkillProgramEffectHandler();
    public override ProgramOperationInteraction Interaction => ProgramOperationInteraction.Choice;
    public override ProgramOperationAiPolicy AiPolicy { get; } = new(
        ProgramOperationAiSemantic.ChooseOption, static (_, _) => { });

    public override SkillProgramEffect Parse(ProgramOperationNodeReader reader)
    {
        reader.AllowOnly("op", "target", "chooserRef", "resultBind", "options", "condition",
            "replacementSuits", "minimumReplacementRank", "maximumReplacementRank");
        var chooserRef = reader.Has("chooserRef")
            ? reader.RequiredParticipantReference("chooserRef") : null;
        if (chooserRef is not null && chooserRef.Kind is not
            (ProgramParticipantRef.Owner or ProgramParticipantRef.EventSource or ProgramParticipantRef.EventTarget or ProgramParticipantRef.SelectedTarget))
            throw new InvalidOperationException($"Invalid skill program at {reader.Path}.chooserRef: unsupported choice participant.");
        var replacementSuits = reader.OptionalEnumArray<Suit>("replacementSuits") ?? [];
        var minimum = reader.Has("minimumReplacementRank") ? reader.RequiredInt("minimumReplacementRank") : 0;
        var maximum = reader.Has("maximumReplacementRank") ? reader.RequiredInt("maximumReplacementRank") : 0;
        if ((replacementSuits.Count > 0) != (minimum > 0 && maximum > 0) ||
            replacementSuits.Count > 0 && (minimum > maximum || maximum > 13))
            throw new InvalidOperationException($"Invalid skill program at {reader.Path}: replacement suits require valid rank bounds.");
        var effect = new SkillProgramEffect(Op, reader.RequiredEnum<SkillProgramEffectTarget>("target"),
            0, reader.Condition(), resultBind: reader.RequiredIdentifier("resultBind"),
            options: reader.ChoiceOptions(), chooserRef: chooserRef, replacementSuits: replacementSuits,
            minimumReplacementRank: minimum, maximumReplacementRank: maximum);
        if (effect.Options.Any(option => option.RequiredTargetKind is not null) &&
            (effect.Target != SkillProgramEffectTarget.Owner ||
             chooserRef is not null and not { Kind: ProgramParticipantRef.Owner }))
            throw new InvalidOperationException($"Invalid skill program at {reader.Path}: requiredTargetKind options require the owner chooser.");
        foreach (var option in effect.Options)
            ValidateOptionCondition(option.Condition, reader.Path);
        return effect;
    }

    public override IReadOnlyList<ProgramResourceOperation> Resources(SkillProgramEffect effect) =>
        WithSelectedTarget(effect, [new CreateChoiceResult(effect.ResultBind!, effect.Options.Select(option => option.Id).ToArray())])
            .Concat(effect.ChooserRef is { Kind: ProgramParticipantRef.SelectedTarget } ? [new ReadSelectedTarget()] : [])
            .Concat(effect.ChooserRef is { Kind: ProgramParticipantRef.EventSource }
                ? [new RequireAnyContext(ProgramContextCapability.Damage)] : [])
            .Concat(effect.ChooserRef is { Kind: ProgramParticipantRef.EventTarget }
                ? [new RequireAnyContext(ProgramContextCapability.Damage | ProgramContextCapability.Judgment | ProgramContextCapability.HealthChange)] : [])
            .ToArray();

    private static void ValidateOptionCondition(SkillProgramCondition condition, string path)
    {
        if (condition.Kind is not (SkillProgramConditionKind.Always or SkillProgramConditionKind.Wounded or
            SkillProgramConditionKind.HpAtLeast or SkillProgramConditionKind.HandCountAtLeast or
            SkillProgramConditionKind.FaceDown or SkillProgramConditionKind.Chained or
            SkillProgramConditionKind.HasClaimableDamageCards or SkillProgramConditionKind.HasOwnedCardCategory or
            SkillProgramConditionKind.PublicCounterAtLeast or SkillProgramConditionKind.PublicCounterOdd or
            SkillProgramConditionKind.BoundCardCountAtLeast or
            SkillProgramConditionKind.BoundCardsMatchCategories or SkillProgramConditionKind.Expression or
            SkillProgramConditionKind.All or SkillProgramConditionKind.Any or SkillProgramConditionKind.Not))
            throw new InvalidOperationException($"Invalid skill program at {path}: option conditions require public chooser state.");
        foreach (var child in condition.Children) ValidateOptionCondition(child, path);
    }
}
