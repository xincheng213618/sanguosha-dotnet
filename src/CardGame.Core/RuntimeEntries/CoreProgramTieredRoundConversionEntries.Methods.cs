#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreProgramTieredRoundConversionEntries
{
    internal bool CanUseTieredRoundConversion(CharacterState actor, IndexedSkillProgramInstance instance, SkillProgramViewAs rule, bool responseUse, bool dyingUse) => _resolve().CanUseTieredRoundConversion(actor, instance, rule, responseUse, dyingUse);
    internal void ConsumeTieredRoundConversion(CardConversionSource source) => _resolve().ConsumeTieredRoundConversion(source);
    internal void ObserveTrueRoundCardNames(IGameEvent payload) => _resolve().ObserveTrueRoundCardNames(payload);
    internal void RecordTrueRoundCardName(long origin, int actor, long? action, CardKind kind, bool counter) => _resolve().RecordTrueRoundCardName(origin, actor, action, kind, counter);
    internal bool TieredRoundConversionUnavailable(ProgramConfiguredViewAsQuery query) => _resolve().TieredRoundConversionUnavailable(query);
    internal int TieredRoundTier(int ownerSeat, string stateId) => _resolve().TieredRoundTier(ownerSeat, stateId);
    internal bool TrueRoundHasUsedName(CardKind kind) => _resolve().TrueRoundHasUsedName(kind);
    internal bool TryRecordTrueRoundDodgeUse(CardActionContext action) => _resolve().TryRecordTrueRoundDodgeUse(action);
    internal bool TracksTrueRoundCardNames { get => _resolve().TracksTrueRoundCardNames; }
}
