namespace CardGame.Core;
public sealed partial class GameEngine
{
    internal readonly record struct NuzhanModifiers(bool IgnoresSlashLimit, int DamageBonus, int PhysicalCardId);
    internal sealed record ProgramCardIdentityMatch(SkillProgram Program, SkillProgramCardIdentity Identity, CardConversionSource Source);
    private ref global::CardGame.Core.CardConversionSource? _selectedResponseConversion => ref _cardConversions.ReferenceToSelectedResponseConversion;
    private ref global::CardGame.Core.CardConversionSource? _selectedUseConversion => ref _cardConversions.ReferenceToSelectedUseConversion;
    private ref bool _hasSelectedResponseConversionChoice => ref _cardConversions.ReferenceToHasSelectedResponseConversionChoice;
    private ref bool _hasSelectedUseConversionChoice => ref _cardConversions.ReferenceToHasSelectedUseConversionChoice;
}
