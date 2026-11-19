# Native materials and physical movement

Required when changing these mechanisms or their consumers. See the
[contract index](README.md) for related topics.

## Representations and registered entities

- Native card representations use `INativeCardRepresentations` and the original `CardUseFrame`: freeze the issued zero-entity face, retain physical printed faces separately from effective action appearance, and declare real carriers explicitly. Actual destruction/payment owners capture exact material departures from the physical journal; representation reads never reconstruct character proofs or authorize material movement. Existing claim and native-return protocols remain separate owners. A Card value without a registered zone has no physical-entity relation: `ICardZoneQueries.TryGetLocation` returns false, never an `OutsideGame` fallback; `GetLocation`, writes and payments remain strict.

## Claims, deliveries and retirement

- Actual damage-card claims are captured once on the original CardUseFrame by CardUseMaterialClaimRuntime from physical movement delivery. Extra-effect episodes do not copy claim ledgers; separate the persistent claim fact from current material-retirement destinations, and retain non-CardUse damage owners' own protocols.

- A damage-card claim may retain its skill invocation once the applied damage's physical cards reach the recipient's hand. The damage window grants that specific child permission to finish after skill loss. An empty claim grants no permission, and the owner's death still stops outstanding skill effects.

- Native material dispositions share `INativeMaterialClaims` and the original CardUse/Judgment frame. Keep accepted claims, issued transfers, conditional child reservations, non-Processing retained reads and actual deliveries distinct; only actual movements carry delivery sequences. Child reservations settle from the original physical entry before typed business returns. Read representation, relinquished retirement ownership and completion destination permission separately; reentered Processing must not be retired twice, and default damage claims must not gain Equipment/Judgment/Outside completion permission. Paid source-loss continuations reuse the original window's ProgramInvocations, independently of owner-death permission.

- Actual native material deliveries use `INativeMaterialClaims` and the original use or delayed Judgment frame's `MaterialClaims`. Capture real journal deliveries once; distinguish representation residency from the original owner's retirement right, which is relinquished after delivery even if that entity later reenters Processing. Keep accepted promises, child handoffs and producer retention separate from delivered acquisition; no character proof scan may replace this shared delivery fact.

- A pair Pindian retains the source's original draw-pile reservation sequence on its own frame. Its unrevealed source and any replacement that returns the reserved card must still refer to that Processing entry. A read of the opponent's draw-pile top leaves it in the draw pile until payment. Simultaneous Pindian keeps its own reservation receipts, and paid reveal and result claims retain their own lifetimes.

- A pair Pindian returns the discard-pile entries actually delivered by its own cleanup with the native result binding. Each side has a separate receipt. A card obtained during an earlier window has no cleanup receipt even if a later discard moves the same physical card into the pile. A consumer rechecks the exact entry before acquiring it.

## Native-use material retirement

- Record actual non-Processing payment or destruction through `INativeMaterialClaims.CaptureRetirement` on the original `CardUseFrame.MaterialClaims`. A `Retirement` has no acquisition recipient and does not contribute to `WasClaimed` or claimed entities. A `Delivery` retains its acquisition and residency semantics. Ordinary payment into Processing issues no retirement receipt.
- Validate original material, issuance cutoff, payment source and actual journal movements at the shared write boundary. For direct payment into OutsideGame, a deck or a public pile, use that payment as the causal movement. For material in Processing, accept the first departure from the original entry and a real destruction immediately following that departure into DiscardPile. Duplicate records are idempotent. Conflicting or fabricated batches leave all rights unchanged.
- When a real child use starts with material already in Processing, retain the actual entry sequence from before issuance. A zero-entity use's physical payment remains a resource cost even when it shares the same Action. Physical lifetime writers supply the exact departure. The shared owner selects the nearest real use tied to that entry and issues no receipt when no native material matches. Later discards, responses or reentry into Processing cannot record a new retirement on a prior owner that relinquished the material.
- `HasRelinquishedMaterial` and `PreservesMaterialAtCompletion` keep the original owner from cleaning up that material again, including after reentry into Processing. For a retirement receipt, `CanReadMaterial` permits representation reads at the captured destination only. Payment, acquisition and movement require their own authorization.
- Native producers capture the exact outcome for common Slash payments, 吴景's real trick payment, selected foreign cards, 管亥's non-response public-pile payment and actual destruction of general cards, chariots, soul cards and ephemeral entities. The common completion, sequential trick, group, Amazing Grace, delayed-placement and material-eligibility paths read those facts. Issuance qualification, payment child windows and typed business returns stay with their producers.

The shared receipt covers actual CardUse payment and destruction. Pure response
materials and delayed Judgment destruction keep their own scopes. Rule-specific
native issuance and business returns remain separate composition work, including
dependency direction in the rest of native card-use scheduling.

## Acquisition, Give and exact movement requests

- Whole discard-phase settlement reads genuine discards from the actual phase's starting movement sequence through the original physical journal. Freeze eligible discard-pile entries on the issuing program frame. Recheck the original entry when moving a card. Returning a card to a player settles its child windows before a later decision claims the remaining entries.

- Discard and judgment provenance claims use the original movement entry at collection, candidate admission and execution. Reentering the discard pile gives the physical card a new entry and ends the previous opportunity.

- `claimMovedCards` with `discardPileReceived`, `movementDiscardOnly` and `perBatch` claims the matching entries of the original discard batch in one physical movement. Selection retains the original batch window, and execution rechecks each original entry at the discard pile after earlier effects such as HP payment.

- True-discard batch claims can use a completed logical discard receipt with the same card, physical destination and movement sequence when the physical reason classifier has no source. Synchronous movement capture uses the facts already available at that point. The claim window can read the completed receipt after the native producer returns.

- Keep logical acquisition collections separate from physical movement batches. Freeze acquired entities, original-hand witnesses and each required source time in shared receipts on the owning batch; characters consume those facts instead of rebuilding them with completion callbacks or private history. Typed replacement provenance stays at its native boundary, and acquisition grouping must not merge or discard ordinary movement windows. Captured invocation permission belongs to its original window and program, independently of skill possession and effectiveness; source-loss permission must not implicitly grant owner-death permission. Read-only card captures do not acquire temporary-material cleanup ownership; derived bindings preserve that ownership and private reads stay within their authorized participant scope. Read movement outcomes from original physical facts, separately from requested materials and logical discard semantics.

- Native Give facts belong to `NativeGivenCardRuntime` and the original journal, with scope interpreted by `NativeGiveScopeRuntime`. Declare logical giving and its actor in rule data; do not infer Give from movement zones, reason strings, gains or a character whitelist. Logical Give triggers declare perNativeGive on the original movement window; keep logical occurrences separate from physical completion indexes and derive capture demand from the declaration. Typed gain settlements keep their original producer contract; do not rebuild character ledgers or re-prove replacement issuance when reading delivered facts.

- Exact owned-card movement requests reuse the original OwnedCardSelection draft and typed SelectedCardPayment. Issue one request only after a full legal selection; distinguish accepted selection from actual delivery, including redirected, destroyed and retained outcomes. History-based card identities do not grant private face access or temporary-material cleanup ownership.
