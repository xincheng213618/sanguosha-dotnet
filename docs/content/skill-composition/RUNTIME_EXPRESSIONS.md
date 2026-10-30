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
| 运算 | `["+", i, ...]`、`["min", i, ...]`、`["max", i, ...]`（2–8 项）、`["-", i, i]`、`["floorDiv", i, i]`；`["when", b, i]` 在 b 为真时取 i，否则未知 |
| 绑定读取 | 整数：`["number", "name"]`、`["boundCount", "cards"]`、`["boundSuitCount", "cards"]`、`["boundDistinctSuitCount", "cards"]`、`["boundRank", "singleCard"]`；布尔：`["boundIsEquipment", "singleCard"]` |
| 量词 | `["all", set, b]`、`["any", set, b]` 返回布尔；`["count", set, b]` 返回整数；`b` 内用 `"it"` 表示当前成员 |

加载时检查运算符、参数个数、类型、字面量范围、嵌套深度（8 层）以及 `"it"` 只能出现在量词谓词中。表达式随规则 JSON 规范化后进入技能玩法指纹，空白不影响指纹；新增该可选节点不改变规则格式版本。

## 求值语义

- 在步骤执行时读取当前公开状态，不冻结到发动时。先完成的摸牌、移动及其子结算会影响后续步骤的判断。
- 三值逻辑：缺失的参与者（例如未选目标、事件没有来源）、未定义体力（`hp`、`lostHp`）以及未知集合都是“未知”。比较与运算遇到未知返回未知；`not` 保持未知；`and` 遇到确定的假即为假，`or` 遇到确定的真即为真；整个条件最终为未知时按假处理并跳过步骤。
- 因此需要“没有体力值的角色不参与比较”时要明确写出，例如 `["or", ["not", ["hasHp", "it"]], ["<", ["hp", "it"], ["hp", "owner"]]]`。
- 表达式只读，不支付、不移动、不发事件。拥有者死亡、失去技能实例和终局仍由执行器统一处理。
- AI 预估只知道拥有者与已选目标的公开状态；其他参与者和集合视为未知，所以依赖它们的步骤在估值中按不发动处理。
- `handLimit` 读取实际规则计算后的手牌上限；AI 只在公开上下文提供该值时使用。`floorDiv` 按数学意义向下取整，除零或商超出整数范围返回未知。未选定独立体力池而两个体力值不同时，`hp`、`lostHp` 也返回未知，不能默认选择其中一人；这与隐匿角色的 `hasHp=false` 不同。

## 适用范围

布尔表达式只允许出现在效果步骤的 `condition` 中（包括 `all`/`any`/`not` 组合内）。主动入口、选项、触发器和持续规则的条件在加载时拒绝表达式；这些位置没有运行中的程序帧。整数表达式还可以用于下述数量字段。

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
