namespace CardGame.Core;

internal sealed record SlashUseVariant(
    CardKind EffectiveKind,
    CardConversionSource? AdditionalConversionSource,
    bool UsesZhuqueFan);
