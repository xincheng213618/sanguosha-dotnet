# 表达式条件

本页说明效果步骤的 `{"kind":"expression"}` 条件和整数 `amountExpr`，以及用它们替代人物专用操作的迁移方向。入口与其他条件见[技能配置、条件与顺序步骤](RUNTIME_CONFIGURATION.md)。

## 为什么需要

人物专用操作中有相当一部分只是“通用操作 + 一个专门的判断或数量”：例如“目标手牌少于体力上限时本人摸一张”“体力唯一最高时翻面”。过去每个这样的判断都新增一个 `SkillProgramEffectOp` 或 `SkillProgramNumberExpression` 枚举值，再写解析、组合校验、执行和返回凭据，一个武将常常因此多出数 KB 的 C#。

表达式条件把这类判断写回 JSON：执行器已经在提交步骤游标后、执行操作前统一检查效果的 `condition`，条件为假则跳过该步骤。所以凡是接受 `condition` 的操作，都可以直接使用表达式，不需要为每个操作增加代码。

## 写法

```json
{ "op": "draw", "target": "owner", "amount": 1,
  "condition": { "kind": "expression", "expr":
    ["and", ["alive", "selectedTarget"], ["<", ["handCount", "selectedTarget"], ["maxHp", "selectedTarget"]]] } }
```

`expr` 是 `[运算符, 参数...]` 形式的 JSON 数组，必须是布尔值。条件对象只接受 `kind` 与 `expr`。

| 类型 | 写法 |
| --- | --- |
| 布尔字面量 | `true`、`false` |
| 整数字面量 | -99 到 99 |
| 参与者 | `"owner"`、`"selectedTarget"`（恰好一名已选目标）、`"eventSource"`、`"eventTarget"`；量词内的 `"it"` |
| 参与者集合 | `"living"`（存活角色，座位顺序）、`"livingOthers"`（除拥有者外的存活角色） |
| 逻辑 | `["and", b, b, ...]`、`["or", b, b, ...]`（2–8 项）、`["not", b]` |
| 比较 | `["<" / "<=" / ">" / ">=" / "==" / "!=", i, i]`、`["same", p, p]` |
| 角色状态 | 布尔：`alive`、`hasHp`、`faceDown`、`chained`；整数：`hp`、`maxHp`、`lostHp`、`handCount`、`handLimit`、`equipmentCount` |
| 窗口冻结事实 | `["windowHp", "owner" / "eventTarget"]`、`["windowHandCount", "owner" / "eventTarget"]` 读取原触发窗口的体力或手牌数；不读取后来子结算改变的现值 |
| 运算 | `["+", i, ...]`、`["min", i, ...]`、`["max", i, ...]`（2–8 项）、`["-", i, i]`、`["floorDiv", i, i]`；`["when", b, i]` 在 b 为真时取 i，否则未知 |
| 绑定读取 | 整数：`["number", "name"]`、`["boundCount", "cards"]`、`["boundSuitCount", "cards"]`、`["boundDistinctSuitCount", "cards"]`、`["boundRank", "singleCard"]`；布尔：`["boundIsEquipment", "singleCard"]` |
| 物理牌标记 | `["cardMarkCount", p, "mark-id", "hand"]`，牌区也可为 `"equipment"`；按当前牌区读取永久实体标记 |
| 手牌印刷花色 | `["handPrintedSuitCount", p, "spade"]`，花色也可为 heart/club/diamond/none；只数当前手牌的实体印刷花色，不应用花色改写 |
| 条件授予事实 | `["ruleWounded", p]` 读规则受伤；`["equipmentSuitCount", p]` 读独立来源改写后的装备花色数；`["publicPileCount", p, "skill:id"]` 读本人该技能全部来源的公共堆实体数 |
| 量词 | `["all", set, b]`、`["any", set, b]` 返回布尔；`["count", set, b]` 返回整数；`b` 内用 `"it"` 表示当前成员 |

加载时检查运算符、参数个数、类型、字面量范围、嵌套深度（8 层）以及 `"it"` 只能出现在量词谓词中。表达式随规则 JSON 规范化后进入技能玩法指纹，空白不影响指纹；新增该可选节点不改变规则格式版本。

## 求值语义

