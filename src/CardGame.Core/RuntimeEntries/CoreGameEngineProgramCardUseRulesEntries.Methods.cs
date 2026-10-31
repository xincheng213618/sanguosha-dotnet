#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineProgramCardUseRulesEntries
{
    internal void AddProgramTargetCountSlashActions(ICollection<LegalAction> actions, CharacterState actor, Card physicalCard, IReadOnlyList<CharacterState> legalTargets, string slashName, CardKind? playedCardKind, CardConversionSource? conversionSource = default, IReadOnlyList<CardConversionSource>? additionalConversionSources = default) => _resolve().AddProgramTargetCountSlashActions(actions, actor, physicalCard, legalTargets, slashName, playedCardKind, conversionSource, additionalConversionSources);
    internal string DescribeAdditionalConversion(CardConversionSource source) => _resolve().DescribeAdditionalConversion(source);
    internal IReadOnlyList<CardConversionSource> GetProgramChainedViewAsConversions(CharacterState owner, Card physicalCard, CardKind inputKind, CardKind outputKind, CardConversionSource primaryConversionSource, bool forResponse) => _resolve().GetProgramChainedViewAsConversions(owner, physicalCard, inputKind, outputKind, primaryConversionSource, forResponse);
    internal IReadOnlyList<SlashUseVariant> GetSlashUseVariants(CharacterState actor, Card physicalCard, CardKind baseEffectiveKind, CardConversionSource? primaryConversionSource) => _resolve().GetSlashUseVariants(actor, physicalCard, baseEffectiveKind, primaryConversionSource);
    internal IReadOnlyList<SlashUseVariant> GetSlashUseVariants2(CharacterState actor, CardKind baseEffectiveKind) => _resolve().GetSlashUseVariants(actor, baseEffectiveKind);
    internal bool HasNativeExtraSlashTargets(CharacterState actor, Card physical, CardKind kind) => _resolve().HasNativeExtraSlashTargets(actor, physical, kind);
    internal bool UsesProgramCardTargetCount(CharacterState source, Card physicalCard, CardKind effectiveKind, int targetCount) => _resolve().UsesProgramCardTargetCount(source, physicalCard, effectiveKind, targetCount);
}
