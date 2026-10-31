#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineProgramOrdinaryTrickUsesEntries
{
    internal void AddProgramOrdinaryTrickTargetCardOptions(ICollection<GameEngine.ProgramOrdinaryTrickUseOption> options, CharacterState source, CharacterState target, CardKind cardKind, LegalActionKind actionKind, string cardName) => _resolve().AddProgramOrdinaryTrickTargetCardOptions(options, source, target, cardKind, actionKind, cardName);
    internal IReadOnlyList<GameEngine.ProgramOrdinaryTrickUseOption> BuildProgramOrdinaryTrickUseOptions(CharacterState source, CardKind? outputKind = default, Suit? physicalSuit = default, bool excludeOwner = false, bool enforceUsePermission = false, Suit? beneficiaryShieldSuit = default, bool? actualEffectiveColor = default, bool hasActualColor = false, IReadOnlyList<int>? physicalCardIds = default, bool includeNextActualUseAdjustment = false, CardConversionSource? conversionSource = default) => _resolve().BuildProgramOrdinaryTrickUseOptions(source, outputKind, physicalSuit, excludeOwner, enforceUsePermission, beneficiaryShieldSuit, actualEffectiveColor, hasActualColor, physicalCardIds, includeNextActualUseAdjustment, conversionSource);
    internal void ResolveProgramOrdinaryTrickUseChoice(PromptChoice selected) => _resolve().ResolveProgramOrdinaryTrickUseChoice(selected);
    internal SkillProgramStepOutcome UseProgramAllHandCardsAsOrdinaryTrick(ProgramSkillFrame frame, string viewAsId) => _resolve().UseProgramAllHandCardsAsOrdinaryTrick(frame, viewAsId);
}
