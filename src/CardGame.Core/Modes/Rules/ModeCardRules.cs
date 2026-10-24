namespace CardGame.Core;

internal readonly record struct ModeAdditionalOrdinaryTrick(CardKind Kind, string BindingSuffix, int Batch = 0);

/// <summary>Mode-owned card names and conversion policies, fixed before this match builds any skill projections.</summary>
internal sealed class ModeCardRules
{
    internal static IReadOnlyList<CardKind> WenheOrdinaryTrickKinds { get; } = Array.AsReadOnly(new[]
        { CardKind.StarShift, CardKind.BeamSwap, CardKind.HandRedistribution, CardKind.WenheChaos });
    private readonly IModeOrdinaryTrickRule _ordinaryTricks;
    private readonly IModeBasicConversionRule? _basicConversions;
    private readonly IModeCardNameRule _cardNames;

    private ModeCardRules(bool projectsSkillPrograms, IModeOrdinaryTrickRule ordinaryTricks,
        IModeBasicConversionRule? basicConversions, IModeCardNameRule cardNames,
        IReadOnlyList<ModeAdditionalOrdinaryTrick> additionalOrdinaryTricks)
    {
        ProjectsSkillPrograms = projectsSkillPrograms;
        _ordinaryTricks = ordinaryTricks;
        _basicConversions = basicConversions;
        _cardNames = cardNames;
        AdditionalOrdinaryTricks = Array.AsReadOnly(additionalOrdinaryTricks.ToArray());
    }

    internal bool ProjectsSkillPrograms { get; }
    internal bool ProjectsBasicConversionFamilies => _basicConversions is not null;
    internal IReadOnlyList<ModeAdditionalOrdinaryTrick> AdditionalOrdinaryTricks { get; }
    internal CardKind? ReplaceOrdinaryTrick(CardKind kind) => _ordinaryTricks.Replace(kind);
    internal bool IsDeclaredCardNameAllowed(CardKind kind) => _cardNames.IsAllowed(kind) &&
        (!ProjectsSkillPrograms || ReplaceOrdinaryTrick(kind) == kind);
    internal bool ShouldProjectProgram(SkillProgram original) =>
        ProjectsSkillPrograms && original.Id != MoSimaYiComposition.Guifu;
    internal SkillProgram ProjectProgram(SkillProgram original) => ShouldProjectProgram(original)
        ? ModeSkillProgramProjection.Project(original, this) : original;

    internal bool TryReplaceBasicConversion(CardKind kind, out CardKind output)
    {
        if (_basicConversions is not null) return _basicConversions.TryReplace(kind, out output);
        output = kind;
        return false;
    }

    internal static ModeCardRules Create(ContentModeDefinition mode)
    {
        var additions = new List<ModeAdditionalOrdinaryTrick>();
        if (mode.HellActivity is not null) additions.Add(new(CardKind.ReturnSoul, ".activity-return-soul"));
        if (mode.WenheActivity is not null)
            additions.AddRange(WenheOrdinaryTrickKinds.Select(kind => new ModeAdditionalOrdinaryTrick(kind, ".activity-" + kind, 1)));
        return new(mode.ShenwuActivity is not null || mode.HulaoActivity is not null || mode.HellActivity is not null ||
                mode.WenheActivity is not null || mode.DragonBoatActivity is not null || mode.GuanduActivity is not null,
            SelectOrdinaryTricks(mode), SelectBasicConversions(mode),
            mode.Id == WushuangMode.ModeId ? new WushuangCardNameRule() : new UnrestrictedCardNameRule(), additions);
    }

    private static IModeOrdinaryTrickRule SelectOrdinaryTricks(ContentModeDefinition mode)
    {
        // Existing priority is significant when validating a composed fixture or an activity recipe.
        if (mode.ShenwuActivity is not null) return new ShenwuOrdinaryTrickRule();
        if (mode.HulaoActivity is not null) return new HulaoOrdinaryTrickRule();
        if (mode.GuanduActivity is not null) return new GuanduOrdinaryTrickRule();
        return mode.DragonBoatActivity is not null ? new DragonBoatOrdinaryTrickRule() : new OriginalOrdinaryTrickRule();
    }

    private static IModeBasicConversionRule? SelectBasicConversions(ContentModeDefinition mode)
    {
        if (mode.GuanduActivity is not null) return new GuanduBasicConversionRule();
        return mode.DragonBoatActivity is not null ? new DragonBoatBasicConversionRule() : null;
    }
}

internal interface IModeOrdinaryTrickRule { CardKind? Replace(CardKind kind); }
internal sealed class OriginalOrdinaryTrickRule : IModeOrdinaryTrickRule
{
    public CardKind? Replace(CardKind kind) => kind;
}
internal sealed class ShenwuOrdinaryTrickRule : IModeOrdinaryTrickRule
{
    public CardKind? Replace(CardKind kind) => kind switch
    { CardKind.DrawTwo => CardKind.SowBeans, CardKind.BorrowedSword => CardKind.TransplantWood, _ => kind };
}
internal sealed class HulaoOrdinaryTrickRule : IModeOrdinaryTrickRule
{
    public CardKind? Replace(CardKind kind) => kind switch
    { CardKind.FiveGrains => CardKind.AllianceFeast, CardKind.PeachGarden => null, _ => kind };
}
internal sealed class GuanduOrdinaryTrickRule : IModeOrdinaryTrickRule
{
    public CardKind? Replace(CardKind kind) => kind switch
    {
        CardKind.FiveGrains => CardKind.StoreGrain, CardKind.PeachGarden => CardKind.Reinforcements,
        CardKind.BarbarianAssault => CardKind.UnitedEffort, _ => kind
    };
}
internal sealed class DragonBoatOrdinaryTrickRule : IModeOrdinaryTrickRule
{
    public CardKind? Replace(CardKind kind) => kind switch
    {
        CardKind.DrawTwo => CardKind.SameBoat, CardKind.FiveGrains => CardKind.StriveUpstream,
        CardKind.PeachGarden => null, CardKind.BarbarianAssault => CardKind.AgainstCurrent,
        CardKind.ArrowBarrage => null, _ => kind
    };
}

internal interface IModeBasicConversionRule { bool TryReplace(CardKind kind, out CardKind output); }
internal sealed class GuanduBasicConversionRule : IModeBasicConversionRule
{
    public bool TryReplace(CardKind kind, out CardKind output)
    { output = kind == CardKind.Alcohol ? CardKind.Intoxication : kind; return output != kind; }
}
internal sealed class DragonBoatBasicConversionRule : IModeBasicConversionRule
{
    public bool TryReplace(CardKind kind, out CardKind output)
    {
        output = kind switch { CardKind.Peach => CardKind.Zong, CardKind.Alcohol => CardKind.RealgarWine, _ => kind };
        return output != kind;
    }
}

internal interface IModeCardNameRule { bool IsAllowed(CardKind kind); }
internal sealed class UnrestrictedCardNameRule : IModeCardNameRule
{
    public bool IsAllowed(CardKind kind) => true;
}
internal sealed class WushuangCardNameRule : IModeCardNameRule
{
    public bool IsAllowed(CardKind kind) => WushuangCatalogue.IsCardAllowed(kind);
}
