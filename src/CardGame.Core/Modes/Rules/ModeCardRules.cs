namespace CardGame.Core;

public readonly record struct ModeAdditionalOrdinaryTrick(CardKind Kind, string BindingSuffix, int Batch = 0);

/// <summary>Mode-owned card names and conversion policies, fixed before this match builds any skill projections.</summary>
public sealed class ModeCardRules
{
    private readonly IModeOrdinaryTrickRule _ordinaryTricks;
    private readonly IModeBasicConversionRule? _basicConversions;
    private readonly IModeCardNameRule _cardNames;
    private readonly HashSet<string> _unprojectedProgramIds;
    private readonly HashSet<CardKind> _disabledOrdinaryTrickKinds;
    private readonly IModeCardRecastRule? _recasts;
    private readonly HashSet<CardKind> _basicPlayConversionKinds;

    public ModeCardRules(bool projectsSkillPrograms, IModeOrdinaryTrickRule ordinaryTricks,
        IModeBasicConversionRule? basicConversions, IModeCardNameRule cardNames,
        IReadOnlyList<ModeAdditionalOrdinaryTrick> additionalOrdinaryTricks,
        IEnumerable<string>? unprojectedProgramIds = null, IEnumerable<CardKind>? disabledOrdinaryTrickKinds = null,
        IModeCardRecastRule? recasts = null, IEnumerable<CardKind>? basicPlayConversionKinds = null)
    {
        ProjectsSkillPrograms = projectsSkillPrograms;
        _ordinaryTricks = ordinaryTricks ?? throw new ArgumentNullException(nameof(ordinaryTricks));
        _basicConversions = basicConversions;
        _cardNames = cardNames ?? throw new ArgumentNullException(nameof(cardNames));
        _unprojectedProgramIds = new(unprojectedProgramIds ?? [], StringComparer.Ordinal);
        _disabledOrdinaryTrickKinds = new(disabledOrdinaryTrickKinds ?? []);
        _recasts = recasts;
        HasHandEquipmentRecast = recasts is not null && Enum.GetValues<EquipmentSlot>().Any(recasts.AllowsHandEquipment);
        BasicPlayConversionKinds = Array.AsReadOnly((basicPlayConversionKinds ?? []).Distinct().ToArray());
        _basicPlayConversionKinds = new(BasicPlayConversionKinds);
        AdditionalOrdinaryTricks = Array.AsReadOnly(additionalOrdinaryTricks.ToArray());
    }

    public bool ProjectsSkillPrograms { get; }
    public bool ProjectsBasicConversionFamilies => _basicConversions is not null;
    public IReadOnlyList<ModeAdditionalOrdinaryTrick> AdditionalOrdinaryTricks { get; }
    public IReadOnlyList<CardKind> BasicPlayConversionKinds { get; }
    public bool HasHandEquipmentRecast { get; }
    public bool AllowsBasicPlayConversion(CardKind outputKind) => _basicPlayConversionKinds.Contains(outputKind);
    public bool AllowsHandEquipmentRecast(EquipmentSlot slot) => _recasts?.AllowsHandEquipment(slot) == true;
    public bool AllowsNativePhysicalRecast(CardKind physicalKind, CardKind effectiveKind) =>
        _recasts?.AllowsNativePhysicalCard(physicalKind, effectiveKind) == true;
    public CardKind? ReplaceOrdinaryTrick(CardKind kind) => _ordinaryTricks.Replace(kind);
    public bool IsCardAllowed(CardKind kind) => _cardNames.IsAllowed(kind);
    public bool IsDeclaredCardNameAllowed(CardKind kind) => IsCardAllowed(kind) &&
        (!ProjectsSkillPrograms || ReplaceOrdinaryTrick(kind) == kind);
    public bool AllowsOrdinaryTrickOption(CardKind kind, CardKind? explicitOutputKind = null) =>
        IsCardAllowed(kind) && (explicitOutputKind is { } explicitKind ? explicitKind == kind :
            !_disabledOrdinaryTrickKinds.Contains(kind) && IsDeclaredCardNameAllowed(kind));
    public bool ShouldProjectProgram(SkillProgram original) =>
        ProjectsSkillPrograms && !_unprojectedProgramIds.Contains(original.Id);
    public SkillProgram ProjectProgram(SkillProgram original) => ShouldProjectProgram(original)
        ? ModeSkillProgramProjection.Project(original, this) : original;

    public bool TryReplaceBasicConversion(CardKind kind, out CardKind output)
    {
        if (_basicConversions is not null) return _basicConversions.TryReplace(kind, out output);
        output = kind;
        return false;
    }

}

public interface IModeOrdinaryTrickRule { CardKind? Replace(CardKind kind); }
public interface IModeBasicConversionRule { bool TryReplace(CardKind kind, out CardKind output); }
public interface IModeCardNameRule { bool IsAllowed(CardKind kind); }
public interface IModeCardRecastRule
{
    bool AllowsHandEquipment(EquipmentSlot slot);
    bool AllowsNativePhysicalCard(CardKind physicalKind, CardKind effectiveKind);
}