- 普通角色状态在步骤执行时读取当前公开状态；先完成的摸牌、移动及其子结算会影响后续步骤的判断。显式 `windowHp` / `windowHandCount` 使用原 `WindowContext.Facts`；缺少窗口/目标或体力未定义、尚未选择体力池时返回未知。AI 也只使用提供给它的同一冻结事实，没有窗口时不猜测。
- 三值逻辑：缺失的参与者（例如未选目标、事件没有来源）、未定义体力（`hp`、`lostHp`）以及未知集合都是“未知”。比较与运算遇到未知返回未知；`not` 保持未知；`and` 遇到确定的假即为假，`or` 遇到确定的真即为真；整个条件最终为未知时按假处理并跳过步骤。
- 因此需要“没有体力值的角色不参与比较”时要明确写出，例如 `["or", ["not", ["hasHp", "it"]], ["<", ["hp", "it"], ["hp", "owner"]]]`。
- 表达式只读，不支付、不移动、不发事件。拥有者死亡、失去技能实例和终局仍由执行器统一处理。
- AI 预估只知道拥有者与已选目标的公开状态；其他参与者和集合视为未知，所以依赖它们的步骤在估值中按不发动处理。
- `handLimit` 读取实际规则计算后的手牌上限；AI 只在公开上下文提供该值时使用。`floorDiv` 按数学意义向下取整，除零或商超出整数范围返回未知。未选定独立体力池而两个体力值不同时，`hp`、`lostHp` 也返回未知，不能默认选择其中一人；这与隐匿角色的 `hasHp=false` 不同。

## 适用范围

布尔表达式可用于效果步骤的 `condition`（包括 `all`/`any`/`not` 组合内）。`skillStates` 的 `when` / `while` 与 `skillState.match.filter` 使用同一求值器，加载器分别限制为该上下文能够提供的只读事实；它们不借用运行中的程序帧。其他入口是否接受表达式仍由各自加载边界决定。整数表达式还可以用于下述数量字段。

`skillGrants[].while` 也使用同一表达式，参与者限 owner、量词 it 及 living/livingOthers，不允许帧绑定、选项、事件参与者或签发窗口事实。条件成立且精确父来源有效时，唯一物化器授予叶技能；条件退出只移除该派生来源，再次成立保留原实例。`ruleWounded` 复用现有规则受伤权威，不能用 Hp<MaxHp 替代；`equipmentSuitCount` 在当前求值已合格的独立改写来源上计算，不回最终技能绑定查询。两者可能依赖技能来源，受体持续限制的 raw while 不接受它们，避免自递归；效果及条件授予可用。公共堆数是 raw 事实，已知空堆返回 0，缺少参与者或读取能力则未知，不按同名技能猜同一实例。

