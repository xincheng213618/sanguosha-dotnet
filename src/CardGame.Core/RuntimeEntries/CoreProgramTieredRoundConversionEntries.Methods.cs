#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreProgramTieredRoundConversionEntries
{
    internal bool CanUseTieredRoundConversion(global::CardGame.Core.CharacterState actor, global::CardGame.Core.IndexedSkillProgramInstance instance, global::CardGame.Core.SkillProgramViewAs rule, bool responseUse, bool dyingUse) => _resolve().CanUseTieredRoundConversion(actor, instance, rule, responseUse, dyingUse);
    internal void ConsumeTieredRoundConversion(global::CardGame.Core.CardConversionSource source) => _resolve().ConsumeTieredRoundConversion(source);
    internal void ObserveTrueRoundCardNames(global::CardGame.Core.IGameEvent payload) => _resolve().ObserveTrueRoundCardNames(payload);
    internal void RecordTrueRoundCardName(long origin, int actor, long? action, global::CardGame.Core.CardKind kind, bool counter) => _resolve().RecordTrueRoundCardName(origin, actor, action, kind, counter);
    internal bool TieredRoundConversionUnavailable(global::CardGame.Core.ProgramConfiguredViewAsQuery query) => _resolve().TieredRoundConversionUnavailable(query);
    internal int TieredRoundTier(int ownerSeat, string stateId) => _resolve().TieredRoundTier(ownerSeat, stateId);
    internal bool TrueRoundHasUsedName(global::CardGame.Core.CardKind kind) => _resolve().TrueRoundHasUsedName(kind);
    internal bool TryRecordTrueRoundDodgeUse(global::CardGame.Core.CardActionContext action) => _resolve().TryRecordTrueRoundDodgeUse(action);
    internal bool TracksTrueRoundCardNames { get => _resolve().TracksTrueRoundCardNames; }
}
