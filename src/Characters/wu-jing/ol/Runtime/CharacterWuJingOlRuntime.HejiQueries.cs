#nullable enable

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterWuJingOlRuntime
{
    internal bool WuJingHejiIncoming(CardUseFrame use) => use.Action is { Type: CardActionType.Use } a && use.TargetSeats is [var target] && a.EffectiveDesignatedTargetSeats.SequenceEqual([target]) && _coreMatchParticipantsPlayersAccess._players[target].IsAlive && (use.CardKind == CardKind.Duel || CoreNativeCardClassificationsRuntime.IsSlashCard(use.CardKind) && a.EffectiveIsRed == true);
}
