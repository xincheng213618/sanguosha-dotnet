namespace CardGame.Core;

// All visible card faces share the actual per-match projector and appearance cache.
internal interface IProgramCardSnapshotQueries : IProgramSelectionQuery
{
    CardSnapshot Snapshot(Card card);
    CardSnapshot JudgmentSnapshot(Card card);
    CardSnapshot EquipmentSnapshot(Card card, int viewerSeat, bool revealAll);
}
