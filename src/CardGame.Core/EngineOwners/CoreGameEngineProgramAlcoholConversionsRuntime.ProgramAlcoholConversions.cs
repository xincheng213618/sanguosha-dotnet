#nullable enable

namespace CardGame.Core.EngineOwners;
internal sealed partial class CoreGameEngineProgramAlcoholConversionsRuntime
{
    internal void ValidateAlcoholConversion(CharacterState owner, Card card, CardConversionSource? source, bool forResponse)
    {
        if (_characterGuanYuWumiaoOperations.IsWuyouIdentityMaterial(owner, card, CardKind.Alcohol, source))
            return;
        if (source is null ? card.Kind != CardKind.Alcohol || _coreCardConversionsOperations.HasProgramCardIdentity(owner, card) : !_coreActionQueriesOperations.GetProgramViewAsConversions(owner, card, CardKind.Alcohol, forResponse, dyingUse: forResponse && _coreDyingCursorRuntimeOperations.ActiveDying?.VictimSeat == owner.Seat).Contains(source))
            throw new InvalidOperationException("The Alcohol conversion has no matching enabled program source.");
    }
}