完整来源、调用续接策略和迁移范围见[条件授予的统一实现](CAPABILITY_MAP.md#条件授予的统一实现)。

## 按回合和时点读取真实用牌

触发器的 `condition` 可使用同一表达式。触发资格仍在原资格阶段判断；该入口不接受 `number`、选项或牌绑定读取，也不为求值创建程序帧。依赖历史的窗口读取原 `HistoryPoint`，由实际回合、拥有者和包含待提交事实的完整 journal 游标确定截点；游标不是事件序号。缺少窗口 point 时 `window` 与 `currentUse` 为未知；明确的 `now` 则读取当前实际回合，不借用一个假窗口。

| 写法 | 结果 |
|---|---|
| `["cardUses","window","latest"]` | 原窗口截点每次用牌的最新已知事实 |
| `["cardUses","now","observations"]` | 当前实际回合逐次采样的事实；后续取消不抹除旧观察 |
| `["cardUses","window","confirmations"]` | 原窗口截点以前每次真实原生目标确认的快照，按确认发生顺序；目标未改变也保留真实确认 |
| `"currentUse"` / `"use"` | 当前窗口用牌／事实集合谓词中的成员 |
| `["useActor",fact]` / `["useDeclarer",fact]` | 采样时使用者／首次接受者，均不等于材料提供者 |
| `["useTargets",fact]` | Latest／Observations 为完整 designated 目标；Confirmations 为原 TargetsConfirmed 的 raw 目标 |
| `["useConfirmedTargets",fact]` | 最近一次真实确认的原始完整目标；没有确认元数据则未知 |
| `["usePrimaryTargets",fact]` | 最近一次真实确认的规范化主要目标；借刀只取持刀者，区别于包含 victim 的 raw 目标 |
| `["useKindIs",fact,["slash","fireSlash","thunderSlash"]]` | 匹配原生声明时牌种，不读后来改写的 EffectiveKind；数组使用共同 ProgramCardKindMatch，完整旧杀族包含冰杀，单项 slash 仍仅普通杀 |
| `["usePhaseIs",fact,"play"]` | 最近一次真实确认发生时的实际 TurnPhase，不是当前阶段；无采样元数据则未知 |
| `["useEstablished",fact]` / `["useDeclared",fact]` | 目标已在实际 finalized 时成立／已有真实原生声明身份；目标确认本身不等于 Established |
| `["useIsResponse",fact]` / `["useIsSelfNativeResponse",fact]` | 实际响应类型／既有自身原生响应协议 |
| `["atOrBefore",fact,"currentUse","accepted"]` | 原接受顺序的包含式截点；保留真实长整数 ID，无 Action 时读取真实声明顺序 |
| `["first",facts]` | 所选事实集的首项；空集或未知集返回未知，不把接受、确认或完成顺序互换 |
| `["sameUse",fact,fact]` | 双方有 native ID 时只比较该真实 ID；只有双方都没有 native ID 才比较真实 ActionId，单边缺失或没有可比身份时未知 |
| `["filter",set,predicate]` / `["targets",facts]` | 筛选事实或角色集合／按所选视图的 Targets 去重合并；不会隐式改成主要目标 |

`any`、`all`、`count` 同时支持事实与角色集合。事实谓词用 `use`，角色谓词用 `it`，两种迭代可嵌套，`first(filter(...))` 保留外层角色绑定；`contains` 判断角色是否在集合中。筛选谓词未知时集合仍未知，不能把未知成员当作不匹配跳过。`turnOwner` 是窗口或明确 now 读取的实际回合拥有者。各视图均支持 `window` 和 `now`，表达式自动声明历史采集需求，与当前是否持有技能分开。

Confirmations 每项是一份真实确认快照，后来更新不回写它。Latest／Observations 的 Targets 仍反映 designated 目标；其 `ConfirmedTargets`、`PrimaryTargets`、`Phase` 是最近确认留下的元数据，普通更新不擦除，也不能因此把更新当成新的确认。声明牌种同样独立于有效牌种，角色改写不制造另一条接受记录。

跨角色首次成为杀目标可由共同表达式组合，不需要人物历史查询：

```json
{"kind":"expression","expr":["sameUse",
  ["first",["filter",["cardUses","window","confirmations"],
    ["and",["useKindIs","use",["slash","fireSlash","thunderSlash"]],
      ["contains",["useConfirmedTargets","use"],"owner"]]]],
  "currentUse"]}
```

魅魂用首次 finalized 的原窗口 point 读取这一跨所有使用者的确认历史，不能加 actor==owner。刘琦屯江在实际资格时读取 `now/confirmations`，并以同一条件组合 `phaseCount` 的 Play entered/skipped 资格，使用 `evaluateConditionAtResolution:true` 保留求值时点。界 current 窃听则使用 pre-TurnEnd 的原窗口 point 和 raw 确认目标，不能接收结束后新增动作，也不能把借刀 victim 从目标域漏掉。真实确认与急袭的 finalized-established ever 关系是不同概念。

`selectTarget` / `selectTargets` 可用 `targetSetExpr` 提供角色集合，与原目标类型和实际合法域取交集；提交时仍走共同输入校验。已支付步骤需继续使用原帧冻结的集合，不随之后子结算的新事实刷新菜单。已迁移复合效果按各自加载合同要求 `targetSetExpr`，不能缺字段后静默回退为人物历史扫描；该格式要求与旧语法退役统一处理，不逐字段另升版本。表达式只读视图可在同一次求值内缓存，不承载 pending、支付或游标。

## 按回合和时点读取实际阶段

```json
["phaseCount","now","owner","play","entered"]
["phaseCount","window","eventTarget","all","skipped"]
```

`phaseCount` 返回非负整数。时点为 `now` 或原窗口的 `window`；参与者是阶段行动者，仍受该时点的实际回合及回合拥有者范围限制。阶段可为 `preparation`、`judgment`、`draw`、`play`、`discard`、`ending` 或 `all`，结果为 `entered` 或 `skipped`。缺少可用时点或参与者时返回未知；已有范围内没有对应事实才返回零。读取自动声明既有实际阶段事实需求。

entered 只计真实阶段开始，恢复旧 token 不增加次数；skipped 只计原生实际跳过边界。自然槽、插入阶段和额外摸牌各保留原 occurrence，不把请求、替代声明或付款次数当成实际结果。原窗口以完整 journal 游标为截点，后续子阶段不会倒灌。

entered 与 skipped 不互斥，也不能用阶段结束次数替代 entered。原生弃牌开始窗已经进入后，技能才决定跳过弃牌阶段时，两项都计入；只有发出跳过请求、尚未到达真实跳过边界时仍不计 skipped。

例如，要求至少实际进入过一次出牌阶段，且本回合没有任何一次出牌阶段被跳过：

```json
["and",
  [">",["phaseCount","now","owner","play","entered"],0],
  ["==",["phaseCount","now","owner","play","skipped"],0]]
```

同回合有多次 Play 时，一次 skip 就使该条件不成立。平寇可直接用 `phaseCount(...,"all","skipped")` 作数量表达式并在原步骤冻结；蒙态只判断相应种类是否大于零。跳阶段作为技能成本时仍读原支付事实，不能拿上述实际次数替换。

## 已签发的用牌加伤

```json
{"op":"addCurrentCardDamage","target":"owner","targetScope":"currentTarget",
 "amount":1,"cardKinds":["slash","fireSlash","thunderSlash"],"damageLayer":"cardDamage",
 "condition":{"kind":"expression","expr":[">=",["windowHp","eventTarget"],["windowHp","owner"]]}}
```

`targetScope` 默认为 `use`，也可指定 `currentTarget`；后者按原窗口目标签发，不限制为杀。`amountExpr` 与 `amount` 互斥，复用既有整数求值和未知值处理。`cardKinds` 可限制实际伤害牌种，完整旧杀族的寒冰杀匹配仍用共同匹配函数；未指定则不限制。`damageLayer` 默认为 `additionalDamage`，`cardDamage` 同时参与牌伤害层计算。

该动作接在真实用牌的 committed、finalized 或 Slash-before-response 窗口；当前目标范围要求后两者。数值在步骤执行时确定，归原用牌帧；具体伤害结算读取共同调整，连环传播继承已确定的伤害，不再加一次。修改当前伤害仍使用原伤害操作。技能的资格、成本和后续惩罚分别保留在它们的原窗口。

旧 `addCurrentTargetSlashDamage` 操作已退役，四份内容改用上例的作用域、条件与层，不保留解析别名。取消抵消和追猎的动态升伤仍属于各自规则。

## 本次目标响应规则的实时条件

原帧 `ResponseRule.While` 复用同一表达式求值器，`eventSource` 是原用牌者，`eventTarget` 是被要求响应的角色。代供材料者与这两个身份分开，不用其手牌替代响应者手牌。该条件只读明确支持的公共角色事实；不能引用程序绑定、技能有效性或冻结窗口数据。

迫袭使用 `["<=",["handCount","eventTarget"],["handCount","eventSource"]]` 实时比较；敢决在真实发行时把装备印刷花色冻结为常量，例如 `["==",["handPrintedSuitCount","eventTarget","spade"],0]`。牌的花色改写不会改变这个印刷花色判断。没有手牌数据的估值 world 返回 unknown，不根据隐藏手牌猜测；原生效果 world 与响应 world 读同一个实体牌区。

目前这类已付签发仍由原生操作调用共用 issuer，尚未增加通用的已付响应效果 JSON 节点。持续技能状态/条件授予的 raw while 加载边界未开放这个新花色读取，不能把这些作用域混用。

## 永久物理牌标记

```json
{ "op": "markInitialHand", "target": "owner", "mark": "zheng" }
{ "op": "markCards", "bind": "shown-cards", "mark": "zheng" }
```

`markInitialHand` 只用于开局触发，读取真实开局母帧在任何开局子结算之前冻结的原始手牌；原候选已获签发后，来源消失或拥有者死亡不会把这批牌改成稍后的手牌。`markCards` 读取已有牌集合绑定，不重新选择、移动或支付。两者写同一种共享实体事实，重复标记不重复产生同一标记；初始牌的原主人是独立的来源信息，后续移动不覆盖它。

`["cardMarkCount","target","zheng","hand"]` 按当时受体的手牌计算；在持续状态里 `target` 是受体，效果表达式使用其支持的参与者名称。该读取也接受 `equipment`，离开牌区会改变计数，但不删除永久实体标记。标记本身不授予手牌上限、距离或其他收益，这些规则仍声明自己的有效技能及对象条件。

展示文件用 `cardMarks: {"zheng":"筝"}` 指定标签，同一标记的不同声明必须使用同一文字；加载器拒绝冲突。UI 只为原本允许该观察者看到的实体附加文字，标记 ID 与标签不混用。翻转标记、离区到期租约及尚未提交的批次不是永久标记，不能直接套用本节点。

## 数量表达式

`draw`、`recover`、`loseHp`、`revealTopCards` 接受可选的 `amountExpr`，与 `amount`、`numberExpression` 按字段是否出现互斥；`damage` 仍保持原协议。表达式必须返回整数，使用与条件相同的节点、类型、深度与只读事实。

```json
{ "op": "draw", "target": "owner",
  "amountExpr": ["-", ["handLimit", "owner"], ["handCount", "owner"]] }
```

在条件通过、步骤实际执行时求值；默认未知跳过这一步，负数按零处理。可显式设置 `"onUnknownAmount":"cancelBinding"`，复用整绑定取消与清理路径。取消型数量表达式的 `hp/lostHp` 依赖及 `captureNumber.valueExpr` 会接回原帧的独立体力池查询，不默认选择一人；默认跳步表达式仍把未确定的体力当作未知。`when(hasHp, ...)` 可保留旧规则要求的“有体力值才计算”，不改变 `maxHp` 这个读取本身。

后续步骤会重新读取当时的状态；同一个步骤的子结算恢复则使用首次签发的数量，不因手牌或体力已经变化而重新计算。`selectedTargets` 使用同一次求值的数量，仍逐个完成原生摸牌与观察窗口。零量没有牌或体力支付；带结果绑定的零量摸牌/翻牌仍生成空集合，供后续步骤读取和清理。

求得的非负数量保存在原 `ProgramSkillFrame.NumberBindings` 中，按指令游标签发，并由 `ProgramAmountEvaluatedEvent` 与通用帧不变量核对。已有回复、体力变化、濒死和摸牌返回继续验证该数量及原帧身份。表达式求值本身只读；签发数量属于步骤执行协议，不新增 checkpoint 字段或格式版本。

表达式摸牌带 `resultBind`、或表达式翻牌时，必须使用 `cancelBinding`：未知时终止整个绑定，避免后续读取不存在的集合；已知零则保持空集合。翻牌集合仍须通过原有移动节点完整消费，即使运行时可能为空。AI 数量估值复用同一表达式求值器，公开装备数量沿现有上下文读取，拥有者手牌估值随前序步骤更新；缺失事实仍未知。

## 冻结数值与牌集合

```json
{ "op": "captureNumber", "target": "owner", "resultBind": "before",
  "valueExpr": ["handCount", "owner"] }
```

`captureNumber` 把整数值保存到原帧的命名绑定；后续 `["number","before"]` 不会重算。值可为零或负数，不按操作数量截断；未知会取消整绑定。绑定只允许创建一次。原生复合操作也可通过同一捕获入口声明输出，例如伏枥的入口阵营数与伤害历史，不再保存第二份数值状态。

`chooseOption` 的全部选项都声明整数 `number` 时，后续 `["choiceNumber","选项绑定名"]` 读取已接受选项的冻结整数；加载器验证生产顺序，数值不能由回答命令提供。`offerGeneratedEquipment.amountExpr` 可复用该整数。`["number","equipment.destroyedThisTurn"]` 是共享实体来源事实，读取当前技能、拥有者在本回合产生装备的销毁批次数；它不创建第二个可变计数器。

牌集合读取必须引用前面已生产的绑定。`boundCount` 读取原绑定实体数，分配后也不是剩余数；花色读取优先使用生产者已冻结的花色列表，否则读取绑定实体的物理花色。`boundSuitCount` 排除 `Suit.None`，`boundDistinctSuitCount` 包含它，以保留需要区分两种口径的规则。`boundRank` 和 `boundIsEquipment` 要求一个实体；牌面读取只使用公开绑定或拥有者可知的原手牌/装备绑定，其他私有绑定未知，AI 不猜牌面。

数值与牌集合引用在加载时统一验证生产顺序及命名冲突；读取元数据不消费牌集合。选择、支付、移牌、随机及待决游标仍由各原生操作负责。

## 绑定后分配

```json
{ "op": "selectPublicPile", "target": "owner", "amount": 4,
  "skillIds": ["classic:bizhuan"], "resultBind": "pile" }
```

随后可用 `distributeBoundCards`，指定 `sourceBind`、`targetKind:"otherLiving"` 和普通表达式条件。先选定具体来源，再对该绑定判断门槛；不是在多个牌堆之间猜一个集合。来源、实体和花色有精确签发凭据。分配复用原公开牌堆游标与移动原因，条件只在发行分配时检查一次；已付子结算返回后继续剩余实体，不重新判断门槛或再次支付。该操作只消费原始 `selectPublicPile` 输出，不能输入任意手牌、Processing 集合或同一集合重复分配。

数量表达式首批迁移、删除项、指标及保留原因见[数量表达式迁移记录](AMOUNT_EXPRESSIONS_MIGRATION.md)。
后续公共窗口链、绑定及分配抽象的完成记录见[公共抽象记录](DSL_ABSTRACTION_COMPLETION.md)。

## 迁移样例

| 技能 | 之前 | 之后 |
| --- | --- | --- |
| 移动界荀彧·节命 | `drawMobileJiemingOwnerIfRecipientShort`，两个人物文件约 5.5 KB | `draw` + 上例表达式 |
| 移动界廖化·伏枥 | `turnOverBoundaryLiaoHuaIfUniqueHighestHp`，描述符与运行方法约 1.6 KB | `turnOver` + `["and", ["hasHp","owner"], ["all","livingOthers", ["or", ["not",["hasHp","it"]], ["<",["hp","it"],["hp","owner"]]]]]` |

伏枥的正反两种情况由 `--filter="program expression"` 的真实对局检查覆盖。

## 新增人物时的规则

1. 先查是否能用“通用操作 + 表达式条件”表达。只是判断“是否执行某个已有操作”的，不新增专用操作、`numberExpression` 或条件种类。
2. 表达式缺少某个只读事实时，优先在 `ProgramExpressions` 与 `GameEngine.ProgramExpressions.cs` 中补一个通用读取项（并补该项的行为检查），而不是为单个人物写操作。
3. 仍需专用操作的情况：需要新的选择交互、支付、移牌协议、随机或跨步骤状态。

## 后续阶段

按收益排序，后续逐步扩展同一套表达式，而不是另建体系：

1. **数量表达式**：`draw`、`recover`、`loseHp`、`revealTopCards` 已接入，包含结果绑定和原体力查询池的迁移。当前无内容引用且没有恢复依赖的旧枚举分支一并删除；仍有内容使用的兼容值保留。其他效果逐项核对，`damage` 尚未接入。
2. **绑定读取**：已支持牌集合数量、花色、牌点、装备类别与帧内冻结数值。其他只读事实按实际规则需要补充，复用已有签发与资源依赖检查。
3. **随机与循环**：本批公开堆逐张分配复用原游标与付款续接；其他受控随机选取和“按座位依次”循环仍需分别界定操作、输入与返回协议。
4. **跨版本参数化**：同名技能的不同版本（如归心、神赋、清俭）共用一个操作并以参数区分。

每个阶段先迁移少量代表人物并量化删除的专用代码，再推广。

## 本次用牌的响应规则

`issueCurrentResponseRule` 在原 `CardUseCommitted`、`CardUseTargetsFinalized` 或 `SlashBeforeResponse` 窗口给真实用牌签发一行规则；不重新发动源技能、不模拟一个用牌帧。最低响应数按 max 合并，`forbid` 按 OR 合并。

```json
{ "op": "issueCurrentResponseRule", "target": "owner", "minimum": 2,
  "actorRef": {"kind":"eventSource"}, "opponentRef": {"kind":"eventSource"},
  "incomingKinds": ["duel"], "requiredKinds": ["slash"],
  "condition": {"kind":"choiceIs","choiceBind":"response-kind","optionId":"duel"} }
```

`audience` 默认 `targets`，`all` 包含原使用者和非目标无懈候选。`actorRef`、`opponentRef`、`responderRef` 可冻结 owner/eventSource/eventTarget，后者只限制该角色；没有声明则不增加这项过滤。`incomingKinds`、`requiredKinds` 限制响应牌种对，`matchUseKind` 另要求本次原牌种类与 incoming 一致。`while` 复用响应规则公开事实的表达式子集，在实际查询时读逻辑响应者和原使用者，不把材料提供者当成响应者。

无前的原生额外目标步骤通过现有 ChoiceBindings 输出 slash/duel，完成该指令后由普通 JSON 条件签发实际一对；支付、原返回及已接受步骤的失源履约仍在原帧。当前 JSON host 需要真实 CardUse 上下文，未覆盖没有该上下文的直接决斗或反无懈链节点。`all` 也不会把原牌禁令传播到后继无懈的独立节点，濒死救援保持自己的边界。
