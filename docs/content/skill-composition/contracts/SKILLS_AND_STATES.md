# Skills and applied states

Required when changing these mechanisms or their consumers. See the
[contract index](README.md) for related topics and the
[capability map](../CAPABILITY_MAP.md) for completion status and remaining gaps.

## Skill changes and availability

- Skill upgrades, persistent upgrade flags, and skill grants/replacements reuse `Runtime/Rules/ProgramSkillChangeRuntime.cs` and the frame-aware helpers in `EngineOwners/CoreGameEngineProgramSkillChangesRuntime.ProgramSkillChanges.cs` through the existing program nodes. Keep rule-specific trigger, cost, and phase qualification outside the shared mutation layer; do not add a turn/round prerequisite to skill changes themselves. Keep paid child state and returns on the original frame. New generals that reuse these operations inherit the shared `Skill changes:` checks instead of copying an upgrade implementation or fixture.

- Ordinary skill suppression, loss and borrowing use `skillState` / `ProgramSkillChangeRuntime`; `SkillAvailability` answers possession separately from effectiveness and `MatchSkillBindingIndex` only caches that answer. Invocation permission belongs to its actual window. Preserve and record distinct remaining lifetimes, continuous predicates and invocation-scoped rules in `CAPABILITY_MAP.md` while migrating them; do not extend legacy character-specific state stores or add another validity answer.

- Skill validity must be derived from declared data in one owner. Do not replace character qualification callbacks with a predicate registry, interface list or new callbacks. Store restrictions separately from real skill grants, use one matching function, and keep window-scoped invocation authorization separate from possession and effectiveness.

- Raw skill-source admission uses `SkillAvailability` for selection, reveal, identity concealment and immutable mode content limits; preserve possession separately from effective admission. Applied equipment-skill restrictions use the same recipient restriction table and metadata matcher, including future equipment grants; real equipment-effect and draw-penalty rules keep their own facts.

- Source-dependent skill grants use declared `skillGrants` conditions, exact `ConditionalSource` relations and `ConditionalSkillGrantRuntime`; `SkillAvailability` interprets their eligibility for both reads and synchronization. Retain independent grants and the original dependent instance across condition exits. Source-loss invocation permission stays on the original frame, separately from ongoing source eligibility; recipient restriction conditions must not recurse through source-dependent skill queries.

## Applied state lifetimes

- Applied states outlive their caster and original skill. After application, evaluate the recipient's declared conditions, lifetime and removal rules; do not revoke a state merely because its caster died or lost the skill. Source qualification controls new applications and future windows; it does not requalify an already applied state. Read HP through the existing match health query, including its mode conversions and selected health receipts.

## Physical marks and hand limits

- Permanent physical card marks use `CardEntityMarkRuntime`, `markInitialHand` / `markCards`, and the shared `cardMarkCount` read; keep mark identity independent of holder, zone, source skill and presentation label. Holder-dependent benefits remain separate rules. Printed Lord qualification belongs to the actual grant as typed setup/program source data and uses the single `PrintedLordSkillQualifications` matcher.

- Hand-limit exemptions use `IHandLimitCounting` and the single `IHandLimitExemptions` card matcher. Continuing benefits declare `HandLimitMatch` on the existing card policy and retain their real effective-source qualification; applied recipient relations are issued from the original typed outcome or physical delivery and do not requalify that source later. Keep permanent physical marks, the holder's continuing benefit, and an issued original-recipient entitlement separate. Marks and relation membership do not grant card-use permission, private face access, or another holder's exemption; related mark and legality readers consume the same shared relation instead of a private grant ledger or event proof. A matching card face alone cannot give an unregistered identity an issued physical relation or Hand entitlement.

- Declare issued hand-limit lifetimes precisely: frozen actual-turn coordinates, explicitly required actual-turn status, permanent identity, current Hand entry, and `SkillStateLifetime` are different contracts. Freeze an entry when the real keep decision applies it; a card leaving and reentering may restore an identity relation but cannot revive an entry-scoped one. Legacy native turn-end, next-owner-turn-start and death expiry stays at its original native cleanup call through `IHandLimitExemptionNativeLifetimes`, using the same global turn-effect grant sequence and terminal canonical record. Do not fold these into an earlier notification boundary or let legacy cleanup expire coordinate-only relations. Public effect projection exposes only its existing declared recipe surfaces, not new private entity counts; real generated-card authorization must not acquire a fake program frame.

## Judgment attachments and generated equipment

- Judgment-area attachments use the shared attachment fact and physical movement lifetime. Slot occupancy is separate from delayed-card identity; direct judgment-area transfers preserve it, leaving ends it, and source loss does not revoke an issued attachment. Character code does not rebuild slot or payment proof ledgers.

- Generated equipment uses `EquipmentCardOrigin` and `EquipmentDestinationPolicy` through `GeneratedEquipmentRuntime`; random pile selection, private generation offers and native placement reuse the equipment composition nodes. Equipment's own entry/departure triggers declare `ownEquipmentOnly`; departing sources freeze before movement and reuse `ProgramInvocationAuthorization` on the owning movement window, never a replacement skill grant or character ledger.
