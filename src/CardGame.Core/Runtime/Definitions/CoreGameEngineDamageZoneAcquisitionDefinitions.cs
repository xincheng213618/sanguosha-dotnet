#nullable enable
using global::CardGame.Core;
using System.Globalization;

namespace CardGame.Core.EngineOwners;
internal static class CoreGameEngineDamageZoneAcquisitionDefinitions
{
internal static readonly CardZoneKind[] DamageAcquisitionZones = [CardZoneKind.Hand, CardZoneKind.Equipment, CardZoneKind.Judgment];
}
