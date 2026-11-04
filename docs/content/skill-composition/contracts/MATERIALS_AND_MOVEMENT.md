# Native materials and physical movement

Required when changing these mechanisms or their consumers. See the
[contract index](README.md) for related topics and the
[capability map](../CAPABILITY_MAP.md) for completion status and remaining gaps.

## Representations and registered entities

- Native card representations use `INativeCardRepresentations` and the original `CardUseFrame`: freeze the issued zero-entity face, retain physical printed faces separately from effective action appearance, and declare real carriers explicitly. Actual destruction/payment owners capture exact material departures from the physical journal; representation reads never reconstruct character proofs or authorize material movement. Existing claim and native-return protocols remain separate owners. A Card value without a registered zone has no physical-entity relation: `ICardZoneQueries.TryGetLocation` returns false, never an `OutsideGame` fallback; `GetLocation`, writes and payments remain strict.

## Claims, deliveries and retirement

- Actual damage-card claims are captured once on the original CardUseFrame by CardUseMaterialClaimRuntime from physical movement delivery. Extra-effect episodes do not copy claim ledgers; separate the persistent claim fact from current material-retirement destinations, and retain non-CardUse damage owners' own protocols.

- Native material dispositions share `INativeMaterialClaims` and the original CardUse/Judgment frame. Keep accepted claims, issued transfers, conditional child reservations, non-Processing retained reads and actual deliveries distinct; only actual movements carry delivery sequences. Child reservations settle from the original physical entry before typed business returns. Read representation, relinquished retirement ownership and completion destination permission separately; reentered Processing must not be retired twice, and default damage claims must not gain Equipment/Judgment/Outside completion permission. Paid source-loss continuations reuse the original window's ProgramInvocations, independently of owner-death permission.

- Actual native material deliveries use `INativeMaterialClaims` and the original use or delayed Judgment frame's `MaterialClaims`. Capture real journal deliveries once; distinguish representation residency from the original owner's retirement right, which is relinquished after delivery even if that entity later reenters Processing. Keep accepted promises, child handoffs and producer retention separate from delivered acquisition; no character proof scan may replace this shared delivery fact.

- Native-use payment and destruction retirements use `INativeMaterialClaims.CaptureRetirement` on the original `CardUseFrame`, with exact journal movements and original payment/Processing-entry identity. Physical lifetime writers submit their exact departure to this owner instead of searching character-marked ancestors. Retirement permanently relinquishes that owner's cleanup right without becoming an acquisition; representation reads retain only the captured destination. Do not rebuild per-character removed-material proofs or apply these use receipts to pure responses or delayed Judgment destruction.

## Acquisition, Give and exact movement requests

- Keep logical acquisition collections separate from physical movement batches. Freeze acquired entities, original-hand witnesses and each required source time in shared receipts on the owning batch; characters consume those facts instead of rebuilding them with completion callbacks or private history. Typed replacement provenance stays at its native boundary, and acquisition grouping must not merge or discard ordinary movement windows. Captured invocation permission belongs to its original window and program, independently of skill possession and effectiveness; source-loss permission must not implicitly grant owner-death permission. Read-only card captures do not acquire temporary-material cleanup ownership; derived bindings preserve that ownership and private reads stay within their authorized participant scope. Read movement outcomes from original physical facts, separately from requested materials and logical discard semantics.

- Native Give facts belong to `NativeGivenCardRuntime` and the original journal, with scope interpreted by `NativeGiveScopeRuntime`. Declare logical giving and its actor in rule data; do not infer Give from movement zones, reason strings, gains or a character whitelist. Logical Give triggers declare perNativeGive on the original movement window; keep logical occurrences separate from physical completion indexes and derive capture demand from the declaration. Typed gain settlements keep their original producer contract; do not rebuild character ledgers or re-prove replacement issuance when reading delivered facts.

- Exact owned-card movement requests reuse the original OwnedCardSelection draft and typed SelectedCardPayment. Issue one request only after a full legal selection; distinguish accepted selection from actual delivery, including redirected, destroyed and retained outcomes. History-based card identities do not grant private face access or temporary-material cleanup ownership.
